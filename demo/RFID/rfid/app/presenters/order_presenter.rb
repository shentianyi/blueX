#encoding: utf-8
class OrderPresenter<Presenter
  Delegators=[:id, :user_id, :status, :orderable_id, :orderable_type, :remarks]
  def_delegators :@order, *Delegators

  def initialize(order)
    @order=order
    self.delegators =Delegators
  end


  def as_basic_info
    {
        id:  @order.id,
        user: UserPresenter.new(User.find_by_id(@order.user_id)).as_basic_info,
        status: OrderType.display(@order.status),
        order_car: OrderCarPresenter.new(@order.orderable).as_basic_info,
        remarks: @order.remarks
    }
  end

end