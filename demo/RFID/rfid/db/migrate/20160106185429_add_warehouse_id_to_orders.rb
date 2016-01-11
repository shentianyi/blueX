class AddWarehouseIdToOrders < ActiveRecord::Migration
  def change
    add_column :orders, :warehouse_id, :integer
    add_index :orders,:warehouse_id
  end
end
