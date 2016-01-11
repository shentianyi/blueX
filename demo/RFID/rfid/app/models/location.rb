class Location < ActiveRecord::Base
  belongs_to :parent, class_name: 'Location'
  has_many :warehouses, dependent: :destroy

  validates_presence_of :nr, :message => "地点编号不能为空!"
  validates_uniqueness_of :nr, :message => "地点编号不能重复!"
end
