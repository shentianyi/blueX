class PickOrdersController < ApplicationController
  before_action :set_pick_order, only: [:show, :edit, :update, :destroy]

  # GET /pick_orders
  # GET /pick_orders.json
  def index
    @pick_orders = PickOrder.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /pick_orders/1
  # GET /pick_orders/1.json
  def show
  end

  # GET /pick_orders/new
  def new
    @pick_order = PickOrder.new
  end

  # GET /pick_orders/1/edit
  def edit
  end

  # POST /pick_orders
  # POST /pick_orders.json
  def create
    @pick_order = PickOrder.new(pick_order_params)

    respond_to do |format|
      if @pick_order.save
        format.html { redirect_to @pick_order, notice: 'Pick order was successfully created.' }
        format.json { render :show, status: :created, location: @pick_order }
      else
        format.html { render :new }
        format.json { render json: @pick_order.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /pick_orders/1
  # PATCH/PUT /pick_orders/1.json
  def update
    respond_to do |format|
      if @pick_order.update(pick_order_params)
        format.html { redirect_to @pick_order, notice: 'Pick order was successfully updated.' }
        format.json { render :show, status: :ok, location: @pick_order }
      else
        format.html { render :edit }
        format.json { render json: @pick_order.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /pick_orders/1
  # DELETE /pick_orders/1.json
  def destroy
    @pick_order.destroy
    respond_to do |format|
      format.html { redirect_to pick_orders_url, notice: 'Pick order was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_pick_order
      @pick_order = PickOrder.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def pick_order_params
      params.require(:pick_order).permit(:pick_id, :order_id)
    end
end
