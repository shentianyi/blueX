require 'test_helper'

class OrderCarsControllerTest < ActionController::TestCase
  setup do
    @order_car = order_cars(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:order_cars)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create order_car" do
    assert_difference('OrderCar.count') do
      post :create, order_car: { nr: @order_car.nr, rfid_nr: @order_car.rfid_nr, status: @order_car.status, warehouse_id: @order_car.warehouse_id }
    end

    assert_redirected_to order_car_path(assigns(:order_car))
  end

  test "should show order_car" do
    get :show, id: @order_car
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @order_car
    assert_response :success
  end

  test "should update order_car" do
    patch :update, id: @order_car, order_car: { nr: @order_car.nr, rfid_nr: @order_car.rfid_nr, status: @order_car.status, warehouse_id: @order_car.warehouse_id }
    assert_redirected_to order_car_path(assigns(:order_car))
  end

  test "should destroy order_car" do
    assert_difference('OrderCar.count', -1) do
      delete :destroy, id: @order_car
    end

    assert_redirected_to order_cars_path
  end
end
