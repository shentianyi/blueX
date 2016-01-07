class Part < ActiveRecord::Base
  belongs_to :part_type
  belongs_to :color
end
