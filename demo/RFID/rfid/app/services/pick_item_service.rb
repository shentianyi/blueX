class PickItemService

  def self.by_car_nr car_nr
    if order_car=OrderCar.find_by_nr(car_nr)
      if pick=Pick.joins(:orders).
          where(orders: {orderable_id: order_car.id, orderable_type: order_car.class.name}).order(id: :desc).first
        ApiMessage.new({
                           meta: {code: 200},
                           data:
                               PickItemPresenter.as_details(pick.pick_items)
                       })
      else
        ApiMessage.new({meta: {code: 400, error_message: 'NO Pick To do'}})
      end
    else
      ApiMessage.new({meta: {code: 400, error_message: 'Order Car not found'}})
    end
  end



end