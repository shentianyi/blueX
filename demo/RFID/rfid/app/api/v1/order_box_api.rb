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
        requires :nrs, type: String, desc: 'order box nrs'
      end
      get :by_nrs do
        params[:nrs]=params[:nrs].split(',')
        OrderBoxService.details(params[:nrs])
      end


      params do
        requires :id, type: Integer, desc: 'order box id'
        requires :pick_item_id, type: Integer, desc: 'pick item id'
        requires :weight, type: Float, desc: 'weight'
        requires :weight_qty, type: Float, desc: 'weight qty'
        requires :weight_valid, type: Boolean, desc: 'weight valid'
      end
      post :weight do
        OrderBoxService.weight_box(params)
      end

    end
  end
end