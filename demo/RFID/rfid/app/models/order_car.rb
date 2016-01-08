class OrderCar < ActiveRecord::Base
  belongs_to :warehouse

  has_many :orders, :as => :orderable

  validates_presence_of :nr, :message => "料车编号不能为空!"
  validates_uniqueness_of :nr, :message => "料车编号不能重复!"
end
