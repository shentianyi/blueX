#encoding: utf-8
class PickItemPresenter<Presenter
  Delegators=[:id, :status]
  def_delegators :@pick_item, *Delegators

  def initialize(pick_item)
    @pick_item=pick_item
    self.delegators =Delegators
  end


  def as_basic_info
    {
        id: @pick_item.id,
        status: @pick_item.status
    }
  end

  def orderable
    if (order_item=@pick_item.order_item) && (orderable=order_item.orderable)
      orderable
    else
      nil
    end
  end

  def as_detail
    o=self.orderable

    {
        id: @pick_item.id,
        status: @pick_item.status,
        quantity:@pick_item.quantity,
        order_box: o.nil? ? nil :  OrderBoxPresenter.new(o).as_basic_info,
        part: PartPresenter.new(@pick_item.part).as_basic_info
    }
  end

  def self.as_details(pick_items)
    json=[]
    pick_items.each do |pick_item|
      json<<PickItemPresenter.new(pick_item).as_detail
    end
    json
  end

end