json.array!(@locations) do |location|
  json.extract! location, :id, :nr, :name, :description, :parent_id
  json.url location_url(location, format: :json)
end
