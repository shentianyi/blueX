json.array!(@movements) do |movement|
  json.extract! movement, :id, :part_id, :fifo, :quantity, :package_nr, :uniq, :from_position_id, :from_warehouse_id, :to_position_id, :to_warehouse_id, :move_type_id, :user_id, :remarks
  json.url movement_url(movement, format: :json)
end
