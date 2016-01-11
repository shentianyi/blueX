class CreatePickOrders < ActiveRecord::Migration
  def change
    create_table :pick_orders do |t|
      t.references :pick, index: true, foreign_key: true
      t.references :order, index: true, foreign_key: true

      t.timestamps null: false
    end
  end
end
