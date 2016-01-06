class CreateParts < ActiveRecord::Migration
  def change
    create_table :parts do |t|
      t.string :nr
      t.string :name
      t.string :description
      t.string :short_description
      t.references :part, index: true, foreign_key: true
      t.references :color, index: true, foreign_key: true
      t.integer :measure_unit_id
      t.integer :purchase_unit_id
      t.string :custom_nr
      t.float :cross_section
      t.float :weight

      t.timestamps null: false
    end
  end
end
