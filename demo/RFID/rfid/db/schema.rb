# encoding: UTF-8
# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema.define(version: 20160106221042) do

  create_table "colors", force: :cascade do |t|
    t.string   "nr",          limit: 255
    t.string   "name",        limit: 255
    t.string   "short_name",  limit: 255
    t.string   "description", limit: 255
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  create_table "locations", force: :cascade do |t|
    t.string   "nr",          limit: 255
    t.string   "name",        limit: 255
    t.string   "description", limit: 255
    t.integer  "parent_id",   limit: 4
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  create_table "move_types", force: :cascade do |t|
    t.string   "nr",          limit: 255
    t.string   "description", limit: 255
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  create_table "movements", force: :cascade do |t|
    t.integer  "part_id",           limit: 4
    t.string   "fifo",              limit: 255
    t.float    "quantity",          limit: 24
    t.string   "package_nr",        limit: 255
    t.string   "uniq_nr",           limit: 255
    t.integer  "from_position_id",  limit: 4
    t.integer  "from_warehouse_id", limit: 4
    t.integer  "to_position_id",    limit: 4
    t.integer  "to_warehouse_id",   limit: 4
    t.integer  "move_type_id",      limit: 4
    t.integer  "user_id",           limit: 4
    t.string   "remarks",           limit: 255
    t.datetime "created_at",                    null: false
    t.datetime "updated_at",                    null: false
  end

  create_table "oauth_access_grants", force: :cascade do |t|
    t.integer  "resource_owner_id", limit: 4,     null: false
    t.integer  "application_id",    limit: 4,     null: false
    t.string   "token",             limit: 255,   null: false
    t.integer  "expires_in",        limit: 4,     null: false
    t.text     "redirect_uri",      limit: 65535, null: false
    t.datetime "created_at",                      null: false
    t.datetime "revoked_at"
    t.string   "scopes",            limit: 255
  end

  add_index "oauth_access_grants", ["token"], name: "index_oauth_access_grants_on_token", unique: true, using: :btree

  create_table "oauth_access_tokens", force: :cascade do |t|
    t.integer  "resource_owner_id", limit: 4
    t.integer  "application_id",    limit: 4
    t.string   "token",             limit: 255, null: false
    t.string   "refresh_token",     limit: 255
    t.integer  "expires_in",        limit: 4
    t.datetime "revoked_at"
    t.datetime "created_at",                    null: false
    t.string   "scopes",            limit: 255
  end

  add_index "oauth_access_tokens", ["refresh_token"], name: "index_oauth_access_tokens_on_refresh_token", unique: true, using: :btree
  add_index "oauth_access_tokens", ["resource_owner_id"], name: "index_oauth_access_tokens_on_resource_owner_id", using: :btree
  add_index "oauth_access_tokens", ["token"], name: "index_oauth_access_tokens_on_token", unique: true, using: :btree

  create_table "oauth_applications", force: :cascade do |t|
    t.string   "name",         limit: 255,                null: false
    t.string   "uid",          limit: 255,                null: false
    t.string   "secret",       limit: 255,                null: false
    t.text     "redirect_uri", limit: 65535,              null: false
    t.string   "scopes",       limit: 255,   default: "", null: false
    t.datetime "created_at"
    t.datetime "updated_at"
    t.integer  "owner_id",     limit: 4
    t.string   "owner_type",   limit: 255
  end

  add_index "oauth_applications", ["owner_id", "owner_type"], name: "index_oauth_applications_on_owner_id_and_owner_type", using: :btree
  add_index "oauth_applications", ["uid"], name: "index_oauth_applications_on_uid", unique: true, using: :btree

  create_table "order_box_types", force: :cascade do |t|
    t.string   "name",        limit: 255
    t.string   "description", limit: 255
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  create_table "order_boxes", force: :cascade do |t|
    t.string   "nr",                  limit: 255
    t.string   "rfid_nr",             limit: 255
    t.integer  "status",              limit: 4,   default: 0
    t.integer  "part_id",             limit: 4
    t.float    "quantity",            limit: 24
    t.integer  "warehouse_id",        limit: 4
    t.integer  "source_warehouse_id", limit: 4
    t.integer  "order_box_type_id",   limit: 4
    t.datetime "created_at",                                  null: false
    t.datetime "updated_at",                                  null: false
  end

  add_index "order_boxes", ["order_box_type_id"], name: "index_order_boxes_on_order_box_type_id", using: :btree
  add_index "order_boxes", ["part_id"], name: "index_order_boxes_on_part_id", using: :btree
  add_index "order_boxes", ["warehouse_id"], name: "index_order_boxes_on_warehouse_id", using: :btree

  create_table "order_cars", force: :cascade do |t|
    t.string   "nr",           limit: 255
    t.string   "rfid_nr",      limit: 255
    t.integer  "warehouse_id", limit: 4
    t.integer  "status",       limit: 4,   default: 0
    t.datetime "created_at",                           null: false
    t.datetime "updated_at",                           null: false
  end

  add_index "order_cars", ["warehouse_id"], name: "index_order_cars_on_warehouse_id", using: :btree

  create_table "order_items", force: :cascade do |t|
    t.integer  "order_id",       limit: 4
    t.integer  "user_id",        limit: 4
    t.integer  "status",         limit: 4,   default: 0
    t.float    "quantity",       limit: 24
    t.integer  "part_id",        limit: 4
    t.integer  "orderable_id",   limit: 4
    t.string   "orderable_type", limit: 255
    t.boolean  "is_emergency"
    t.string   "remarks",        limit: 255
    t.datetime "created_at",                             null: false
    t.datetime "updated_at",                             null: false
  end

  add_index "order_items", ["order_id"], name: "index_order_items_on_order_id", using: :btree
  add_index "order_items", ["orderable_id"], name: "index_order_items_on_orderable_id", using: :btree
  add_index "order_items", ["orderable_type"], name: "index_order_items_on_orderable_type", using: :btree
  add_index "order_items", ["part_id"], name: "index_order_items_on_part_id", using: :btree
  add_index "order_items", ["user_id"], name: "index_order_items_on_user_id", using: :btree

  create_table "orders", force: :cascade do |t|
    t.integer  "user_id",        limit: 4
    t.integer  "status",         limit: 4,   default: 0
    t.integer  "orderable_id",   limit: 4
    t.string   "orderable_type", limit: 255
    t.string   "remarks",        limit: 255
    t.datetime "created_at",                             null: false
    t.datetime "updated_at",                             null: false
    t.integer  "warehouse_id",   limit: 4
    t.string   "nr",             limit: 255
  end

  add_index "orders", ["nr"], name: "index_orders_on_nr", using: :btree
  add_index "orders", ["orderable_id"], name: "index_orders_on_orderable_id", using: :btree
  add_index "orders", ["orderable_type"], name: "index_orders_on_orderable_type", using: :btree
  add_index "orders", ["user_id"], name: "index_orders_on_user_id", using: :btree
  add_index "orders", ["warehouse_id"], name: "index_orders_on_warehouse_id", using: :btree

  create_table "part_positions", force: :cascade do |t|
    t.integer  "part_id",           limit: 4
    t.integer  "position_id",       limit: 4
    t.float    "safe_stock",        limit: 24
    t.integer  "from_warehouse_id", limit: 4
    t.integer  "from_position_id",  limit: 4
    t.datetime "created_at",                   null: false
    t.datetime "updated_at",                   null: false
  end

  add_index "part_positions", ["part_id"], name: "index_part_positions_on_part_id", using: :btree
  add_index "part_positions", ["position_id"], name: "index_part_positions_on_position_id", using: :btree

  create_table "part_types", force: :cascade do |t|
    t.string   "nr",          limit: 255
    t.string   "name",        limit: 255
    t.string   "description", limit: 255
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  create_table "parts", force: :cascade do |t|
    t.string   "nr",                limit: 255
    t.string   "name",              limit: 255
    t.string   "description",       limit: 255
    t.string   "short_description", limit: 255
    t.integer  "part_type_id",      limit: 4
    t.integer  "color_id",          limit: 4
    t.integer  "measure_unit_id",   limit: 4
    t.integer  "purchase_unit_id",  limit: 4
    t.string   "custom_nr",         limit: 255
    t.float    "cross_section",     limit: 24
    t.float    "weight",            limit: 24
    t.datetime "created_at",                    null: false
    t.datetime "updated_at",                    null: false
  end

  add_index "parts", ["color_id"], name: "index_parts_on_color_id", using: :btree
  add_index "parts", ["part_type_id"], name: "index_parts_on_part_type_id", using: :btree

  create_table "pick_items", force: :cascade do |t|
    t.integer  "pick_id",       limit: 4
    t.integer  "status",        limit: 4,   default: 0
    t.integer  "warehouse_id",  limit: 4
    t.integer  "position_id",   limit: 4
    t.float    "quantity",      limit: 24
    t.integer  "part_id",       limit: 4
    t.boolean  "is_emergency"
    t.integer  "order_item_id", limit: 4
    t.string   "remarks",       limit: 255
    t.datetime "created_at",                            null: false
    t.datetime "updated_at",                            null: false
  end

  add_index "pick_items", ["order_item_id"], name: "index_pick_items_on_order_item_id", using: :btree
  add_index "pick_items", ["part_id"], name: "index_pick_items_on_part_id", using: :btree
  add_index "pick_items", ["pick_id"], name: "index_pick_items_on_pick_id", using: :btree
  add_index "pick_items", ["position_id"], name: "index_pick_items_on_position_id", using: :btree
  add_index "pick_items", ["warehouse_id"], name: "index_pick_items_on_warehouse_id", using: :btree

  create_table "pick_orders", force: :cascade do |t|
    t.integer  "pick_id",    limit: 4
    t.integer  "order_id",   limit: 4
    t.datetime "created_at",           null: false
    t.datetime "updated_at",           null: false
  end

  add_index "pick_orders", ["order_id"], name: "index_pick_orders_on_order_id", using: :btree
  add_index "pick_orders", ["pick_id"], name: "index_pick_orders_on_pick_id", using: :btree

  create_table "picks", force: :cascade do |t|
    t.integer  "user_id",      limit: 4
    t.integer  "status",       limit: 4,   default: 0
    t.integer  "warehouse_id", limit: 4
    t.string   "remarks",      limit: 255
    t.datetime "created_at",                           null: false
    t.datetime "updated_at",                           null: false
    t.string   "nr",           limit: 255
  end

  add_index "picks", ["nr"], name: "index_picks_on_nr", using: :btree
  add_index "picks", ["user_id"], name: "index_picks_on_user_id", using: :btree
  add_index "picks", ["warehouse_id"], name: "index_picks_on_warehouse_id", using: :btree

  create_table "positions", force: :cascade do |t|
    t.string   "nr",           limit: 255
    t.string   "name",         limit: 255
    t.string   "description",  limit: 255
    t.integer  "warehouse_id", limit: 4
    t.datetime "created_at",               null: false
    t.datetime "updated_at",               null: false
  end

  add_index "positions", ["warehouse_id"], name: "index_positions_on_warehouse_id", using: :btree

  create_table "settings", force: :cascade do |t|
    t.string   "var",         limit: 255,   null: false
    t.text     "value",       limit: 65535
    t.integer  "target_id",   limit: 4,     null: false
    t.string   "target_type", limit: 255,   null: false
    t.datetime "created_at"
    t.datetime "updated_at"
  end

  add_index "settings", ["target_type", "target_id", "var"], name: "index_settings_on_target_type_and_target_id_and_var", unique: true, using: :btree

  create_table "storages", force: :cascade do |t|
    t.integer  "part_id",      limit: 4
    t.datetime "fifo"
    t.float    "quantity",     limit: 24
    t.string   "package_nr",   limit: 255
    t.string   "uniq_nr",      limit: 255
    t.integer  "position_id",  limit: 4
    t.integer  "warehouse_id", limit: 4
    t.string   "remarks",      limit: 255
    t.datetime "created_at",               null: false
    t.datetime "updated_at",               null: false
  end

  create_table "unit_groups", force: :cascade do |t|
    t.string   "nr",          limit: 255
    t.string   "name",        limit: 255
    t.string   "description", limit: 255
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  create_table "units", force: :cascade do |t|
    t.string   "nr",            limit: 255
    t.string   "name",          limit: 255
    t.string   "short_name",    limit: 255
    t.string   "description",   limit: 255
    t.integer  "unit_group_id", limit: 4
    t.datetime "created_at",                null: false
    t.datetime "updated_at",                null: false
  end

  add_index "units", ["unit_group_id"], name: "index_units_on_unit_group_id", using: :btree

  create_table "users", force: :cascade do |t|
    t.string   "nr",                     limit: 255
    t.string   "name",                   limit: 255
    t.integer  "role_id",                limit: 4
    t.boolean  "can_delete"
    t.boolean  "can_edit"
    t.datetime "created_at",                                      null: false
    t.datetime "updated_at",                                      null: false
    t.string   "email",                  limit: 255, default: "", null: false
    t.string   "encrypted_password",     limit: 255, default: "", null: false
    t.string   "reset_password_token",   limit: 255
    t.datetime "reset_password_sent_at"
    t.datetime "remember_created_at"
    t.integer  "sign_in_count",          limit: 4,   default: 0,  null: false
    t.datetime "current_sign_in_at"
    t.datetime "last_sign_in_at"
    t.string   "current_sign_in_ip",     limit: 255
    t.string   "last_sign_in_ip",        limit: 255
  end

  add_index "users", ["email"], name: "index_users_on_email", unique: true, using: :btree
  add_index "users", ["reset_password_token"], name: "index_users_on_reset_password_token", unique: true, using: :btree

  create_table "warehouses", force: :cascade do |t|
    t.string   "nr",          limit: 255
    t.string   "name",        limit: 255
    t.string   "description", limit: 255
    t.integer  "type",        limit: 4
    t.integer  "parent_id",   limit: 4
    t.integer  "location_id", limit: 4
    t.datetime "created_at",              null: false
    t.datetime "updated_at",              null: false
  end

  add_index "warehouses", ["location_id"], name: "index_warehouses_on_location_id", using: :btree

  add_foreign_key "order_boxes", "order_box_types"
  add_foreign_key "order_boxes", "parts"
  add_foreign_key "order_boxes", "warehouses"
  add_foreign_key "order_cars", "warehouses"
  add_foreign_key "order_items", "orders"
  add_foreign_key "order_items", "parts"
  add_foreign_key "order_items", "users"
  add_foreign_key "orders", "users"
  add_foreign_key "part_positions", "parts"
  add_foreign_key "part_positions", "positions"
  add_foreign_key "parts", "colors"
  add_foreign_key "parts", "part_types"
  add_foreign_key "pick_items", "order_items"
  add_foreign_key "pick_items", "parts"
  add_foreign_key "pick_items", "picks"
  add_foreign_key "pick_items", "positions"
  add_foreign_key "pick_items", "warehouses"
  add_foreign_key "pick_orders", "orders"
  add_foreign_key "pick_orders", "picks"
  add_foreign_key "picks", "users"
  add_foreign_key "picks", "warehouses"
  add_foreign_key "positions", "warehouses"
  add_foreign_key "units", "unit_groups"
  add_foreign_key "warehouses", "locations"
end
