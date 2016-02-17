class PartType < ActiveRecord::Base
  def self.options
    self.all.map { |r| [r.nr, r.id] }
  end
end
