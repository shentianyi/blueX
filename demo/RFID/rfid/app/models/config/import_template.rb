class ImportTemplate
  USER_EXCEL_TEMPLATE='user.xlsx'
  LOCATION_EXCEL_TEMPLATE='location.xlsx'
  POSITION_EXCEL_TEMPLATE='position.xlsx'
  WAREHOUSE_EXCEL_TEMPLATE='warehouse.xlsx'
  PART_EXCEL_TEMPLATE='part.xlsx'
  ORDER_CAR_EXCEL_TEMPLATE='order_car.xlsx'
  ORDER_BOX_EXCEL_TEMPLATE='order_box.xlsx'
  ORDER_BOX_TYPE_EXCEL_TEMPLATE='order_box_type.xlsx'
  ENTER_STORAGE_EXCEL_TEMPLATE='enter_storage.xlsx'


  def self.method_missing(method_name, *args, &block)
    if method_name.to_s.include?('_template')
      return Base64.urlsafe_encode64(File.join($template_file_path, self.const_get(method_name.upcase)))
    else
      super
    end
  end
end