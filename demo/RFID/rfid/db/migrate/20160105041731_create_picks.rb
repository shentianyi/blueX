class CreatePicks < ActiveRecord::Migration
  def change
    create_table :picks do |t|
      t.references :user, index: true, foreign_key: true
      t.integer :status, default: 0
      t.integer :warehouse_id
      t.string :remarks

      t.timestamps null: false
    end
  end
end
