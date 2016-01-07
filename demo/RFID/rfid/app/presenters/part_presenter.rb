
#encoding: utf-8
class PartPresenter<Presenter
  Delegators=[:id, :nr, :name, :description, :short_description, :part_type_id, :color_id, :measure_unit_id, :purchase_unit_id, :custom_nr, :cross_section, :weight]
  def_delegators :@part, *Delegators

  def initialize(part)
    @part=part
    self.delegators =Delegators
  end


  def as_basic_info
    if @part.nil?
      nil
    else
      {
          id: @part.id,
          nr: @part.nr
      }
    end
  end

end