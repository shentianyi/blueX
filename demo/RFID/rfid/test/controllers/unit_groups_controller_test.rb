require 'test_helper'

class UnitGroupsControllerTest < ActionController::TestCase
  setup do
    @unit_group = unit_groups(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:unit_groups)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create unit_group" do
    assert_difference('UnitGroup.count') do
      post :create, unit_group: { description: @unit_group.description, name: @unit_group.name, nr: @unit_group.nr }
    end

    assert_redirected_to unit_group_path(assigns(:unit_group))
  end

  test "should show unit_group" do
    get :show, id: @unit_group
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @unit_group
    assert_response :success
  end

  test "should update unit_group" do
    patch :update, id: @unit_group, unit_group: { description: @unit_group.description, name: @unit_group.name, nr: @unit_group.nr }
    assert_redirected_to unit_group_path(assigns(:unit_group))
  end

  test "should destroy unit_group" do
    assert_difference('UnitGroup.count', -1) do
      delete :destroy, id: @unit_group
    end

    assert_redirected_to unit_groups_path
  end
end
