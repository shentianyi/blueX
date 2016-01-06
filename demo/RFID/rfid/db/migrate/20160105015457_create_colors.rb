class CreateColors < ActiveRecord::Migration
  def change
    create_table :colors do |t|
      t.string :nr
      t.string :name
      t.string :short_name
      t.string :description

      t.timestamps null: false
    end
  end
end
