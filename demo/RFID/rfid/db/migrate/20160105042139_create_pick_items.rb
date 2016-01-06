class CreatePickItems < ActiveRecord::Migration
  def change
    create_table :pick_items do |t|
      t.references :pick, index: true, foreign_key: true
      t.integer :status, default: 0
      t.integer :warehouse_id
      t.integer :position_id
      t.float :quantity
      t.integer :part_id
      t.boolean :is_emergency
      t.integer :order_item_id
      t.string :remarks

      t.timestamps null: false
    end
  end
end
