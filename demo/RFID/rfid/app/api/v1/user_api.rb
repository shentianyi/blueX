module V1
  class UserAPI < Base
    namespace :user_sessions do
      guard_locale!

      # user sign in
      params do
        requires :nr, type: String, desc: 'user nr'
        requires :password, type: String, desc: 'user password'
      end
      post do
        UserService.sign_in(nr: params[:nr],
                            password: params[:password])
      end

      delete do
        guard!
        status 200
        user = User.find_by_email(current_user.email)
        # user.update_attributes(last_request_at: Time.now.utc.to_s)
        {result_code: '1', msg: ["Signed Out"]}
      end

    end
  end
end
