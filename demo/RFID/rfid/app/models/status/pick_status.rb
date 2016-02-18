class PickStatus<BaseStatus
  INIT = 100
  PICKING = 200
  PICKED = 300
  ABORTED = 400

  def self.display state
    case state
      when INIT
        '新建'
      when PICKING
        '进行中'
      when PICKED
        '完成'
      else
        '放弃'
    end
  end
end