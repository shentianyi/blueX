require 'test_helper'

class OrderBoxesControllerTest < ActionController::TestCase
  setup do
    @order_box = order_boxes(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:order_boxes)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create order_box" do
    assert_difference('OrderBox.count') do
      post :create, order_box: { nr: @order_box.nr, order_box_type_id: @order_box.order_box_type_id, part_id: @order_box.part_id, quantity: @order_box.quantity, rfid_nr: @order_box.rfid_nr, source_warehouse_id: @order_box.source_warehouse_id, status: @order_box.status, warehouse_id: @order_box.warehouse_id }
    end

    assert_redirected_to order_box_path(assigns(:order_box))
  end

  test "should show order_box" do
    get :show, id: @order_box
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @order_box
    assert_response :success
  end

  test "should update order_box" do
    patch :update, id: @order_box, order_box: { nr: @order_box.nr, order_box_type_id: @order_box.order_box_type_id, part_id: @order_box.part_id, quantity: @order_box.quantity, rfid_nr: @order_box.rfid_nr, source_warehouse_id: @order_box.source_warehouse_id, status: @order_box.status, warehouse_id: @order_box.warehouse_id }
    assert_redirected_to order_box_path(assigns(:order_box))
  end

  test "should destroy order_box" do
    assert_difference('OrderBox.count', -1) do
      delete :destroy, id: @order_box
    end

    assert_redirected_to order_boxes_path
  end
end
