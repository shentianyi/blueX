json.array!(@part_positions) do |part_position|
  json.extract! part_position, :id, :part_id, :position_id, :safe_stock, :from_warehouse_id, :from_position_id
  json.url part_position_url(part_position, format: :json)
end
