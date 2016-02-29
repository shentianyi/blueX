class PartPosition < ActiveRecord::Base
  belongs_to :part
  belongs_to :position

  belongs_to :from_position, class_name: 'Position'
  belongs_to :from_warehouse, class_name: 'Warehouse'

  def self.to_xlsx part_positions
    p = Axlsx::Package.new

    wb = p.workbook
    wb.add_worksheet(:name => "sheet1") do |sheet|
      sheet.add_row ["序号", "零件号", "库位号", "安全库存", "默认源仓库", "默认源库位"]
      part_positions.each_with_index { |part_position, index|

        part = Part.find_by_id(part_position.part_id)
        position = Position.find_by_id(part_position.position_id)
        from_warehouse = Warehouse.find_by_id(part_position.from_warehouse_id)
        from_position = Position.find_by_id(part_position.from_position_id)

        sheet.add_row [
                          index+1,
                          part.blank? ? '' : part.nr,
                          position.blank? ? '' : position.nr,
                          part_position.safe_stock,
                          from_warehouse.blank? ? '' : from_warehouse.nr,
                          from_position.blank? ? '' : from_position.nr
                      ]
      }
    end
    p.to_stream.read
  end
end
