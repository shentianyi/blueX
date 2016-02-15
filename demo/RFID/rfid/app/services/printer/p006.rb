# print pick list
module Printer
  class P006<Base
    HEAD=[:id, :user_id, :created_at]
    BODY=[:part_id, :quantity, :box_quantity,:whouse_id, :is_emergency, :remark]

    def generate_data
      p=PickList.find_by_id(self.id)
      head={id: p.id, user_id: p.user_id, created_at: p.created_at.localtime.strftime('%Y.%m.%d %H:%M:%S')}
      heads=[]
      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end

      pick_items=p.pick_items.where(state:PickItemState::PRINTING).order(state: :asc, is_emergency: :desc)
      if pick_items.count == 0
        pick_items = p.pick_items.order(state: :asc, is_emergency: :desc)
      end

      pick_items.each do |i|
        body= {
            part_id: i.part_id,
            quantity: i.quantity,
            box_quantity: i.box_quantity,
            whouse_id:i.destination_whouse_id,
            is_emergency: i.is_emergency ? '是' : ' ',
            remark: i.remark||' '
        }

        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
        i.update(state:PickItemState::PRINTED)
      end
    end
  end
end