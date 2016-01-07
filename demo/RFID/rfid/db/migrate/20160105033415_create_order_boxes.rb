class CreateOrderBoxes < ActiveRecord::Migration
  def change
    create_table :order_boxes do |t|
      t.string :nr
      t.string :rfid_nr
      t.integer :status, default: 100
      t.references :part, index: true, foreign_key: true
      t.float :quantity
      t.references :warehouse, index: true, foreign_key: true
      t.integer :source_warehouse_id
      t.references :order_box_type, index: true, foreign_key: true

      t.timestamps null: false
    end
  end
end
