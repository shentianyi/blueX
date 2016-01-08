module FileHandler
  module Excel
    class PartHandler<Base
      HEADERS=[
          :nr, :name, :description, :short_description, :part_type_id, :color_id, :measure_unit_id, :purchase_unit_id, :custom_nr, :cross_section, :weight
      ]

      def self.import(file)
        msg = Message.new
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        validate_msg = validate_import(file)
        if validate_msg.result
          #validate file
          begin
            Part.transaction do
              2.upto(book.last_row) do |line|
                row = {}
                HEADERS.each_with_index do |k, i|
                  row[k] = book.cell(line, i+1).to_s.strip
                end
                row[:part_type_id] = PartType.find_by_nr(row[:part_type_id]).id unless row[:part_type_id].blank?
                row[:color_id] = Color.find_by_nr(row[:color_id]).id unless row[:color_id].blank?
                row[:measure_unit_id] = Unit.find_by_nr(row[:measure_unit_id]).id unless row[:measure_unit_id].blank?
                row[:purchase_unit_id] = Unit.find_by_nr(row[:purchase_unit_id]).id unless row[:purchase_unit_id].blank?

                row.delete(:status) if row[:status].blank?

                s =Part.new(row)
                unless s.save
                  puts s.errors.to_json
                  raise s.errors.to_json
                end
              end
            end
            msg.result = true
            msg.content = "导入零件信息成功！"
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

        if Part.find_by_nr(row[:nr])
          msg.contents<<"该零件已存在"
        end

        unless PartType.find_by_nr(row[:part_type_id])
          msg.contents<<"该零件类型不存在"
        end

        unless Color.find_by_nr(row[:color_id])
          msg.contents<<"该颜色不存在"
        end

        unless Unit.find_by_nr(row[:measure_unit_id])
          msg.contents<<"计量单位#{row[:measure_unit_id]}不存在"
        end

        unless Unit.find_by_nr(row[:purchase_unit_id])
          msg.contents<<"计量单位#{row[:purchase_unit_id]}不存在"
        end

        unless msg.result=(msg.contents.size==0)
          msg.content=msg.contents.join('/')
        end
        msg
      end
    end
  end
end