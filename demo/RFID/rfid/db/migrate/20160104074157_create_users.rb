class CreateUsers < ActiveRecord::Migration
  def change
    create_table :users do |t|
      t.string :nr
      t.string :name
      t.integer :role_id
      t.boolean :can_delete
      t.boolean :can_edit

      t.timestamps null: false
    end
  end
end
