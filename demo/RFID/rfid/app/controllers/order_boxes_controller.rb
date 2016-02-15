class OrderBoxesController < ApplicationController
  before_action :set_order_box, only: [:show, :edit, :update, :destroy]

  # GET /order_boxes
  # GET /order_boxes.json
  def index
    @order_boxes = OrderBox.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /order_boxes/1
  # GET /order_boxes/1.json
  def show
  end

  # GET /order_boxes/new
  def new
    @order_box = OrderBox.new
  end

  # GET /order_boxes/1/edit
  def edit
  end

  # POST /order_boxes
  # POST /order_boxes.json
  def create
    @order_box = OrderBox.new(order_box_params)

    respond_to do |format|
      if @order_box.save
        format.html { redirect_to @order_box, notice: 'Order box was successfully created.' }
        format.json { render :show, status: :created, location: @order_box }
      else
        format.html { render :new }
        format.json { render json: @order_box.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /order_boxes/1
  # PATCH/PUT /order_boxes/1.json
  def update
    respond_to do |format|
      if @order_box.update(order_box_params)
        format.html { redirect_to @order_box, notice: 'Order box was successfully updated.' }
        format.json { render :show, status: :ok, location: @order_box }
      else
        format.html { render :edit }
        format.json { render json: @order_box.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /order_boxes/1
  # DELETE /order_boxes/1.json
  def destroy
    @order_box.destroy
    respond_to do |format|
      format.html { redirect_to order_boxes_url, notice: 'Order box was successfully destroyed.' }
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
        msg = FileHandler::Excel::OrderBoxHandler.import(fd)
      rescue => e
        msg.content = e.message
      end
      render json: msg
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_order_box
      @order_box = OrderBox.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def order_box_params
      params.require(:order_box).permit(:nr, :rfid_nr, :status, :part_id, :quantity, :warehouse_id, :source_warehouse_id, :order_box_type_id)
    end
end
