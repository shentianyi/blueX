module V1
  class WarehouseAPI<Base
    guard_all!

    namespace :warehouses do
      params do
        requires :order_car_id, type: Integer, desc: 'order car id'
        optional :order_box_ids, type: String, desc: 'order box ids'
      end
      post :move_by_car do
        Rails.logger.debug '-----------------------'
        Rails.logger.debug params[:order_box_ids]
        Rails.logger.debug '-----------------------'
        params[:order_box_ids]=params[:order_box_ids].split(',')
        WarehouseService.move_by_car(current_user,params)
      end

    end
  end
end