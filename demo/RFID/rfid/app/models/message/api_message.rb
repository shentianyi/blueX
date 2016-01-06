#encoding: utf-8
require 'base_class'

class ApiMessage<CZ::BaseClass
  attr_accessor :meta, :code, :error_message, :data

  def default
    {
        meta: {
            code: 400,
            error_message: ""
        }
    }
  end

  # def as_json
  #   {
  #       result_code: self.result_code,
  #       messages: self.messages
  #   }
  # end
end