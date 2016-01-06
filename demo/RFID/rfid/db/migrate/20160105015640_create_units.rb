class CreateUnits < ActiveRecord::Migration
  def change
    create_table :units do |t|
      t.string :nr
      t.string :name
      t.string :short_name
      t.string :description
      t.references :unit_group,index: true, foreign_key: true

      t.timestamps null: false
    end
  end
end
