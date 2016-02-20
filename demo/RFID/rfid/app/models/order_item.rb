class OrderItem < ActiveRecord::Base
  belongs_to :order
  belongs_to :user
  belongs_to :orderable, :polymorphic => true
  belongs_to :part
  has_many :pick_items


  def orderable_nr
    @orderable_nr||= (self.orderable.present? ? self.orderable.nr : '')
  end
end
