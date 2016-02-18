class OrderBoxType < ActiveRecord::Base
  has_many :order_boxes



  def self.options
    self.all.map { |r| [r.name, r.id] }
  end
end
