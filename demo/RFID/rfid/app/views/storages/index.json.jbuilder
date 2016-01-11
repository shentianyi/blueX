json.array!(@storages) do |storage|
  json.extract! storage, :id, :part_id, :fifo, :quantity, :package_nr, :uniq_nr, :position_id, :warehouse_id
  json.url storage_url(storage, format: :json)
end
