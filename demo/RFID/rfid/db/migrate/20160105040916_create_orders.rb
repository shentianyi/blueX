class CreateOrders < ActiveRecord::Migration
  def change
    create_table :orders do |t|
      t.references :user, index: true, foreign_key: true
      t.integer :status, default: 0
      t.integer :orderable_id
      t.string :orderable_type
      t.string :remarks

      t.timestamps null: false
    end

    add_index :orders, :orderable_id
    add_index :orders, :orderable_type
  end
end
