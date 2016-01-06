class CreatePartTypes < ActiveRecord::Migration
  def change
    create_table :part_types do |t|
      t.string :nr
      t.string :name
      t.string :description

      t.timestamps null: false
    end
  end
end
