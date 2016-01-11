class CreateMoveTypes < ActiveRecord::Migration
  def change
    create_table :move_types do |t|
      t.string :nr
      t.string :description

      t.timestamps null: false
    end
  end
end
