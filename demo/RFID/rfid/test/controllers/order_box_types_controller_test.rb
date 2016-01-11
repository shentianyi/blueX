require 'test_helper'

class OrderBoxTypesControllerTest < ActionController::TestCase
  setup do
    @order_box_type = order_box_types(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:order_box_types)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create order_box_type" do
    assert_difference('OrderBoxType.count') do
      post :create, order_box_type: { description: @order_box_type.description, name: @order_box_type.name }
    end

    assert_redirected_to order_box_type_path(assigns(:order_box_type))
  end

  test "should show order_box_type" do
    get :show, id: @order_box_type
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @order_box_type
    assert_response :success
  end

  test "should update order_box_type" do
    patch :update, id: @order_box_type, order_box_type: { description: @order_box_type.description, name: @order_box_type.name }
    assert_redirected_to order_box_type_path(assigns(:order_box_type))
  end

  test "should destroy order_box_type" do
    assert_difference('OrderBoxType.count', -1) do
      delete :destroy, id: @order_box_type
    end

    assert_redirected_to order_box_types_path
  end
end
