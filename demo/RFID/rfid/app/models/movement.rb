class Movement < ActiveRecord::Base
  belongs_to :move_type, class_name: 'MoveType'
  belongs_to :to_warehouse, class_name: 'Warehouse'
  belongs_to :from_warehouse, class_name: 'Warehouse'
  belongs_to :to_position, class_name: 'Position'
  belongs_to :from_position, class_name: 'Position'
  belongs_to :part, class_name: 'Part'
  belongs_to :user, class_name: 'User'
end
