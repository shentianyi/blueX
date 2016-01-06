module V1
  class SystemAPI < Base
    guard_all!

    namespace :system do
      get :utc do
        {date: Time.now.utc.to_s}
      end


    end
  end
end
