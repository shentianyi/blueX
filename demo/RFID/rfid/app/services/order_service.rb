class OrderService
  # require
  #  nr:string
  def self.create_by_car user, params
    begin
      # Order.transaction do
        validable_car_and_box(params) do |car, boxs|
          order=Order.new(status: OrderStatus::INIT)
          order.user=user

          boxs.each do |box|
            order_item=OrderItem.new({
                                         status: OrderStatus::INIT,
                                         quantity: box.quantity,
                                         part_id: box.part_id,
                                         is_emergency: 0
                                     })
            order_item.user=user
            order_item.order=order
            box.order_items<<order_item
          end

          if order.save
            car.orders<<order
            {
                meta: {
                    code: 200,
                    message:'Signed Success'
                },
                data: OrderPresenter.new(order).as_basic_info
            }
          else
            ApiMessage.new({meta: {code: 400, message: '生成需求单失败'}})
          end
        end
      end
    # rescue => e
    #   ApiMessage.new({meta: {code: 400, message: e.message}})
    # end
  end


  private
  def self.validable_car_and_box params
    if car=OrderCar.find_by_id(params[:order_car_id])
      err_infos=[]
      boxs=[]
      params[:order_box_ids].each do |box_id|
        unless box=OrderBox.find_by_id(box_id)
          err_infos<<"料盒#{box_id}没有找到!"
        end
        boxs<<box
      end

      if err_infos.size==0
        if block_given?
          yield(car, boxs)
        else
          ApiMessage.new({meta: {code: 200, message: '数据验证通过'}})
        end
      else
        ApiMessage.new({meta: {code: 400, message: err_infos.join(',')}})
      end
    else
      ApiMessage.new({meta: {code: 400, message: '料车没有找到'}})
    end
  end

end