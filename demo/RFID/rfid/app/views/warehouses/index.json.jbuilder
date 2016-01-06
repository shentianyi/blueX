json.array!(@warehouses) do |warehouse|
  json.extract! warehouse, :id, :nr, :name, :description, :type, :parent_id, :location_id
  json.url warehouse_url(warehouse, format: :json)
end
