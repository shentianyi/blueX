class PickOrder < ActiveRecord::Base
  belongs_to :pick
  belongs_to :order
end
