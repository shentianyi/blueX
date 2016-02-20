class PickItem < ActiveRecord::Base
  belongs_to :pick
  belongs_to :order_item
  belongs_to :part
  belongs_to :warehouse
  belongs_to :position

  def orderable_nr
    @orderable_nr||= (self.order_item.present? ? self.order_item.orderable_nr : '')
  end

  def order_box
   return @order_box if @order_box.present?
    @order_box=self.order_item.orderable
  end


  def can_move_store?
    self.status==PickItemStatus::PICKED || (self.status==PickItemStatus::PICKING && self.order_box.order_box_type && Setting.not_need_weight_box_type_values.include?(self.order_box.order_box_type.name))
  end
end
