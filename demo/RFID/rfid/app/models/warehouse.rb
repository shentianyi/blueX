class Warehouse < ActiveRecord::Base
  belongs_to :location
  has_many :positions
end
