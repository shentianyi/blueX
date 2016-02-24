class Pick < ActiveRecord::Base
  include AutoKey

  has_many :pick_items,:dependent => :destroy

  has_many :pick_orders,:dependent => :destroy

  has_many :orders, through: :pick_orders, :dependent => :destroy

  belongs_to :user

  belongs_to :warehouse

#  validates_presence_of :nr, :message => "择货单号不能为空!"
#  validates_uniqueness_of :nr, :message => "择货单号不能重复!"


  def self.by_order_car order_car
    Pick.joins(:orders).
        where(orders: {orderable_id: order_car.id, orderable_type: order_car.class.name})
        .order(id: :desc).first
  end
end
