module V1
  class OrderCarAPI < Base
    guard_all!

    namespace :order_cars do
      params do
        requires :nr, type: String, desc: 'order car nr'
      end
      get :by_nr do
        OrderCarService.details(params[:nr])
      end


    end
  end
end