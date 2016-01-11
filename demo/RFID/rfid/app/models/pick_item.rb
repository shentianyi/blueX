class PickItem < ActiveRecord::Base
  belongs_to :pick
  belongs_to :order_item
  belongs_to :part
  belongs_to :warehouse
  belongs_to :position
end
