module V1
  class PickAPI < Base
    guard_all!

    namespace :picks do
      params do
        requires :order_car_id, type: Integer, desc: 'order car id'
        optional :order_box_ids, type: String, desc: 'order box ids'
      end
      post :create_by_car do
        Rails.logger.debug '-----------------------'
        Rails.logger.debug params[:order_box_ids]
        Rails.logger.debug '-----------------------'
        params[:order_box_ids]=params[:order_box_ids].split(',')
        PickService.create_by_car(current_user, params)
      end

      params do
        requires :status, type: Integer, desc: 'Pick Status'
      end
      get :by_status do
        PickService.by_status(status)
      end

    end
  end
end