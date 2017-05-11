p = Axlsx::Package.new

wb = p.workbook
wb.add_worksheet(:name => "sheet") do |sheet|
  sheet.add_row ["序号", "料盒号", "零件号", "需求数量", "重量", "称重数量", "重量合规", "状态", "仓库号", "库位号", "需求项号", "备注"]

  unfinished = 0
  start_time = Time.now
  PickItem.where(status: PickItemStatus::PICKED, created_at: '2017-01-01'.to_time..Time.now).each_with_index do |pick_item, index|

    puts "Index: #{index}--------Start:#{start_time}---------CurrTime: #{Time.now}"
    condition = {}
    condition[:part_id] = pick_item.part_id
    order_box=pick_item.order_item.orderable
    if order_box
      #condition[:from_warehouse_id] = order_box.source_warehouse_id
      #condition[:to_position_id] = order_box.warehouse_id
      #condition[:to_warehouse_id] = order_box.position_id
      condition[:quantity] = [pick_item.weight_qty, order_box.quantity]
      condition[:remarks] = "RFID MOVE:#{order_box.nr}"
    else
      condition[:quantity] = pick_item.weight_qty
    end
    condition[:created_at] = [pick_item.created_at..pick_item.created_at+2.days]

    m=Movement.where(condition)

    unless m.first
      unfinished += 1
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
      puts "unfinished count:#{unfinished}"
    end

  end

end
p.serialize('unfinished_pick_items.xlsx')