class OrderBox < ActiveRecord::Base
  belongs_to :warehouse
  belongs_to :position

  belongs_to :source_warehouse,class_name: 'Warehouse'
  belongs_to :part
  belongs_to :order_box_type
  has_many :order_items, :as => :orderable

  validates_presence_of :nr, :message => "料盒编号编号不能为空!"
  validates_uniqueness_of :nr, :message => "料盒编号不能重复!"
end
