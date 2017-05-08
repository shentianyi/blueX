p = Axlsx::Package.new

wb = p.workbook
wb.add_worksheet(:name => "sheet") do |sheet|
  sheet.add_row ["序号", "料盒号", "零件号", "需求数量", "重量", "称重数量", "重量合规", "状态", "仓库号", "库位号", "需求项号", "备注"]

  PickItem.all.each_with_index do |pick_item, index|

    condition = {}
    if pick_item.status!=PickItemStatus::PICKED
      next
    else
      condition[:part_id] = pick_item.part_id
      condition[:quantity] = pick_item.quantity
      order_box=pick_item.order_item.orderable
      if order_box
        condition[:from_warehouse_id] = order_box.source_warehouse_id
        condition[:to_position_id] = order_box.warehouse_id
        condition[:to_warehouse_id] = order_box.position_id
      end
      condition[:created_at] = [pick_item.created_at..pick_item.created_at+2.days]

      m=Movement.where(condition).where()
      if m
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


  end

end
p.serialize('unfinished_pick_items.xlsx')