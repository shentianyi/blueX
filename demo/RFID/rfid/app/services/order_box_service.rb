class OrderBoxService
  # require
  #  nr:string
  def self.details nr
    if ob=OrderBox.find_by_nr(nr)
      OrderBoxPresenter.new(ob).as_basic_feedback
    else
      ApiMessage.new({
                         meta: {
                             code: 400,
                             error_message: '未找到该料盒'
                         }
                     })
    end
  end

end