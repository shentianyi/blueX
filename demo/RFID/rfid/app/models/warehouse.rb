class Warehouse < ActiveRecord::Base

  self.inheritance_column = nil

  belongs_to :location
  has_many :positions
end
