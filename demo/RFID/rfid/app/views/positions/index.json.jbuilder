json.array!(@positions) do |position|
  json.extract! position, :id, :nr, :name, :description, :warehouse_id
  json.url position_url(position, format: :json)
end
