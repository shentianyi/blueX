class PickItem < ActiveRecord::Base
  belongs_to :pick
  belongs_to :order_item
  belongs_to :part
  belongs_to :warehouse
  belongs_to :position

  def orderable_nr
    @orderable_nr||= (self.order_item.present? ? self.order_item.orderable_nr : '')
  end

  def order_box
   return @order_box if @order_box.present?
    @order_box=self.order_item.orderable
  end


  def can_move_store?
    self.status==PickItemStatus::PICKED || (self.status==PickItemStatus::PICKING && self.order_box.order_box_type && Setting.not_need_weight_box_type_values.include?(self.order_box.order_box_type.name))
  end

  def self.generate_unfinished_data date_start, date_end, part
    data=[]
    if part
      pick_items = PickItem.where(status: PickItemStatus::PICKED, created_at: date_start.to_time.utc..date_end.to_time.utc, part_id: part.id)
    else
      pick_items = PickItem.where(status: PickItemStatus::PICKED, created_at: date_start.to_time.utc..date_end.to_time.utc)
    end

    pick_items.each_with_index do |pick_item, index|
      condition={}
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
      condition[:created_at] = [pick_item.created_at.utc..(pick_item.created_at+3.days).utc]

      if Movement.where(condition).count ==0
        data<<{
            order_box_nr: pick_item.orderable_nr,
            order_box_from_wh: order_box.blank? ? '' : (order_box.source_warehouse.blank? ? '' : order_box.source_warehouse.nr),
            order_box_to_wh: order_box.blank? ? '' : (order_box.warehouse.blank? ? '' : order_box.warehouse.nr),
            order_box_to_po: order_box.blank? ? '' : (order_box.position.blank? ? '' : order_box.position.nr),
            part_nr: pick_item.part.blank? ? '' : pick_item.part.nr,
            order_qty: pick_item.quantity,
            weight: pick_item.weight,
            weight_qty: pick_item.weight_qty,
            weight_valid: pick_item.weight_valid ? '是' : '否',
            created_at: pick_item.created_at.localtime,
            weigh_time: pick_item.updated_at.localtime,
            status: PickItemStatus.display(pick_item.status),
            remarks: pick_item.remarks,
            pick_nr: pick_item.pick.nr

        }
      end
    end

    puts '--------------------------------------------------------------------'
    puts data.count
    puts '--------------------------------------------------------------------'

    data
  end

end
