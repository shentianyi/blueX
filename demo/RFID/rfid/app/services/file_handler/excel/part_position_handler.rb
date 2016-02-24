module FileHandler
  module Excel
    class PartPositionHandler<Base
      HEADERS=[
          :part_id, :old_part_id, :position_id, :old_position_id, :safe_stock, :from_warehouse_id, :from_position_id, :operator
      ]

      def self.import(file)
        msg = Message.new
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        validate_msg = validate_import(file)
        if validate_msg.result
          #validate file
          begin
            PartPosition.transaction do
              2.upto(book.last_row) do |line|
                row = {}
                HEADERS.each_with_index do |k, i|
                  row[k] = book.cell(line, i+1).to_s.strip
                  if k.to_s=='part_id'
                    row[k]=row[k].sub(/\.0/, '')
                  end
                end
                row[:part_id] = Part.find_by_nr(row[:part_id]).id unless row[:part_id].blank?
                row[:position_id] = Position.find_by_nr(row[:position_id]).id unless row[:position_id].blank?
                row[:old_part_id] = Part.find_by_nr(row[:old_part_id]).id unless row[:old_part_id].blank?
                row[:old_position_id] = Position.find_by_nr(row[:old_position_id]).id unless row[:old_position_id].blank?
                row[:from_warehouse_id] = Warehouse.find_by_nr(row[:from_warehouse_id]).id unless row[:from_warehouse_id].blank?
                row[:from_position_id] = Position.find_by_nr(row[:from_position_id]).id unless row[:from_position_id].blank?

                if (!row[:old_part_id].blank? && !row[:old_position_id].blank?) && pp=PartPosition.where(part_id: row[:old_part_id], position_id: row[:old_position_id]).first
                  if row[:operator].blank? || row[:operator]=='update'
                    pp.update(row.except(:nr, :operator, :old_part_id, :old_position_id))
                  elsif row[:operator]=='delete'
                    pp.destroy
                  end
                else
                  if row[:operator].blank? || row[:operator]=='create'
                    s =PartPosition.new(row.except(:operator, :old_part_id, :old_position_id))
                    unless s.save
                      puts s.errors.to_json
                      raise s.errors.to_json
                    end
                  end
                end

              end
            end
            msg.result = true
            msg.content = "导入零件位置信息成功！"
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

      def self.validate_import file
        tmp_file=full_tmp_path(file.original_name)
        msg = Message.new(result: true)
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        p = Axlsx::Package.new
        p.workbook.add_worksheet(:name => "Basic Worksheet") do |sheet|
          sheet.add_row HEADERS+['Error Msg']
          #validate file
          2.upto(book.last_row) do |line|
            row = {}
            HEADERS.each_with_index do |k, i|
              row[k] = book.cell(line, i+1).to_s.strip
              if k.to_s=='part_id'
                row[k]=row[k].sub(/\.0/, '')
              end
            end

            mssg = validate_row(row, line)
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

      def self.validate_row(row, line)
        msg = Message.new(contents: [])

        unless row[:part_id].blank?
          if Part.find_by_nr(row[:part_id]).blank?
            msg.contents<<"零件号:#{row[:part_id]}不存在"
          end
        end

        unless row[:position_id].blank?
          if Position.find_by_nr(row[:position_id]).blank?
            msg.contents<<"库位号:#{row[:position_id]}不存在"
          end
        end

        unless row[:from_warehouse_id].blank?
          if Warehouse.find_by_nr(row[:from_warehouse_id]).blank?
            msg.contents<<"默认源仓库:#{row[:from_warehouse_id]}不存在"
          end
        end

        unless row[:from_position_id].blank?
          if Position.find_by_nr(row[:from_position_id]).blank?
            msg.contents<<"默认源库位:#{row[:from_position_id]}不存在"
          end
        end

        unless msg.result=(msg.contents.size==0)
          msg.content=msg.contents.join('/')
        end
        msg
      end
    end
  end
end
