json.array!(@users) do |user|
  json.extract! user, :id, :nr, :name, :email, :role_id, :encrypted_password
  json.url user_url(user, format: :json)
end
