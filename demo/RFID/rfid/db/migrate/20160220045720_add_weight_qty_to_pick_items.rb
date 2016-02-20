class AddWeightQtyToPickItems < ActiveRecord::Migration
  def change
    add_column :pick_items, :weight_qty, :float
  end
end
