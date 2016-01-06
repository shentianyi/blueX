#encoding: utf-8
class UserPresenter<Presenter
  Delegators=[:id, :nr, :name, :email, :role_id]
  def_delegators :@user, *Delegators

  def initialize(user)
    @user=user
    self.delegators =Delegators
  end

  def as_basic_info
    if @user.nil?
      nil
    else
      {
          id: @user.id,
          nr: @user.nr
      }
    end
  end

  def as_basic_feedback(messages=nil, result_code=nil)
    if @user.nil?
      {
          meta: {
              code: 400,
              error_message:'Signed failed, User Nr Or Password Error'
          }
      }
    else
      {
          meta: {
              code: result_code||200,
              message:'Signed Success'
          },
          data: {
              id: @user.id,
              nr: @user.nr,
              email: @user.email,
              name: @user.name,
              token: @user.access_token.token
          }
      }
    end
  end

end