json.array!(@order_cars) do |order_car|
  json.extract! order_car, :id, :nr, :rfid_nr, :warehouse_id, :status
  json.url order_car_url(order_car, format: :json)
end
