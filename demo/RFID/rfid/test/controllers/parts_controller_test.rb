require 'test_helper'

class PartsControllerTest < ActionController::TestCase
  setup do
    @part = parts(:one)
  end

  test "should get index" do
    get :index
    assert_response :success
    assert_not_nil assigns(:parts)
  end

  test "should get new" do
    get :new
    assert_response :success
  end

  test "should create part" do
    assert_difference('Part.count') do
      post :create, part: { color_id: @part.color_id, cross_section: @part.cross_section, custom_nr: @part.custom_nr, description: @part.description, measure_unit_id: @part.measure_unit_id, name: @part.name, nr: @part.nr, part_id: @part.part_id, purchase_unit_id: @part.purchase_unit_id, short_description: @part.short_description, weight: @part.weight }
    end

    assert_redirected_to part_path(assigns(:part))
  end

  test "should show part" do
    get :show, id: @part
    assert_response :success
  end

  test "should get edit" do
    get :edit, id: @part
    assert_response :success
  end

  test "should update part" do
    patch :update, id: @part, part: { color_id: @part.color_id, cross_section: @part.cross_section, custom_nr: @part.custom_nr, description: @part.description, measure_unit_id: @part.measure_unit_id, name: @part.name, nr: @part.nr, part_id: @part.part_id, purchase_unit_id: @part.purchase_unit_id, short_description: @part.short_description, weight: @part.weight }
    assert_redirected_to part_path(assigns(:part))
  end

  test "should destroy part" do
    assert_difference('Part.count', -1) do
      delete :destroy, id: @part
    end

    assert_redirected_to parts_path
  end
end
