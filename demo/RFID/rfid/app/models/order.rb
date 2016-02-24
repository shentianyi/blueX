class Order < ActiveRecord::Base
  include AutoKey
  belongs_to :user
  belongs_to :warehouse
  has_many :order_items,:dependent => :destroy
  has_many :pick_orders, :dependent => :destroy

  belongs_to :orderable, :polymorphic => true

#  validates_presence_of :nr, :message => "需求单号不能为空!"
#  validates_uniqueness_of :nr, :message => "需求单号不能重复!"
end
