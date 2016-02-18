class OrderItemStatus<BaseStatus
  INIT = 100
  PICKING = 200
  PICKED = 300
  DELIVERED=400
  ABORTED = 500

  def self.display state
    case state
      when INIT
        '新建'
      when PICKING
        '进行中'
      when PICKED
        '完成择货'
      when DELIVERED
        '完成交接'
      else
        '放弃'
    end
  end
end