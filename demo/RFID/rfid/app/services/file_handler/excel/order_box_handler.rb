module FileHandler
  module Excel
    class OrderBoxHandler<Base
      HEADERS=[
          :nr, :rfid_nr, :status, :part_id, :quantity, :warehouse_id, :source_warehouse_id, :order_box_type_id
      ]

      def self.import(file)
        msg = Message.new
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        validate_msg = validate_import(file)
        if validate_msg.result
          #validate file
          begin
            OrderBox.transaction do
              2.upto(book.last_row) do |line|
                row = {}
                HEADERS.each_with_index do |k, i|
                  row[k] = book.cell(line, i+1).to_s.strip
                end
                row[:part_id] = Part.find_by_nr(row[:part_id]).id
                row[:warehouse_id] = Warehouse.find_by_nr(row[:warehouse_id]).id unless row[:warehouse_id].blank?
                row[:source_warehouse_id] = Warehouse.find_by_nr(row[:source_warehouse_id]).id unless row[:source_warehouse_id].blank?
                row[:order_box_type_id] = OrderBoxType.find_by_nr(row[:order_box_type_id]).id unless row[:order_box_type_id].blank?

                row.delete(:status) if row[:status].blank?

                s =OrderBox.new(row)
                unless s.save
                  puts s.errors.to_json
                  raise s.errors.to_json
                end
              end
            end
            msg.result = true
            msg.content = "导入料盒信息成功！"
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

        if OrderBox.find_by_nr(row[:nr])
          msg.contents<<"该料盒已存在"
        end

        unless Part.find_by_nr(row[:part_id])
          msg.contents<<"该零件不存在"
        end

        unless row[:warehouse_id].blank?
          unless Warehouse.find_by_nr(row[:warehouse_id])
            msg.contents<<"要货仓库不存在"
          end
        end

        unless row[:source_warehouse_id].blank?
          unless Warehouse.find_by_nr(row[:source_warehouse_id])
            msg.contents<<"出货仓库不存在"
          end
        end

        unless row[:order_box_type_id].blank?
          unless OrderBoxType.find_by_nr(row[:order_box_type_id])
            msg.contents<<"料盒类型不存在"
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