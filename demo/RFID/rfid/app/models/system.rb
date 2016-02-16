class System


  def self.init
    # init first system user
    unless user=User.find_by_email('admin@leoni.com')
      user=User.create({nr: 'admin', name: 'admin', password: '123456@',
                        password_confirmation: '123456@', role_id: 400,
                        email: 'leoni@leoni.com', can_edit: 1, can_delete: 0})
    end

    # unless user=User.find_by_email('rfid@rfid.com')
    #   user=User.create({nr: 'rfid', name: 'rfid', password: 'rfid', password_confirmation: 'rfid', role_id: 400, email: 'rfid@rfid.com', can_edit: 1, can_delete: 1})
    # end
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