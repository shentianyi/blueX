class ChangeColumnOfMovements < ActiveRecord::Migration
  def change
    rename_column :movements,:uniq,:uniq_nr
  end
end
