p = Axlsx::Package.new

wb = p.workbook
wb.add_worksheet(:name => "sheet") do |sheet|
  sheet.add_row ["序号", "料盒号", "零件号", "需求数量", "重量", "称重数量", "重量合规", "状态", "仓库号", "库位号", "需求项号", "备注"]
  PickItem.all.each_with_index do |pick_item, index|

    sheet.add_row [
                      index+1,
                      pick_item.orderable_nr,
                      pick_item.part.blank? ? '' : pick_item.part.nr,
                      pick_item.quantity,
                      pick_item.weight,
                      pick_item.weight_qty,
                      pick_item.weight_valid,
                      PickItemStatus.display(pick_item.status),
                      pick_item.warehouse.blank? ? '' : pick_item.warehouse.nr,
                      pick_item.position.blank? ? '' : pick_item.position.nr,
                      pick_item.order_item.blank? ? '' : pick_item.order_item.id,
                      pick_item.remarks
                  ]
  end

end
p.serialize('pick_items.xlsx')