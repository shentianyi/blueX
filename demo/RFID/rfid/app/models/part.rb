class Part < ActiveRecord::Base
  belongs_to :part_type
  belongs_to :color
  has_many :part_positions, :dependent => :destroy

  validates_presence_of :nr, :message => "零件号不能为空!"
  validates_uniqueness_of :nr, :message => "零件号不能重复!"

  def display_unit id
    if u=Unit.find_by_id(id)
      u.nr
    else
      ''
    end
  end

  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end

  def self.to_xlsx parts
    p = Axlsx::Package.new

    wb = p.workbook
    wb.add_worksheet(:name => "sheet1") do |sheet|
      sheet.add_row ["序号", "编码", "名称", "描述", "简述", "零件类型", "颜色", "计量单位", "销售单位", "客户号", "截面",	 "重量",	 "重量误差"]
      parts.each_with_index { |part, index|

        part_type = PartType.find_by_id(part.part_type_id)
        color = Color.find_by_id(part.color_id)
        rmeasure_unit = Unit.find_by_id(part.measure_unit_id)
        purchase_unit = Unit.find_by_id(part.purchase_unit_id)

        sheet.add_row [
                          index+1,
                          part.nr,
                          part.name,
                          part.description,
                          part.short_description,
                          part_type.blank? ? '' : part_type.nr,
                          color.blank? ? '' : color.nr,
                          rmeasure_unit.blank? ? '' : rmeasure_unit.nr,
                          purchase_unit.blank? ? '' : purchase_unit.nr,
                          part.custom_nr,
                          part.cross_section,
                          part.weight,
                          part.weight_range
                      ]
      }
    end
    p.to_stream.read
  end
end
