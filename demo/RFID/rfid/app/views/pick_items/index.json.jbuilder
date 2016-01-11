json.array!(@pick_items) do |pick_item|
  json.extract! pick_item, :id, :pick_id, :status, :warehouse_id, :position_id, :quantity, :part_id, :is_emergency, :order_item_id, :remarks
  json.url pick_item_url(pick_item, format: :json)
end
