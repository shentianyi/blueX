class Order < ActiveRecord::Base
  belongs_to :user

  belongs_to :orderable, :polymorphic => true
end
