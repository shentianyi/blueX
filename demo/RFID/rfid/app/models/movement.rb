class Movement < ActiveRecord::Base
  belongs_to :move_type, class_name: 'MoveType'
  belongs_to :to_warehouse, class_name: 'Warehouse'
  belongs_to :from_warehouse, class_name: 'Warehouse'
  belongs_to :to_position, class_name: 'Position'
  belongs_to :from_position, class_name: 'Position'
  belongs_to :part, class_name: 'Part'
  belongs_to :user, class_name: 'User'


  def self.to_xlsx movements
    p = Axlsx::Package.new

    wb = p.workbook
    wb.add_worksheet(:name => "sheet1") do |sheet|
      sheet.add_row ["序号", "零件号", "超市位置", "FIFO", "数量", "唯一码", "源库位", "源仓库", "目的库位", "目的仓库", "移库类型", "创建者", "创建时间", "备注"]
      movements.each_with_index { |movement, index|
        unless movement.part.blank?

          cswz=movement.part.part_positions.where(from_warehouse_id: movement.from_warehouse_id).first
          sheet.add_row [
                            index+1,
                            movement.part.nr,
                            cswz.blank? ? '' : cswz.position.nr[0, 8],
                            movement.fifo,
                            movement.quantity,
                            movement.package_nr,
                            movement.from_position.blank? ? '' : movement.from_position.nr,
                            movement.from_warehouse.blank? ? '' : movement.from_warehouse.nr,
                            movement.to_position.blank? ? '' : movement.to_position.nr,
                            movement.to_warehouse.blank? ? '' : movement.to_warehouse.nr,
                            movement.move_type.blank? ? '' : movement.move_type.nr,
                            movement.user.blank? ? '' : movement.user.nr,
                            movement.created_at.localtime,
                            movement.remarks
                        ]
        end
      }
    end
    p.to_stream.read
  end
end
