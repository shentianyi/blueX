class Storage < ActiveRecord::Base
  belongs_to :position
  belongs_to :warehouse, class_name: 'Warehouse'
  belongs_to :part, class_name: 'Part'
  belongs_to :user
end
