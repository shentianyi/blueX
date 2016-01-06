class CreateOrderBoxes < ActiveRecord::Migration
  def change
    create_table :order_boxes do |t|
      t.string :nr
      t.string :rfid_nr
      t.integer :status
      t.integer :part_id
      t.float :quantity
      t.references :warehouse, index: true, foreign_key: true
      t.integer :source_warehouse_id
      t.integer :order_box_type_id

      t.timestamps null: false
    end
  end
end
