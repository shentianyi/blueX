class Position < ActiveRecord::Base
  belongs_to :warehouse
  has_many :part_positions, :dependent => :destroy
  has_many :parts, :through => :part_positions
end
