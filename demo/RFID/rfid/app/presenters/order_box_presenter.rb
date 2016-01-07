#encoding: utf-8
class OrderBoxPresenter<Presenter
  Delegators=[:id, :nr, :rfid_nr, :status, :part_id, :quantity, :warehouse_id, :source_warehouse_id, :order_box_type_id]
  def_delegators :@order_box, *Delegators

  def initialize(order_box)
    @order_box=order_box
    self.delegators =Delegators
  end


  def as_basic_feedback(messages=nil, result_code=nil)
    if @order_box.nil?
      {
          meta: {
              code: 400,
              error_message:'未找到该料盒'
          }
      }
    else
      {
          meta: {
              code: result_code||200,
              message:'找到啦'
          },
          data: {
              id: @order_box.id,
              nr: @order_box.nr,
              rfid_nr: @order_box.rfid_nr,
              status: @order_box.status,

              order_box_type: OrderBoxTypePresenter.new(OrderBoxType.find_by_id(@order_box.order_box_type_id)).as_basic_info,

              #要货仓库
              warehouse: WarehousePresenter.new(Warehouse.find_by_id(@order_box.warehouse_id)).as_basic_info,
              position:PositionPresenter.new(@order_box.position).as_basic_info,
              #出货仓库
              source_warehouse: WarehousePresenter.new(Warehouse.find_by_id(@order_box.source_warehouse_id)).as_basic_info,

              part: PartPresenter.new(Part.find_by_id(@order_box.part_id)).as_basic_info,
              quantity: @order_box.quantity,
              stock: StorageService.stock(@order_box.source_warehouse_id, @order_box.part_id),
              positions: StorageService.positions(@order_box.source_warehouse_id, @order_box.part_id).uniq.pluck(:nr)
          }
      }
    end
  end

end