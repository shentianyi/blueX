module FileHandler
  module Excel
    class UserHandler<Base
      HEADERS=[
          'nr', 'name', 'email', 'role_id', 'password', 'password_confirmation', 'can_edit', 'can_delete', 'operator'
      ]

      def self.import(file)
        msg = Message.new
        book = Roo::Excelx.new file.full_path
        book.default_sheet = book.sheets.first

        validate_msg = validate_import(file)
        if validate_msg.result
          #validate file
          begin
            User.transaction do
              2.upto(book.last_row) do |line|
                row = {}
                HEADERS.each_with_index do |k, i|
                  row[k] = book.cell(line, i+1).to_s.strip
                  row[k] = row[k].sub(/\.0/, '') if k=='nr'
                  row[k] = row[k].sub(/\.0/, '') if k=='role_id'
                  row[k] = row[k].sub(/\.0/, '') if k=='password'
                  row[k] = row[k].sub(/\.0/, '') if k=='password_confirmation'
                end

                if u=User.find_by_nr(row['nr'])
                  if row['operator'].blank? || row['operator']=='update'
                    u.update(row.except('nr', 'operator'))
                  elsif row['operator']=='delete'
                    u.destroy
                  end
                else
                  if row['operator'].blank? || row['operator']=='create'
                    s =User.new(row.except('operator'))
                    unless s.save
                      puts s.errors.to_json
                      raise s.errors.to_json
                    end
                  end
                end

              end
            end
            msg.result = true
            msg.content = "导入员工信息成功！"
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

        unless msg.result=(msg.contents.size==0)
          msg.content=msg.contents.join('/')
        end
        msg
      end
    end
  end
end