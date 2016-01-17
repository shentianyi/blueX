class AddWeightRangeToParts < ActiveRecord::Migration
  def change
    add_column :parts, :weight_range, :float,default: 0.1
  end
end
