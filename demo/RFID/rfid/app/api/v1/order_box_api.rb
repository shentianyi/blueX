module V1
  class OrderBoxAPI < Base
    guard_all!

    namespace :order_boxes do
      params do
        requires :nr, type: String, desc: 'order box nr'
      end
      get :by_nr do
        OrderBoxService.details(params[:nr])
      end


    end
  end
end