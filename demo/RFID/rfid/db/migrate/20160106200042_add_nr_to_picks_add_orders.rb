class AddNrToPicksAddOrders < ActiveRecord::Migration
  def change
    add_column :orders,:nr,:string
    add_column :picks,:nr,:string

    add_index :orders,:nr
    add_index :picks,:nr
  end
end
