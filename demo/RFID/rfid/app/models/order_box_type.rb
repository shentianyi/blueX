class OrderBoxType < ActiveRecord::Base
  def self.options
    self.all.map { |r| [r.name, r.id] }
  end
end
