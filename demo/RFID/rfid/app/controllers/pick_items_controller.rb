class PickItemsController < ApplicationController
  before_action :set_pick_item, only: [:show, :edit, :update, :destroy]

  # GET /pick_items
  # GET /pick_items.json
  def index
    @pick_items = PickItem.paginate(:page => params[:page], :per_page => 100)
  end

  # GET /pick_items/1
  # GET /pick_items/1.json
  def show
  end

  # GET /pick_items/new
  def new
    @pick_item = PickItem.new
  end

  # GET /pick_items/1/edit
  def edit
  end

  # POST /pick_items
  # POST /pick_items.json
  def create
    @pick_item = PickItem.new(pick_item_params)

    respond_to do |format|
      if @pick_item.save
        format.html { redirect_to @pick_item, notice: 'Pick item was successfully created.' }
        format.json { render :show, status: :created, location: @pick_item }
      else
        format.html { render :new }
        format.json { render json: @pick_item.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /pick_items/1
  # PATCH/PUT /pick_items/1.json
  def update
    respond_to do |format|
      if @pick_item.update(pick_item_params)
        format.html { redirect_to @pick_item, notice: 'Pick item was successfully updated.' }
        format.json { render :show, status: :ok, location: @pick_item }
      else
        format.html { render :edit }
        format.json { render json: @pick_item.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /pick_items/1
  # DELETE /pick_items/1.json
  def destroy
    @pick_item.destroy
    respond_to do |format|
      format.html { redirect_to pick_items_url, notice: 'Pick item was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def unfinished
    puts '--------------------------------------------------------------------------'
    @part_id = params[:part_id]
    @date_start = params[:date_start].nil? ? 1.day.ago.strftime("%Y-%m-%d 7:00") : params[:date_start]
    @date_end = params[:date_end].nil? ? Time.now.strftime("%Y-%m-%d 7:00") : params[:date_end]

    if @date_end.to_time - @date_start.to_time > 3.days
      raise "查询时间跨度不要大于3天, 该查询过程耗时比较长, 请耐心等待!"
    end

    part=Part.find_by_nr(params[:part_id])

    @pick_items = PickItem.generate_unfinished_data(@date_start, @date_end, part)
    respond_to do |format|
      format.xlsx do
        send_data(entry_with_xlsx(@pick_items),
                  :type => "application/vnd.openxmlformates-officedocument.spreadsheetml.sheet",
                  :filename => "择货完成未移库-#{Time.now.localtime}.xlsx")
      end
      format.html
    end
  end

  def entry_with_xlsx items
    p = Axlsx::Package.new
    wb = p.workbook
    wb.add_worksheet(:name => "Basic Sheet") do |sheet|

      sheet.add_row ['NO.', '料盒号', '源仓库', '目的仓库', '目的库位', '零件号', '需求数量', '重量', '称重数量', '重量合规', '创建时间', '称重时间', '状态', '备注', '择货单编号']
      items.each_with_index { |pick_item, index|
        sheet.add_row [
                          index+1 ,
                          pick_item[:order_box_nr],
                          pick_item[:order_box_from_wh] ,
                          pick_item[:order_box_to_wh] ,
                          pick_item[:order_box_to_po] ,
                          pick_item[:part_nr] ,
                          pick_item[:order_qty] ,
                          pick_item[:weight] ,
                          pick_item[:weight_qty] ,
                          pick_item[:weight_valid] ,
                          pick_item[:created_at] ,
                          pick_item[:weigh_time] ,
                          pick_item[:status] ,
                          pick_item[:remarks] ,
                          pick_item[:pick_nr] ,
                      ], :types => [:string, :string, :string, :string, :string, :string, :string, :string, :string, :string, :string, :string, :string,:string]
      }
    end
    p.to_stream.read
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_pick_item
      @pick_item = PickItem.find(params[:id])
    end

    # Never trust parameters from the scary internet, only allow the white list through.
    def pick_item_params
      params.require(:pick_item).permit(:pick_id, :status, :warehouse_id, :position_id, :quantity, :part_id, :is_emergency, :order_item_id, :remarks)
    end
end
