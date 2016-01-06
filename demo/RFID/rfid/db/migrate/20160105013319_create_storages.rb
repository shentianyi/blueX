class CreateStorages < ActiveRecord::Migration
  def change
    create_table :storages do |t|
      t.integer :part_id
      t.datetime :fifo
      t.float :quantity
      t.string :package_nr
      t.string :uniq
      t.integer :position_id
      t.integer :warehouse_id
      t.string :remarks

      t.timestamps null: false
    end
  end
end
