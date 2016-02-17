class Warehouse < ActiveRecord::Base

  self.inheritance_column = nil

  belongs_to :parent, class_name: 'Warehouse'
  belongs_to :location
  has_many :positions
  has_many :order_boxes


  validates_presence_of :nr, :message => "仓库编号不能为空!"
  validates_uniqueness_of :nr, :message => "仓库编号不能重复!"

  after_save :create_position

  def create_position
    position=Position.create({nr: "#{self.nr}_default"})
    self.positions<<position
  end

  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end
end
