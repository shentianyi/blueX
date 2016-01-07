class OrderCar < ActiveRecord::Base
  belongs_to :warehouse

  has_many :orders, :as => :orderable
end
