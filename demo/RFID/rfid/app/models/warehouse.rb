class Warehouse < ActiveRecord::Base

  self.inheritance_column = nil

  belongs_to :parent, class_name: 'Warehouse'
  belongs_to :location
  has_many :positions, :dependent => :destroy
  has_many :order_boxes, :dependent => :destroy
  has_many :order_cars, :dependent => :destroy


  validates_presence_of :nr, :message => "仓库编号不能为空!"
  validates_uniqueness_of :nr, :message => "仓库编号不能重复!"

  after_save :create_position

  def create_position
    unless self.positions.where(nr: self.nr).first
      self.positions.create(nr: self.nr)
    end
  end

  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end
end
