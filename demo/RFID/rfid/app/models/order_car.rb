class OrderCar < ActiveRecord::Base
  has_many :orders, :as => :orderable
end
