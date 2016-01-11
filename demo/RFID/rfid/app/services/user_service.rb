class UserService
  # require
  #  nr:string
  #  password: string
  def self.sign_in params
    UserPresenter.new(if (user=User.find_for_database_authentication(nr: params[:nr])) && user.valid_password?(params[:password])
                        user
                      else
                        nil
                      end).as_basic_feedback(nil,200)
  end

end