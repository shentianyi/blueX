class BaseStatus
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