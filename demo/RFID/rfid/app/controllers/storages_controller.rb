class StoragesController < ApplicationController
  before_action :set_storage, only: [:show, :edit, :update, :destroy]

  # GET /storages
  # GET /storages.json
  def index
    @storages = Storage.order(created_at: :desc).paginate(:page => params[:page], :per_page => 100)
  end

  # GET /storages/1
  # GET /storages/1.json
  def show
  end

  # GET /storages/new
  def new
    @storage = Storage.new
  end

  # GET /storages/1/edit
  def edit
  end

  # POST /storages
  # POST /storages.json
  def create
    @storage = Storage.new(storage_params)

    respond_to do |format|
      if @storage.save
        format.html { redirect_to @storage, notice: 'Storage was successfully created.' }
        format.json { render :show, status: :created, location: @storage }
      else
        format.html { render :new }
        format.json { render json: @storage.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /storages/1
  # PATCH/PUT /storages/1.json
  def update
    respond_to do |format|
      if @storage.update(storage_params)
        format.html { redirect_to @storage, notice: 'Storage was successfully updated.' }
        format.json { render :show, status: :ok, location: @storage }
      else
        format.html { render :edit }
        format.json { render json: @storage.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /storages/1
  # DELETE /storages/1.json
  def destroy
    @storage.destroy
    respond_to do |format|
      format.html { redirect_to storages_url, notice: 'Storage was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def search
    super { |query|
      unless params[:storage][:position_id].blank?
        if position = Position.find_by_nr(params[:storage][:position_id])
          query = query.unscope(where: :position_id).where(position_id: position.id)
        end
      end

      unless params[:storage][:part_id].blank?
        if part_id = Part.find_by_nr(params[:storage][:part_id])
          query = query.unscope(where: :part_id).where(part_id: part_id)
        end
      end

      query
    }
  end

  def import
    if request.post?
      msg = Message.new
      begin
        file=params[:files][0]
        fd = FileData.new(data: file, original_name: file.original_filename, path: $upload_data_file_path, path_name: "#{Time.now.strftime('%Y%m%d%H%M%S%L')}~#{file.original_filename}")
        fd.save
        msg = FileHandler::Excel::StorageHandler.import(fd, current_user)
      rescue => e
        msg.content = e.message
      end
      render json: msg
    end
  end

  private
  # Use callbacks to share common setup or constraints between actions.
  def set_storage
    @storage = Storage.find(params[:id])
  end

  # Never trust parameters from the scary internet, only allow the white list through.
  def storage_params
    params.require(:storage).permit(:part_id, :fifo, :quantity, :package_nr, :uniq_nr, :position_id, :warehouse_id, :remarks, :user_id)
  end
end
