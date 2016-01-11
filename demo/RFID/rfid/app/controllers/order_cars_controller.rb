class OrderCarsController < ApplicationController
  before_action :set_order_car, only: [:show, :edit, :update, :destroy]

  # GET /order_cars
  # GET /order_cars.json
  def index
    @order_cars = OrderCar.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /order_cars/1
  # GET /order_cars/1.json
  def show
  end

  # GET /order_cars/new
  def new
    @order_car = OrderCar.new
  end

  # GET /order_cars/1/edit
  def edit
  end

  # POST /order_cars
  # POST /order_cars.json
  def create
    @order_car = OrderCar.new(order_car_params)

    respond_to do |format|
      if @order_car.save
        format.html { redirect_to @order_car, notice: 'Order car was successfully created.' }
        format.json { render :show, status: :created, location: @order_car }
      else
        format.html { render :new }
        format.json { render json: @order_car.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /order_cars/1
  # PATCH/PUT /order_cars/1.json
  def update
    respond_to do |format|
      if @order_car.update(order_car_params)
        format.html { redirect_to @order_car, notice: 'Order car was successfully updated.' }
        format.json { render :show, status: :ok, location: @order_car }
      else
        format.html { render :edit }
        format.json { render json: @order_car.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /order_cars/1
  # DELETE /order_cars/1.json
  def destroy
    @order_car.destroy
    respond_to do |format|
      format.html { redirect_to order_cars_url, notice: 'Order car was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def import
    if request.post?
      msg = Message.new
      begin
        file=params[:files][0]
        fd = FileData.new(data: file, original_name: file.original_filename, path: $upload_data_file_path, path_name: "#{Time.now.strftime('%Y%m%d%H%M%S%L')}~#{file.original_filename}")
        fd.save
        msg = FileHandler::Excel::OrderCarHandler.import(fd)
      rescue => e
        msg.content = e.message
      end
      render json: msg
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_order_car
      @order_car = OrderCar.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def order_car_params
      params.require(:order_car).permit(:nr, :rfid_nr, :warehouse_id, :status)
    end
end
