class OrderCarStatus<BaseStatus
  INIT=100
  PICKING=200
  OTHER=500

  def self.display state
    case state
      when INIT
        '空闲'
      when PICKING
        '择货中'
      else
        '未知状态'
    end
  end

  def self.menu
    data = []
    self.constants.each do |c|
      unless c==:OTHER
        v = self.const_get(c)
        data << [self.display(v), v]
      end
    end
    data
  end
end