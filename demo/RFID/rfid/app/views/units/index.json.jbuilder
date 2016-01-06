json.array!(@units) do |unit|
  json.extract! unit, :id, :nr, :name, :short_name, :description, :unit_group_id
  json.url unit_url(unit, format: :json)
end
