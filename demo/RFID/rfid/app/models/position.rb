class Position < ActiveRecord::Base
  belongs_to :warehouse
  has_many :part_positions, :dependent => :destroy
  has_many :parts, :through => :part_positions, :dependent => :destroy

  validates_presence_of :nr, :message => "库位编号不能为空!"
  validates_uniqueness_of :nr, :message => "库位编号不能重复!"

  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end

  def self.to_xlsx positions
    p = Axlsx::Package.new

    wb = p.workbook
    wb.add_worksheet(:name => "sheet1") do |sheet|
      sheet.add_row ["序号", "编码", "名称", "描述", "所属仓库"]
      positions.each_with_index { |position, index|
        warehouse=Warehouse.find_by_id(position.warehouse_id)
        sheet.add_row [
                            index+1,
                            position.nr,
                            position.name,
                            position.description,
                            warehouse.blank? ? '' : warehouse.nr
                      ]
      }
    end
    p.to_stream.read
  end
end
