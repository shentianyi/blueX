class PartPosition < ActiveRecord::Base
  belongs_to :part
  belongs_to :position
end
