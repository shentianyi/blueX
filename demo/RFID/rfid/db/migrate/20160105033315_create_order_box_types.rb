class CreateOrderBoxTypes < ActiveRecord::Migration
  def change
    create_table :order_box_types do |t|
      t.string :name
      t.string :description
      t.float :weight, default: 0

      t.timestamps null: false
    end
  end
end
