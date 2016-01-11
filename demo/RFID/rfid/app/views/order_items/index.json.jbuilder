json.array!(@order_items) do |order_item|
  json.extract! order_item, :id, :order_id, :user_id, :status, :quantity, :part_id, :orderable_id, :orderable_type, :is_emergency, :remarks
  json.url order_item_url(order_item, format: :json)
end
