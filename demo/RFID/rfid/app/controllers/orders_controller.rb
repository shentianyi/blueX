class OrdersController < ApplicationController
  before_action :set_order, only: [:show, :edit, :update, :destroy, :order_items, :exports]

  # GET /orders
  # GET /orders.json
  def index
    @orders = Order.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /orders/1
  # GET /orders/1.json
  def show
  end

  # GET /orders/new
  def new
    @order = Order.new
  end

  # GET /orders/1/edit
  def edit
  end

  # POST /orders
  # POST /orders.json
  def create
    @order = Order.new(order_params)

    respond_to do |format|
      if @order.save
        format.html { redirect_to @order, notice: 'Order was successfully created.' }
        format.json { render :show, status: :created, location: @order }
      else
        format.html { render :new }
        format.json { render json: @order.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /orders/1
  # PATCH/PUT /orders/1.json
  def update
    respond_to do |format|
      if @order.update(order_params)
        format.html { redirect_to @order, notice: 'Order was successfully updated.' }
        format.json { render :show, status: :ok, location: @order }
      else
        format.html { render :edit }
        format.json { render json: @order.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /orders/1
  # DELETE /orders/1.json
  def destroy
    @order.destroy
    respond_to do |format|
      format.html { redirect_to orders_url, notice: 'Order was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def order_items
    @order_items = @order.order_items.paginate(:page => params[:page])
    @page_start=(params[:page].nil? ? 0 : (params[:page].to_i-1))*100
  end

  def exports
    send_data(entry_with_xlsx(@order.order_items),
              :type => "application/vnd.openxmlformates-officedocument.spreadsheetml.sheet",
              :filename => "需求单_#{@order.nr}_需求项.xlsx")
  end

  def entry_with_xlsx order_items
    p = Axlsx::Package.new
    wb = p.workbook
    wb.add_worksheet(:name => "Basic Sheet") do |sheet|
      sheet.add_row entry_header
      order_items.each_with_index { |i, index|
        sheet.add_row [
                          index+1,
                          "#{i.order.nr}",
                          OrderItemStatus.display(i.status),
                          i.user.blank? ? '' : i.user.nr,
                          i.quantity,
                          i.part.blank? ? '' : i.part.nr,
                          i.is_emergency ? '是' : '否',
                          i.orderable.blank? ? '' : "#{i.orderable.nr}",
                          i.remarks
                      ], :types => [:string, :string, :string, :string, :string, :string, :string, :string]
      }
    end
    p.to_stream.read
  end

  def entry_header
    ["编号", "择货单号", "状态", "创建者", "数量", "零件号", "是否加急", "料盒编号", "备注"]
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_order
      @order = Order.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def order_params
      params.require(:order).permit(:user_id, :status, :orderable_id, :orderable_type, :remarks, :warehouse_id, :nr)
    end
end
