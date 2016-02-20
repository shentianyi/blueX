class PickItemStatus<BaseStatus
  INIT = 100
  PICKING = 200
  PICKED = 300
  ABORTED = 400

  def self.display state
    case state
      when INIT
        '新建'
      when PICKING
        '择货中'
      when PICKED
        '已择货'
      else
        '放弃'
    end
  end
end