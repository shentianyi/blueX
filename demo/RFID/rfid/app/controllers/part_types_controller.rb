class PartTypesController < ApplicationController
  before_action :set_part_type, only: [:show, :edit, :update, :destroy]

  # GET /part_types
  # GET /part_types.json
  def index
    @part_types = PartType.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /part_types/1
  # GET /part_types/1.json
  def show
  end

  # GET /part_types/new
  def new
    @part_type = PartType.new
  end

  # GET /part_types/1/edit
  def edit
  end

  # POST /part_types
  # POST /part_types.json
  def create
    @part_type = PartType.new(part_type_params)

    respond_to do |format|
      if @part_type.save
        format.html { redirect_to @part_type, notice: 'Part type was successfully created.' }
        format.json { render :show, status: :created, location: @part_type }
      else
        format.html { render :new }
        format.json { render json: @part_type.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /part_types/1
  # PATCH/PUT /part_types/1.json
  def update
    respond_to do |format|
      if @part_type.update(part_type_params)
        format.html { redirect_to @part_type, notice: 'Part type was successfully updated.' }
        format.json { render :show, status: :ok, location: @part_type }
      else
        format.html { render :edit }
        format.json { render json: @part_type.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /part_types/1
  # DELETE /part_types/1.json
  def destroy
    @part_type.destroy
    respond_to do |format|
      format.html { redirect_to part_types_url, notice: 'Part type was successfully destroyed.' }
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
        msg = FileHandler::Excel::PartTypeHandler.import(fd)
      rescue => e
        msg.content = e.message
      end
      render json: msg
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_part_type
      @part_type = PartType.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def part_type_params
      params.require(:part_type).permit(:nr, :name, :description)
    end
end
