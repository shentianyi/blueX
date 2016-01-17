class PickService
  # require
  #  nr:string
  def self.create_by_car user, params
    #begin
    Pick.transaction do
      validable_car_and_box(params) do |car, boxs|
        order=Order.new(status: OrderStatus::PICKING)
        order.user=user
        order.warehouse=car.warehouse

        order.orderable=car
        car.update_attributes(status: OrderCarStatus::PICKING)
        car.orders<<order
        boxs.each do |box|
          box.update_attributes(status: OrderBoxStatus::PICKING)
          order_item=OrderItem.new({
                                       quantity: box.quantity,
                                       part_id: box.part_id,
                                       is_emergency: false
                                   })
          order_item.user=user
          order_item.order=order
          order_item.orderable=box
          box.order_items<<order_item
          order.order_items<<order_item
        end

        pick=Pick.new(status: PickStatus::PICKING)
        pick.user=user

        order.order_items.each do |item|
          order_box=item.orderable
          pick_item=PickItem.new(
              warehouse_id: order_box.source_warehouse_id,
              part_id: item.part_id,
              quantity: item.quantity,
              status: PickStatus::PICKING
          )
          pick_item.order_item=item
          pick.pick_items<<pick_item
        end

        pick.orders<<order

        if pick.save
          {
              meta: {
                  code: 200,
                  message: 'Creates Success'
              },
              data: PickPresenter.new(pick).as_basic_info
          }
        else
          ApiMessage.new({meta: {code: 400, error_message: '生成需求单失败'}})
        end
      end
    end
    # rescue => e
    #   ApiMessage.new({meta: {code: 400, message: e.message}})
    # end
  end


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
          ApiMessage.new({meta: {code: 200, error_message: '数据验证通过'}})
        end
      else
        ApiMessage.new({meta: {code: 400, error_message: err_infos.join(',')}})
      end
    else
      ApiMessage.new({meta: {code: 400, error_message: '料车没有找到'}})
    end
  end


  def self.by_status status
    PickPresenter.as_details(Pick.where(status: status).all)
  end

end