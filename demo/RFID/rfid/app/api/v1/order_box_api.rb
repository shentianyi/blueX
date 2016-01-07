module V1
  class OrderBoxAPI < Base
    guard_all!

    namespace :order_boxes do
      params do
        requires :nr, type: String, desc: 'order box nr'
      end
      get :by_nr do
        OrderBoxService.detail(params[:nr])
      end


      params do
        requires :nrs, type:String,desc: 'order box nrs'
      end
      get :by_nrs do
        params[:nrs]=params[:nrs].split(',')
        OrderBoxService.details(params[:nrs])
      end



    end
  end
end