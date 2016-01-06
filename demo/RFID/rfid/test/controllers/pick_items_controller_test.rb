require 'test_helper'

class PickItemsControllerTest < ActionController::TestCase
  setup do
    @pick_item = pick_items(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:pick_items)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create pick_item" do
    assert_difference('PickItem.count') do
      post :create, pick_item: { is_emergency: @pick_item.is_emergency, order_item_id: @pick_item.order_item_id, part_id: @pick_item.part_id, pick_id: @pick_item.pick_id, position_id: @pick_item.position_id, quantity: @pick_item.quantity, remarks: @pick_item.remarks, status: @pick_item.status, warehouse_id: @pick_item.warehouse_id }
    end

    assert_redirected_to pick_item_path(assigns(:pick_item))
  end

  test "should show pick_item" do
    get :show, id: @pick_item
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @pick_item
    assert_response :success
  end

  test "should update pick_item" do
    patch :update, id: @pick_item, pick_item: { is_emergency: @pick_item.is_emergency, order_item_id: @pick_item.order_item_id, part_id: @pick_item.part_id, pick_id: @pick_item.pick_id, position_id: @pick_item.position_id, quantity: @pick_item.quantity, remarks: @pick_item.remarks, status: @pick_item.status, warehouse_id: @pick_item.warehouse_id }
    assert_redirected_to pick_item_path(assigns(:pick_item))
  end

  test "should destroy pick_item" do
    assert_difference('PickItem.count', -1) do
      delete :destroy, id: @pick_item
    end

    assert_redirected_to pick_items_path
  end
end
