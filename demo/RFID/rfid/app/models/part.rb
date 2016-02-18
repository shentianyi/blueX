class Part < ActiveRecord::Base
  belongs_to :part_type
  belongs_to :color

  validates_presence_of :nr, :message => "零件号不能为空!"
  validates_uniqueness_of :nr, :message => "零件号不能重复!"

  def display_unit id
    if u=Unit.find_by_id(id)
      u.nr
    else
      ''
    end
  end

  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end
end
