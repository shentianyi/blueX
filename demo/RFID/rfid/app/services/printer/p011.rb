# print movement list
module Printer
  class P011<Base
    HEAD=[:id, :user_id, :created_at]
    BODY=[:part_id, :quantity, :box_quantity,:whouse_id, :is_emergency, :remark]

    def generate_data
      p = Pick.find_by_nr(self.id)
      # head={
      #     pick_nr: p.id,
      #     status: PickStatus.display(p.status),
      #     warehouse_id: p.warehouse.blank? ? '' : p.warehouse.nr,
      #     creator: p.user.blank? ? '' : p.user.nr,
      #     remarks: p.remarks,
      #     date: p.created_at.localtime.strftime('%Y.%m.%d %H:%M:%S')
      # }

      head={
          id: p.nr,
          user_id: p.user.blank? ? '' : p.user.nr,
          created_at: p.created_at.localtime.strftime('%Y.%m.%d %H:%M:%S')
      }
      heads=[]
      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end

      p.pick_items.each_with_index do |i, index|
        # body= {
        #     No: index+1,
        #     part_id: i.part_id,
        #     quantity: i.quantity,
        #     position_id: i.position.blank? ? '' : i.position.nr,
        #     warehouse_id: i.warehouse.blank? ? '' : i.warehouse.nr,
        #     status: PickItemStatus.display(i.status),
        #     is_emergency: i.is_emergency ? '是' : '否',
        #     remarks: i.remarks || ' '
        # }

        body= {
            part_id: i.part.blank? ? '' : i.part.nr,
            quantity: i.quantity,
            box_quantity: ' ',
            whouse_id: i.warehouse.blank? ? '' : i.warehouse.nr,
            status: PickItemStatus.display(i.status),
            is_emergency: i.is_emergency ? '是' : '否',
            remark: i.remarks || ' '
        }

        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
      end
    end
  end
end