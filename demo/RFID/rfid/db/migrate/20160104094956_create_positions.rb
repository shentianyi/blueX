class CreatePositions < ActiveRecord::Migration
  def change
    create_table :positions do |t|
      t.string :nr
      t.string :name
      t.string :description
      t.references :warehouse, index: true, foreign_key: true

      t.timestamps null: false
    end
  end
end
