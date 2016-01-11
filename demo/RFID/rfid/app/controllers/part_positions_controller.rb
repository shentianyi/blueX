class PartPositionsController < ApplicationController
  before_action :set_part_position, only: [:show, :edit, :update, :destroy]

  # GET /part_positions
  # GET /part_positions.json
  def index
    @part_positions = PartPosition.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /part_positions/1
  # GET /part_positions/1.json
  def show
  end

  # GET /part_positions/new
  def new
    @part_position = PartPosition.new
  end

  # GET /part_positions/1/edit
  def edit
  end

  # POST /part_positions
  # POST /part_positions.json
  def create
    @part_position = PartPosition.new(part_position_params)

    respond_to do |format|
      if @part_position.save
        format.html { redirect_to @part_position, notice: 'Part position was successfully created.' }
        format.json { render :show, status: :created, location: @part_position }
      else
        format.html { render :new }
        format.json { render json: @part_position.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /part_positions/1
  # PATCH/PUT /part_positions/1.json
  def update
    respond_to do |format|
      if @part_position.update(part_position_params)
        format.html { redirect_to @part_position, notice: 'Part position was successfully updated.' }
        format.json { render :show, status: :ok, location: @part_position }
      else
        format.html { render :edit }
        format.json { render json: @part_position.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /part_positions/1
  # DELETE /part_positions/1.json
  def destroy
    @part_position.destroy
    respond_to do |format|
      format.html { redirect_to part_positions_url, notice: 'Part position was successfully destroyed.' }
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
        msg = FileHandler::Excel::PartPositionHandler.import(fd)
      rescue => e
        msg.content = e.message
      end
      render json: msg
    end
  end

  def search
    super { |query|
      unless params[:part_position][:part_id].blank?
        if part = Part.find_by_nr(params[:part_position][:part_id])
          query = query.unscope(where: :part_id).where(part_id: part.id)
        end
      end

      unless params[:part_position][:position_id].blank?
        if part = Position.find_by_nr(params[:part_position][:position_id])
          query = query.unscope(where: :position_id).where(position_id: part.id)
        end
      end

      unless params[:part_position][:from_position_id].blank?
        if part = Position.find_by_nr(params[:part_position][:from_position_id])
          query = query.unscope(where: :from_position_id).where(from_position_id: part.id)
        end
      end

      unless params[:part_position][:from_warehouse_id].blank?
        if part = Warehouse.find_by_nr(params[:part_position][:from_warehouse_id])
          query = query.unscope(where: :from_warehouse_id).where(from_warehouse_id: part.id)
        end
      end

      query
    }
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_part_position
      @part_position = PartPosition.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def part_position_params
      params.require(:part_position).permit(:part_id, :position_id, :safe_stock, :from_warehouse_id, :from_position_id)
    end
end
