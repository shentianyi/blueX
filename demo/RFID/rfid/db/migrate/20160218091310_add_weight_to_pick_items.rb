class AddWeightToPickItems < ActiveRecord::Migration
  def change
    add_column :pick_items, :weight, :float
  end
end
