require 'test_helper'

class PickOrdersControllerTest < ActionController::TestCase
  setup do
    @pick_order = pick_orders(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:pick_orders)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create pick_order" do
    assert_difference('PickOrder.count') do
      post :create, pick_order: { order_id: @pick_order.order_id, pick_id: @pick_order.pick_id }
    end

    assert_redirected_to pick_order_path(assigns(:pick_order))
  end

  test "should show pick_order" do
    get :show, id: @pick_order
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @pick_order
    assert_response :success
  end

  test "should update pick_order" do
    patch :update, id: @pick_order, pick_order: { order_id: @pick_order.order_id, pick_id: @pick_order.pick_id }
    assert_redirected_to pick_order_path(assigns(:pick_order))
  end

  test "should destroy pick_order" do
    assert_difference('PickOrder.count', -1) do
      delete :destroy, id: @pick_order
    end

    assert_redirected_to pick_orders_path
  end
end
