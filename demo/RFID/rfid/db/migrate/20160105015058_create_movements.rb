class CreateMovements < ActiveRecord::Migration
  def change
    create_table :movements do |t|
      t.integer :part_id
      t.string :fifo
      t.float :quantity
      t.string :package_nr
      t.string :uniq
      t.integer :from_position_id
      t.integer :from_warehouse_id
      t.integer :to_position_id
      t.integer :to_warehouse_id
      t.integer :move_type_id
      t.integer :user_id
      t.string :remarks

      t.timestamps null: false
    end
  end
end
