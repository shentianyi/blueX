#encoding: utf-8
class OrderBoxTypePresenter<Presenter
  Delegators=[:id, :name, :description]
  def_delegators :@order_box_type, *Delegators

  def initialize(order_box_type)
    @order_box_type=order_box_type
    self.delegators =Delegators
  end


  def as_basic_info
    if @order_box_type.nil?
      nil
    else
      {
          id: @order_box_type.id,
          name: @order_box_type.name
      }
    end
  end

end