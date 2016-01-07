class Order < ActiveRecord::Base
  include AutoKey
  belongs_to :user
  belongs_to :warehouse
  has_many :order_items
  has_many :pick_orders

  belongs_to :orderable, :polymorphic => true
end
