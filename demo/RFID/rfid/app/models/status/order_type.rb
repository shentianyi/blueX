class OrderType
  INIT = 100
  PROCESSING = 200
  HANDLED = 300
  OTHER = 400

  def self.display state
    case state
      when INIT
        '初始化'
      when PROCESSING
        '进行中'
      when HANDLED
        '已处理'
      else
        '未知'
    end
  end
end