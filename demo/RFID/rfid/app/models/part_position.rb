class PartPosition < ActiveRecord::Base
  belongs_to :part
  belongs_to :position

  belongs_to :from_position, class_name: 'Position'
  belongs_to :from_warehouse, class_name: 'Warehouse'
end
