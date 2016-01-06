class Pick < ActiveRecord::Base
  include AutoKey

  has_many :pick_items

  has_many :pick_orders

  has_many :orders, through: :pick_orders

  belongs_to :user
end
