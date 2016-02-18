class OrderBoxStatus<BaseStatus
  INIT=100
  PICKING=200
  PICKED=300
  OTHER=500

  def self.display state
    case state
      when INIT
        '空闲'
      when PICKING
        '进行中'
      when PICKED
        '已择货'
      else
        '未知状态'
    end
  end

  def self.menu
    data = []
    self.constants.each do |c|
      v = self.const_get(c)
      data << [self.display(v), v]
    end
    data
  end
end