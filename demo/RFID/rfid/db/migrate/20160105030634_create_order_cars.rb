class CreateOrderCars < ActiveRecord::Migration
  def change
    create_table :order_cars do |t|
      t.string :nr
      t.string :rfid_nr
      t.references :warehouse, index: true, foreign_key: true
      t.integer :status, default: 100

      t.timestamps null: false
    end
  end
end
