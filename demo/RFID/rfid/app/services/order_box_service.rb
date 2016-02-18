class OrderBoxService
  # require
  #  nr:string
  def self.detail nr
    if ob=OrderBox.find_by_nr(nr)
      OrderBoxPresenter.new(ob).as_basic_feedback
    else
      ApiMessage.new({
                         meta: {
                             code: 400,
                             error_message: '未找到该料盒'
                         }
                     })
    end
  end

  def self.details nrs
    order_boxes=[]
    nrs.each do |nr|
      if ob=OrderBox.find_by_nr(nr)
        order_boxes<< OrderBoxPresenter.new(ob).as_data
      end
    end
    ApiMessage.new({
                       meta: {
                           code: 200
                       },
                       data: order_boxes
                   })

  end

  def self.weight_box(id,pick_item_id,weight)
    if box=OrderBox.find_by_id(id)
      box.update_attributes(status: OrderBoxStatus::PICKED)
    end
    if item=PickItem.find_by_id(pick_item_id)
      item.update_attributes(status: PickItemStatus::PICKED,weight:weight)
    end

    {
        meta: {
            code: 200,
            message: '称重成功'
        }
    }
  end
end