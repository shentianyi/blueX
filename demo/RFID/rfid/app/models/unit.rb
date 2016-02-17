class Unit < ActiveRecord::Base
  validates_presence_of :nr, :message => "单位编号不能为空!"
  validates_uniqueness_of :nr, :message => "单位编号不能重复!"

  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end
end
