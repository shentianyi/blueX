#encoding: utf-8
class PickPresenter<Presenter
  Delegators=[:id, :nr, :user_id, :status]
  def_delegators :@pick, *Delegators

  def initialize(pick)
    @pick=pick
    self.delegators =Delegators
  end


  def as_basic_info
    {
        id: @pick.id,
        nr: @pick.nr,
        status: @pick.status,
        remarks: @pick.remarks
    }
  end

  def orderable_nr
    if (order=@pick.orders.first) && (orderable=order.orderable)
      orderable.nr
    else
      ''
    end
  end

  def as_detail
    {
        id: @pick.id,
        nr: @pick.nr,
        status: @pick.status,
        orderable_nr: orderable_nr,
        remarks: @pick.remarks
    }
  end

  def self.as_details(picks)
    json=[]
    picks.each do |pick|
      json<<PickPresenter.new(pick)
    end
    json
  end

end