class CreateUnitGroups < ActiveRecord::Migration
  def change
    create_table :unit_groups do |t|
      t.string :nr
      t.string :name
      t.string :description

      t.timestamps null: false
    end
  end
end
