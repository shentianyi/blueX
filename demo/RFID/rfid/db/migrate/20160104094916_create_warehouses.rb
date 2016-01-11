class CreateWarehouses < ActiveRecord::Migration
  def change
    create_table :warehouses do |t|
      t.string :nr
      t.string :name
      t.string :description
      t.integer :type
      t.integer :parent_id
      t.references :location, index: true, foreign_key: true

      t.timestamps null: false
    end
  end
end
