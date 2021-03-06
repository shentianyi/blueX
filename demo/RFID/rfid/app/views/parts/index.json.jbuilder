json.array!(@parts) do |part|
  json.extract! part, :id, :nr, :name, :description, :short_description, :part_type_id, :color_id, :measure_unit_id, :purchase_unit_id, :custom_nr, :cross_section, :weight
  json.url part_url(part, format: :json)
end
