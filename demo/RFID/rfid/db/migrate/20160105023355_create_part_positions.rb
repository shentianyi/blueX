class CreatePartPositions < ActiveRecord::Migration
  def change
    create_table :part_positions do |t|
      t.references :part, index: true, foreign_key: true
      t.references :position, index: true, foreign_key: true
      t.float :safe_stock
      t.integer :from_warehouse_id
      t.integer :from_position_id

      t.timestamps null: false
    end
  end
end
