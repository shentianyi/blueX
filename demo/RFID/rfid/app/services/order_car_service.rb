class OrderCarService
  # require
  #  nr:string
  def self.details nr
    if oc=OrderCar.find_by_nr(nr)
      OrderCarPresenter.new(oc).as_basic_feedback
    else
      ApiMessage.new({
                         meta: {
                             code: 400,
                             error_message: '未找到该料车'
                         }
                     })
    end
  end

end