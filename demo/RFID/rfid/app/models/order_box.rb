class OrderBox < ActiveRecord::Base
  belongs_to :warehouse
  belongs_to :position

  belongs_to :source_warehouse,class_name: 'Warehouse'
  belongs_to :part
  belongs_to :order_box_type
  has_many :order_items, :as => :orderable

  before_save :set_default_position

  validates_presence_of :nr, :message => "料盒编号编号不能为空!"
  validates_uniqueness_of :nr, :message => "料盒编号不能重复!"

  def can_move_store?
    self.status==OrderBoxStatus::PICKED || (self.status==OrderBoxStatus::PICKING && self.order_box_type && Setting.not_need_weight_box_type_values.include?(self.order_box_type.name))
  end

  def set_default_position
    if self.position.blank?
      if p=self.warehouse.positions.where(nr:self.warehouse.nr).first
        self.position=p
      end
    end
  end


end
