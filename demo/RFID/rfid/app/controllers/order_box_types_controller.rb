class OrderBoxTypesController < ApplicationController
  before_action :set_order_box_type, only: [:show, :edit, :update, :destroy]

  # GET /order_box_types
  # GET /order_box_types.json
  def index
    @order_box_types = OrderBoxType.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /order_box_types/1
  # GET /order_box_types/1.json
  def show
  end

  # GET /order_box_types/new
  def new
    @order_box_type = OrderBoxType.new
  end

  # GET /order_box_types/1/edit
  def edit
  end

  # POST /order_box_types
  # POST /order_box_types.json
  def create
    @order_box_type = OrderBoxType.new(order_box_type_params)

    respond_to do |format|
      if @order_box_type.save
        format.html { redirect_to @order_box_type, notice: 'Order box type was successfully created.' }
        format.json { render :show, status: :created, location: @order_box_type }
      else
        format.html { render :new }
        format.json { render json: @order_box_type.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /order_box_types/1
  # PATCH/PUT /order_box_types/1.json
  def update
    respond_to do |format|
      if @order_box_type.update(order_box_type_params)
        format.html { redirect_to @order_box_type, notice: 'Order box type was successfully updated.' }
        format.json { render :show, status: :ok, location: @order_box_type }
      else
        format.html { render :edit }
        format.json { render json: @order_box_type.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /order_box_types/1
  # DELETE /order_box_types/1.json
  def destroy
    @order_box_type.destroy
    respond_to do |format|
      format.html { redirect_to order_box_types_url, notice: 'Order box type was successfully destroyed.' }
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
        msg = FileHandler::Excel::OrderBoxTypeHandler.import(fd)
      rescue => e
        msg.content = e.message
      end
      render json: msg
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_order_box_type
      @order_box_type = OrderBoxType.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def order_box_type_params
      params.require(:order_box_type).permit(:name, :description, :weight)
    end
end
