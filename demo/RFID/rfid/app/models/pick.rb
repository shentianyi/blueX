class Pick < ActiveRecord::Base
  include AutoKey

  has_many :pick_items,:dependent => :destroy

  has_many :pick_orders,:dependent => :destroy

  has_many :orders, through: :pick_orders

  belongs_to :user

  belongs_to :warehouse

#  validates_presence_of :nr, :message => "择货单号不能为空!"
#  validates_uniqueness_of :nr, :message => "择货单号不能重复!"
end
