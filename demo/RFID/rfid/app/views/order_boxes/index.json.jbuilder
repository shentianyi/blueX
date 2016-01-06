json.array!(@order_boxes) do |order_box|
  json.extract! order_box, :id, :nr, :rfid_nr, :status, :part_id, :quantity, :warehouse_id, :source_warehouse_id, :order_box_type_id
  json.url order_box_url(order_box, format: :json)
end
