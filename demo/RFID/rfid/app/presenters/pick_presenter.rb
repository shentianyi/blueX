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

end