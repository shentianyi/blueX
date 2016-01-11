class AddPositionIdToOrderBoxes < ActiveRecord::Migration
  def change
    add_column :order_boxes, :position_id, :integer
    add_index :order_boxes,:position_id
  end
end
