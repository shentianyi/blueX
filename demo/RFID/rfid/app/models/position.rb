class Position < ActiveRecord::Base
  belongs_to :warehouse
  has_many :part_positions, :dependent => :destroy
  has_many :parts, :through => :part_positions

  validates_presence_of :nr, :message => "库位编号不能为空!"
  validates_uniqueness_of :nr, :message => "库位编号不能重复!"
end
