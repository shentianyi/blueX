class Storage < ActiveRecord::Base
  belongs_to :position
  belongs_to :warehouse, class_name: 'Warehouse'
  belongs_to :part, class_name: 'Part'
  belongs_to :user

  def self.to_xlsx storages
    p = Axlsx::Package.new

    wb = p.workbook
    wb.add_worksheet(:name => "sheet1") do |sheet|
      sheet.add_row ["序号", "零件", "FIFO", "数量", "唯一码", "库位", "仓库", "创建时间", "备注"]
      storages.each_with_index { |storage, index|
        sheet.add_row [
                          index+1,
                          storage.part.blank? ? '' : storage.part.nr,
                          storage.fifo.blank? ? '' : storage.fifo.localtime,
                          storage.quantity,
                          storage.package_nr,
                          storage.position.blank? ? '' : storage.position.nr,
                          storage.warehouse.blank? ? '' : storage.warehouse.nr,
                          storage.created_at.localtime,
                          storage.remarks
                      ]
      }
    end
    p.to_stream.read
  end
end
