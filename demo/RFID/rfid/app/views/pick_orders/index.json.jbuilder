json.array!(@pick_orders) do |pick_order|
  json.extract! pick_order, :id, :pick_id, :order_id
  json.url pick_order_url(pick_order, format: :json)
end
