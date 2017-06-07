class PicksController < ApplicationController
  before_action :set_pick, only: [:show, :edit, :update, :destroy, :pick_end_items, :pick_items, :exports]

  # GET /picks
  # GET /picks.json
  def index
    @picks = Pick.order(created_at: :desc).paginate(:page => params[:page], :per_page => 100)
  end

  # GET /picks/1
  # GET /picks/1.json
  def show
  end

  # GET /picks/new
  def new
    @pick = Pick.new
  end

  # GET /picks/1/edit
  def edit
  end

  # POST /picks
  # POST /picks.json
  def create
    @pick = Pick.new(pick_params)

    respond_to do |format|
      if @pick.save
        format.html { redirect_to @pick, notice: 'Pick was successfully created.' }
        format.json { render :show, status: :created, location: @pick }
      else
        format.html { render :new }
        format.json { render json: @pick.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /picks/1
  # PATCH/PUT /picks/1.json
  def update
    respond_to do |format|
      if @pick.update(pick_params)
        format.html { redirect_to @pick, notice: 'Pick was successfully updated.' }
        format.json { render :show, status: :ok, location: @pick }
      else
        format.html { render :edit }
        format.json { render json: @pick.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /picks/1
  # DELETE /picks/1.json
  def destroy
    @pick.destroy
    respond_to do |format|
      format.html { redirect_to picks_url, notice: 'Pick was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def pick_items
    @pick_items = @pick.pick_items.paginate(:page => params[:page])
    @page_start=(params[:page].nil? ? 0 : (params[:page].to_i-1))*100
  end

  def pick_end_items
    @pick_items = @pick.pick_items.where(status: PickItemStatus::PICKED).paginate(:page => params[:page])
    @page_start=(params[:page].nil? ? 0 : (params[:page].to_i-1))*100
    render :pick_items
  end

  def exports
    pick_items = @pick.pick_items

    send_data(entry_with_xlsx(pick_items),
              :type => "application/vnd.openxmlformates-officedocument.spreadsheetml.sheet",
              :filename => "择货单_#{@pick.nr}_择货项.xlsx")
  end

  def entry_with_xlsx pick_items
    p = Axlsx::Package.new
    wb = p.workbook
    wb.add_worksheet(:name => "Basic Sheet") do |sheet|
      sheet.add_row entry_header
      pick_items.each_with_index { |i, index|
        sheet.add_row [
                          index+1,
                          i.pick.nr,
                          PickItemStatus.display(i.status),
                          i.warehouse.blank? ? '' : i.warehouse.nr,
                          i.position.blank? ? '' : i.position.nr,
                          i.quantity,
                          i.part.blank? ? '' : i.part.nr,
                          i.is_emergency ? '是' : '否',
                          i.order_item.blank? ? '' : i.order_item.order.nr,
                          i.remarks
                      ], :types => [:string, :string, :string, :string, :string, :string, :string, :string, :string]
      }
    end
    p.to_stream.read
  end

  def entry_header
    ["编号", "择货单号", "状态", "仓库号", "库位号", "数量", "零件号", "是否加急", "需求单号", "备注"]
  end

  def search
    super { |query|
      unless params[:pick][:warehouse_id].blank?
        if warehouse = Position.find_by_nr(params[:pick][:warehouse_id])
          query = query.unscope(where: :warehouse_id).where(warehouse_id: warehouse.id)
        end
      end

      unless params[:pick][:user_id].blank?
        if user = User.find_by_nr(params[:pick][:user_id])
          query = query.unscope(where: :user_id).where(user_id: user.id)
        end
      end

      query = query.order(created_at: :desc)

      query
    }
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_pick
      @pick = Pick.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def pick_params
      params.require(:pick).permit(:user_id, :status, :warehouse_id, :remarks, :nr)
    end
end
