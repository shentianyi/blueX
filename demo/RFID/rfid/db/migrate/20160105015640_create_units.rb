class CreateUnits < ActiveRecord::Migration
  def change
    create_table :units do |t|
      t.string :nr
      t.string :name
      t.string :short_name
      t.string :description
      t.integer :unit_group_id

      t.timestamps null: false
    end
  end
end
