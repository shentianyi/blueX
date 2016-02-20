class AddWeightValidToPickItems < ActiveRecord::Migration
  def change
    add_column :pick_items, :weight_valid, :boolean
  end
end
