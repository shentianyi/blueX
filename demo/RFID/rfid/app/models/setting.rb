class Setting < ActiveRecord::Base
  validates :code, :value, presence: true
  NO_NEED_WEIGHT_BOX_TYPES='not_need_weight_box_types'

  def self.method_missing(method_name, *args, &block)
    if method_name.match(/\?$/)
      if setting=Setting.where(code: method_name.to_s.sub(/\?$/,'')).first
        return setting.value=='1'
      else
        super
      end
    elsif setting=Setting.where(code: method_name).first
      return setting.value
    else
      super
    end
  end



  def self.not_need_weight_box_type_values
    self.not_need_weight_box_types.split(',')
  end
end