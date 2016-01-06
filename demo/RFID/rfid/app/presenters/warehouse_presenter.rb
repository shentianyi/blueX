
#encoding: utf-8
class WarehousePresenter<Presenter
  Delegators=[:id, :nr, :name, :description, :type, :parent_id, :location_id]
  def_delegators :@warehouse, *Delegators

  def initialize(warehouse)
    @warehouse=warehouse
    self.delegators =Delegators
  end


  def as_basic_info
    if @warehouse.nil?
      nil
    else
      {
          id: @warehouse.id,
          nr: @warehouse.nr,
          name: @warehouse.name
      }
    end
  end

end