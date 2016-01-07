class ChangeColumnOfStorages < ActiveRecord::Migration
  def change
    rename_column :storages,:uniq,:uniq_nr
  end
end
