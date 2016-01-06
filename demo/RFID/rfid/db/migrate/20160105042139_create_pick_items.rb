class CreatePickItems < ActiveRecord::Migration
  def change
    create_table :pick_items do |t|
      t.references :pick, index: true, foreign_key: true
      t.integer :status, default: 100

      t.references :warehouse, index: true, foreign_key: true
      # t.integer :warehouse_id

      t.references :position, index: true, foreign_key: true
      # t.integer :position_id
      t.float :quantity

      t.references :part, index: true, foreign_key: true
      # t.integer :part_id

      t.boolean :is_emergency

      t.references :order_item, index: true, foreign_key: true
      # t.integer :order_item_id
      t.string :remarks

      t.timestamps null: false
    end
  end
end
