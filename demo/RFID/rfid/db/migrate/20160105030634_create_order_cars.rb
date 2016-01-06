class CreateOrderCars < ActiveRecord::Migration
  def change
    create_table :order_cars do |t|
      t.string :nr
      t.string :rfid_nr
      t.integer :warehouse_id
      t.integer :status, default: 0

      t.timestamps null: false
    end
  end
end
