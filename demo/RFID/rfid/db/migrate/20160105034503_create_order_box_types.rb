class CreateOrderBoxTypes < ActiveRecord::Migration
  def change
    create_table :order_box_types do |t|
      t.string :name
      t.string :description

      t.timestamps null: false
    end
  end
end
