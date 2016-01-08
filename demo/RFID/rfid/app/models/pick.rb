class Pick < ActiveRecord::Base
  include AutoKey

  has_many :pick_items,:dependent => :destroy

  has_many :pick_orders,:dependent => :destroy

  has_many :orders, through: :pick_orders

  belongs_to :user

  belongs_to :warehouse
end
