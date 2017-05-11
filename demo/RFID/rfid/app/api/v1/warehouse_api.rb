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
        params[:order_box_ids]=params[:order_box_ids].split(',') if params[:order_box_ids].present?
        WarehouseService.move_by_car(current_user, params)
      end

      params do
        optional :pick_id, type: Integer, desc: 'pick id'
      end
      post :move_by_pick do
        p params
        WarehouseService.move_by_pick(current_user, params)
      end

    end
  end
end