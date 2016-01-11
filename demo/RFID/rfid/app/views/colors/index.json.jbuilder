json.array!(@colors) do |color|
  json.extract! color, :id, :nr, :name, :short_name, :description
  json.url color_url(color, format: :json)
end
