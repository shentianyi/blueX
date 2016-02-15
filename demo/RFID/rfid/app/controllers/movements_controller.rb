class MovementsController < ApplicationController
  before_action :set_movement, only: [:show, :edit, :update, :destroy]

  # GET /movements
  # GET /movements.json
  def index
    @movements = Movement.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /movements/1
  # GET /movements/1.json
  def show
  end

  # GET /movements/new
  def new
    @movement = Movement.new
  end

  # GET /movements/1/edit
  def edit
  end

  # POST /movements
  # POST /movements.json
  def create
    @movement = Movement.new(movement_params)

    respond_to do |format|
      if @movement.save
        format.html { redirect_to @movement, notice: 'Movement was successfully created.' }
        format.json { render :show, status: :created, location: @movement }
      else
        format.html { render :new }
        format.json { render json: @movement.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /movements/1
  # PATCH/PUT /movements/1.json
  def update
    respond_to do |format|
      if @movement.update(movement_params)
        format.html { redirect_to @movement, notice: 'Movement was successfully updated.' }
        format.json { render :show, status: :ok, location: @movement }
      else
        format.html { render :edit }
        format.json { render json: @movement.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /movements/1
  # DELETE /movements/1.json
  def destroy
    @movement.destroy
    respond_to do |format|
      format.html { redirect_to movements_url, notice: 'Movement was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def search
    super { |query|
      unless params[:movement][:from_position_id].blank?
        if from_position = Position.find_by_nr(params[:movement][:from_position_id])
          query = query.unscope(where: :from_position_id).where(from_position_id: from_position.id)
        end
      end

      unless params[:movement][:to_position_id].blank?
        if to_position = Position.find_by_nr(params[:movement][:to_position_id])
          query = query.unscope(where: :to_position_id).where(to_position_id: to_position.id)
        end
      end

      unless params[:movement][:part_id].blank?
        if part = Part.find_by_nr(params[:movement][:part_id])
          query = query.unscope(where: :part_id).where(part_id: part.id)
        end
      end

      unless params[:movement][:user_id].blank?
        if user = User.find_by_nr(params[:movement][:user_id])
          query = query.unscope(where: :user_id).where(user_id: user.id)
        end
      end

      query
    }
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_movement
      @movement = Movement.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def movement_params
      params.require(:movement).permit(:part_id, :fifo, :quantity, :package_nr, :uniq_nr, :from_position_id, :from_warehouse_id, :to_position_id, :to_warehouse_id, :move_type_id, :user_id, :remarks)
    end
end
