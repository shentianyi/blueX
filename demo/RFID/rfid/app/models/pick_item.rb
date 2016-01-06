class PickItem < ActiveRecord::Base
  belongs_to :pick
  belongs_to :order_item
end
