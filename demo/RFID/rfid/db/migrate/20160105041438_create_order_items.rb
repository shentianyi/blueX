class CreateOrderItems < ActiveRecord::Migration
  def change
    create_table :order_items do |t|
      t.references :order, index: true, foreign_key: true
      t.references :user, index: true, foreign_key: true
      t.integer :status, default: 100
      t.float :quantity
      t.references :part, index: true, foreign_key: true
      t.integer :orderable_id, index: true
      t.string :orderable_type, index: true
      t.boolean :is_emergency
      t.string :remarks

      t.timestamps null: false
    end
  end
end
