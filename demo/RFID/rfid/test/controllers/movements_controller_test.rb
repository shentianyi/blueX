require 'test_helper'

class MovementsControllerTest < ActionController::TestCase
  setup do
    @movement = movements(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:movements)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create movement" do
    assert_difference('Movement.count') do
      post :create, movement: { fifo: @movement.fifo, from_position_id: @movement.from_position_id, from_warehouse_id: @movement.from_warehouse_id, move_type_id: @movement.move_type_id, package_nr: @movement.package_nr, part_id: @movement.part_id, quantity: @movement.quantity, remarks: @movement.remarks, to_position_id: @movement.to_position_id, to_warehouse_id: @movement.to_warehouse_id, uniq: @movement.uniq, user_id: @movement.user_id }
    end

    assert_redirected_to movement_path(assigns(:movement))
  end

  test "should show movement" do
    get :show, id: @movement
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @movement
    assert_response :success
  end

  test "should update movement" do
    patch :update, id: @movement, movement: { fifo: @movement.fifo, from_position_id: @movement.from_position_id, from_warehouse_id: @movement.from_warehouse_id, move_type_id: @movement.move_type_id, package_nr: @movement.package_nr, part_id: @movement.part_id, quantity: @movement.quantity, remarks: @movement.remarks, to_position_id: @movement.to_position_id, to_warehouse_id: @movement.to_warehouse_id, uniq: @movement.uniq, user_id: @movement.user_id }
    assert_redirected_to movement_path(assigns(:movement))
  end

  test "should destroy movement" do
    assert_difference('Movement.count', -1) do
      delete :destroy, id: @movement
    end

    assert_redirected_to movements_path
  end
end
