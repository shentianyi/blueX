class Warehouse < ActiveRecord::Base

  self.inheritance_column = nil

  belongs_to :parent, class_name: 'Warehouse'
  belongs_to :location
  has_many :positions

  validates_presence_of :nr, :message => "仓库编号不能为空!"
  validates_uniqueness_of :nr, :message => "仓库编号不能重复!"


end
