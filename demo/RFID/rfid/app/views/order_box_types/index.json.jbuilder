json.array!(@order_box_types) do |order_box_type|
  json.extract! order_box_type, :id, :name, :description
  json.url order_box_type_url(order_box_type, format: :json)
end
