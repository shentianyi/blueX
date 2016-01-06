class Part < ActiveRecord::Base
  belongs_to :part
  belongs_to :color
end
