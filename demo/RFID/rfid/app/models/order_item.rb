class OrderItem < ActiveRecord::Base
  belongs_to :order
  belongs_to :user
  belongs_to :orderable, :polymorphic => true
  belongs_to :part
end
