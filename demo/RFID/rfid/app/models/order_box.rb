class OrderBox < ActiveRecord::Base
  belongs_to :warehouse
  has_many :order_items, :as => :orderable
end
