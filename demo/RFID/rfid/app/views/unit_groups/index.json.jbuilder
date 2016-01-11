json.array!(@unit_groups) do |unit_group|
  json.extract! unit_group, :id, :nr, :name, :description
  json.url unit_group_url(unit_group, format: :json)
end
