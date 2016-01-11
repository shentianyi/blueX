module FileHandler
  module Excel
    class StorageHandler<Base

      IMPORT_HEADERS=[
          :part_id, :fifo, :quantity, :package_nr, :position_id, :warehouse_id, :remarks, :user_id
      ]

      MOVE_HEADERS = [
          :part_id, :fifo, :quantity, :package_nr, :uniq, :from_position_id, :from_warehouse_id, :to_position_id, :to_warehouse_id, :move_type_id, :user_id, :remarks
      ]

      def self.import(file, user)
        msg = Message.new
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        validate_msg = validate_import(file)
        if validate_msg.result
          begin
            Storage.transaction do
              2.upto(book.last_row) do |line|
                row = {}
                IMPORT_HEADERS.each_with_index do |k, i|
                  row[k] = book.cell(line, i+1).to_s.strip
                  if k== :part_id || k== :package_nr || k==:user_id
                    row[k] = row[k].sub(/\.0/, '')
                  end
                end

                row[:part_id]=Part.find_by_nr(row[:part_id]).id unless row[:part_id].blank?
                row[:position_id]=Position.find_by_nr(row[:position_id]).id unless row[:position_id].blank?
                row[:warehouse_id]=Warehouse.find_by_nr(row[:warehouse_id]).id unless row[:warehouse_id].blank?

                WarehouseService.new.enter_stock(row, user)
              end
            end
            msg.result = true
            msg.content = "导入库存数据成功"
          rescue => e
            puts e.backtrace
            msg.result = false
            msg.content = e.message
          end
        else
          msg.result = false
          msg.content = validate_msg.content
        end

        msg
      end

      #
      # def self.move(file, current_user)
      #   msg = Message.new
      #   book = Roo::Excelx.new file.full_path
      #   book.default_sheet = book.sheets.first
      #   validate_msg = validate_move(file)
      #   if validate_msg.result
      #     begin
      #       Storage.transaction do
      #         builder = current_user.blank? ? '' : current_user.id
      #         move_list = MovementList.create(builder: builder, name: "#{builder}_#{DateTime.now.strftime("%H.%d.%m.%Y")}_File")
      #         2.upto(book.last_row) do |line|
      #           row = {}
      #           MOVE_HEADERS.each_with_index do |k, i|
      #             row[k] = book.cell(line, i+1).to_s.strip
      #             if k== :partNr || k== :packageId
      #               row[k] = row[k].sub(/\.0/, '')
      #             end
      #           end
      #
      #           row[:movement_list_id] = move_list.id
      #           MovementSource.create(row)
      #
      #           row[:user] = current_user
      #           WhouseService.new.move(row)
      #           move_list.update(state: MovementListState::ENDING)
      #         end
      #       end
      #       msg.result = true
      #       msg.content = "导入移库数据成功"
      #     rescue => e
      #       puts e.backtrace
      #       msg.result = false
      #       msg.content = e.message
      #     end
      #   else
      #     msg.result = false
      #     msg.content = validate_msg.content
      #   end
      #
      #   msg
      # end

      def self.validate_import file
        tmp_file=full_tmp_path(file.original_name)
        msg = Message.new(result: true)
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        p = Axlsx::Package.new
        p.workbook.add_worksheet(:name => "Basic Worksheet") do |sheet|
          sheet.add_row IMPORT_HEADERS+['Error Msg']
          #validate file
          2.upto(book.last_row) do |line|
            row = {}
            IMPORT_HEADERS.each_with_index do |k, i|
              row[k] = book.cell(line, i+1).to_s.strip
              row[k]=row[k].sub(/\.0/, '') if k== :partNr || k== :packageId
            end

            mssg = validate_import_row(row)
            if mssg.result
              sheet.add_row row.values
            else
              if msg.result
                msg.result = false
                msg.content = "下载错误文件<a href='/files/#{Base64.urlsafe_encode64(tmp_file)}'>#{::File.basename(tmp_file)}</a>"
              end
              sheet.add_row row.values<<mssg.content
            end
          end
        end
        p.use_shared_strings = true
        p.serialize(tmp_file)
        msg
      end

      def self.validate_import_row(row)
        msg = Message.new(contents: [])

        if row[:warehouse_id].present?
          unless Warehouse.find_by_nr(row[:warehouse_id])
            msg.contents << "目的仓库号:#{row[:warehouse_id]} 不存在!"
          end
        end

        if row[:position_id].present?
          unless Position.find_by_nr(row[:position_id])
            msg.contents << "目的库位号:#{row[:position_id]} 不存在!"
          end
        end

        if row[:part_id].present?
          unless Part.find_by_nr(row[:part_id])
            msg.contents << "零件号:#{row[:part_id]} 不存在!"
          end
        end

        if row[:user_id].present?
          unless User.find_by_nr(row[:user_id])
            msg.contents << "员工号:#{row[:user_id]} 不存在!"
          end
        end

        unless row[:quantity].to_f > 0
          msg.contents << "数量: #{row[:quantity]} 不可以小于等于 0!"
        end

        if row[:fifo].present?
          begin
            row[:fifo].to_time
          rescue => e
            msg.contents << "FIFO: #{row[:fifo]} 错误!"
          end
        end

        unless msg.result=(msg.contents.size==0)
          msg.content=msg.contents.join('/')
        end
        msg
      end

      # def self.validate_move file
      #   tmp_file=full_tmp_path(file.original_name)
      #   msg = Message.new(result: true)
      #   book = Roo::Excelx.new file.full_path
      #   book.default_sheet = book.sheets.first
      #
      #   p = Axlsx::Package.new
      #   p.workbook.add_worksheet(:name => "Basic Worksheet") do |sheet|
      #     sheet.add_row MOVE_HEADERS+['Error Msg']
      #     #validate file
      #     2.upto(book.last_row) do |line|
      #       row = {}
      #       MOVE_HEADERS.each_with_index do |k, i|
      #         row[k] = book.cell(line, i+1).to_s.strip
      #         row[k]=row[k].sub(/\.0/, '') if k== :partNr || k== :packageId
      #       end
      #
      #       mssg = validate_move_row(row)
      #       if mssg.result
      #         sheet.add_row row.values
      #       else
      #         if msg.result
      #           msg.result = false
      #           msg.content = "下载错误文件<a href='/files/#{Base64.urlsafe_encode64(tmp_file)}'>#{::File.basename(tmp_file)}</a>"
      #         end
      #         sheet.add_row row.values<<mssg.content
      #       end
      #     end
      #   end
      #   p.use_shared_strings = true
      #   p.serialize(tmp_file)
      #   msg
      # end

      # def self.validate_move_row(row)
      #   msg = Message.new(contents: [])
      #   StorageOperationRecord.save_record(row, 'MOVE')
      #
      #   if row[:packageId].present?
      #     unless package = Storage.exists_package?(row[:packageId])
      #       msg.contents << "唯一码:#{row['packageId']} 不存在!"
      #     end
      #
      #     if package && package.qty < row[:qty].to_f
      #       msg.contents << "移库量大于剩余库存量,唯一码#{row['packageId']}!"
      #     end
      #
      #     if row[:fromWh].present?
      #       storage = Storage.find_by(packageId: row[:packageId], ware_house_id: row[:fromWh])
      #       unless storage
      #         msg.contents << "源仓库#{row[:fromWh]}不存在该唯一码#{row[:packageId]}！"
      #       end
      #     end
      #
      #     msg.contents << "数量: #{row[:qty]} 不可以小于等于 0!" if row[:qty].to_f < 0
      #
      #   else
      #
      #     if row[:partNr].blank?
      #       msg.contents << "零件号不能为空!"
      #     end
      #
      #     if row[:qty].blank? || row[:qty].to_f <= 0
      #       msg.contents << "数量: #{row[:qty]} 不可以小于等于 0!"
      #     end
      #
      #   end
      #
      #   if row[:toWh].blank?
      #     msg.contents << "目的仓库号不能为空!"
      #   end
      #
      #   if row[:toPosition].blank?
      #     msg.contents << "目的库位号不能为空!"
      #   end
      #
      #   if row[:fromWh].present?
      #     src_warehouse = Whouse.find_by_id(row[:fromWh])
      #     unless src_warehouse
      #       msg.contents << "源仓库号:#{row[:fromWh]} 不存在!"
      #     end
      #   end
      #
      #   if row[:toWh].present?
      #     dse_warehouse = Whouse.find_by_id(row[:toWh])
      #     unless dse_warehouse
      #       msg.contents << "目的仓库号:#{row[:toWh]} 不存在!"
      #     end
      #   end
      #
      #   positions = []
      #   if row[:packageId].present? && row[:partNr].blank?
      #   else
      #     part_id = Part.find_by_id(row[:partNr])
      #     if part_id
      #       part_id.positions.each do |position|
      #         positions += ["#{position.detail}"]
      #       end
      #     else
      #       msg.contents << "零件号:#{row[:partNr]} 不存在!"
      #     end
      #   end
      #
      #   if row[:fifo].present?
      #     begin
      #       row[:fifo].to_time
      #     rescue => e
      #       msg.contents << "FIFO: #{row[:fifo]} 错误!"
      #     end
      #   end
      #
      #   if row[:fromPosition].present?
      #     from_position = Position.find_by(detail: row[:fromPosition])
      #     unless from_position
      #       msg.contents << "源位置:#{row[:fromPosition]} 不存在!"
      #     end
      #   end
      #
      #   if from_position && part_id
      #     unless positions.include?(row[:fromPosition])
      #       msg.contents << "零件号:#{row[:partNr]} 不在源库位号:#{row[:fromPosition]}上!"
      #     end
      #   end
      #
      #   if row[:toPosition].present?
      #     to_position = Position.find_by(detail: row[:toPosition])
      #     unless to_position
      #       msg.contents << "目的库位号:#{row[:toPosition]} 不存在!"
      #     end
      #   end
      #
      #   if to_position && part_id
      #     unless positions.include?(row[:toPosition])
      #       msg.contents << "零件号:#{row[:partNr]}不在目的库位号:#{row[:toPosition]}上!"
      #     end
      #   end
      #
      #   if row[:employee_id].present?
      #     employee_id = User.find(row[:employee_id])
      #     unless employee_id
      #       msg.contents << "员工号:#{row[:employee_id].sub(/\.0/, '')} 不存在!"
      #     end
      #   end
      #
      #   unless msg.result=(msg.contents.size==0)
      #     msg.content=msg.contents.join('/')
      #   end
      #   msg
      # end


    end
  end
end