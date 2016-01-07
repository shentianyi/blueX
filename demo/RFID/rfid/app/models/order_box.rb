class OrderBox < ActiveRecord::Base
  belongs_to :warehouse
  belongs_to :source_warehouse,class_name: 'Warehouse'
  belongs_to :part
  belongs_to :order_box_type
  has_many :order_items, :as => :orderable
end
