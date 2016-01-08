module FileHandler
  module Excel
    class PositionHandler<Base
      HEADERS=[
          :nr, :name, :description, :warehouse_id
      ]

      def self.import(file)
        msg = Message.new
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        validate_msg = validate_import(file)
        if validate_msg.result
          #validate file
          begin
            Position.transaction do
              2.upto(book.last_row) do |line|
                row = {}
                HEADERS.each_with_index do |k, i|
                  row[k] = book.cell(line, i+1).to_s.strip
                end
                row[:warehouse_id] = Warehouse.find_by_nr(row[:warehouse_id]).id unless row[:warehouse_id].blank?

                s =Position.new(row)
                unless s.save
                  puts s.errors.to_json
                  raise s.errors.to_json
                end
              end
            end
            msg.result = true
            msg.content = "导入库位信息成功！"
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

        if Position.find_by_nr(row[:nr])
          msg.contents<<"该库位已存在"
        end

        unless row[:warehouse_id].blank?
          unless Warehouse.find_by_nr(row[:warehouse_id])
            msg.contents<<"所属仓库不存在"
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