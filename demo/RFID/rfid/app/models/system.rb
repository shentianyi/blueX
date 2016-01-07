class System


  def self.init
    # init first system user
    unless user=User.find_by_email('leoni@warehouse.com')
      user=User.new({nr: 'leoni_wms', name: 'leoni_wms', password: '123456@', password_confirmation: '123456@', role_id: 100, email: 'leoni@warehouse.com'})
    end
    # user.update_attributes(:is_sys => true)

    # init oauth app
    unless default_app
      app=Doorkeeper::Application.new(name: Settings.oauth.application.name,
                                      uid: Settings.oauth.application.uid,
                                      redirect_uri: Settings.oauth.application.redirect_uri)
      app.owner = user
      app.save
    end

    # init first system user access token
    user.generate_access_token

    unless MoveType.find_by_nr('MOVE')
      MoveType.create(nr:'MOVE')
    end

    unless MoveType.find_by_nr('ENTRY')
      MoveType.create(nr:'ENTRY')
    end

  end

  def self.default_app
    Doorkeeper::Application.by_uid(Settings.oauth.application.uid)
  end
end