module AutoKey

  extend ActiveSupport::Concern
  included do
    before_create :generate_auto_key

    def generate_auto_key
      if self.respond_to?(:nr) && self.nr.blank?
        self.nr =Time.now.strftime('%Y%m%d%H%M%S%L')
      end
    end


  end
end
