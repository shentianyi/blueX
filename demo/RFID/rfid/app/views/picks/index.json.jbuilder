json.array!(@picks) do |pick|
  json.extract! pick, :id, :user_id, :status, :warehouse_id, :remarks
  json.url pick_url(pick, format: :json)
end
