class CreateSettings < ActiveRecord::Migration
  def change
    create_table :settings do |t|
      t.string :code
      t.string :name
      t.string :value
      t.integer :type

      t.timestamps
    end
  end
end
