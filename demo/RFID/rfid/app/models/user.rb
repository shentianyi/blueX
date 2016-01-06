class User < ActiveRecord::Base

  has_many :access_tokens, class_name: 'Doorkeeper::AccessToken', foreign_key: :resource_owner_id

  # Include default devise modules. Others available are:
  # :confirmable, :lockable, :timeoutable and :omniauthable
  devise :database_authenticatable, :registerable,
         :recoverable, :rememberable, :trackable, :validatable, :authentication_keys => [:nr]

  after_create :generate_access_token


  # the last access token for user
  def access_token
    access_tokens.where(application_id: System.default_app.id,
                        revoked_at: nil).where('date_add(created_at,interval expires_in second) > ?', Time.now.utc).
        order('created_at desc').
        limit(1).
        first || generate_access_token
  end

  def method_missing(method_name, *args, &block)
    if Role::RoleMethods.include?(method_name)
      Role.send(method_name, self.role_id)
    else
      super
    end
  end

  def role
    Role.display self.role_id
  end

  # private
  # generate token
  def generate_access_token
    if System.default_app
      Doorkeeper::AccessToken.create!(application_id: System.default_app.id,
                                      resource_owner_id: self.id,
                                      expires_in: Doorkeeper.configuration.access_token_expires_in)
    end
  end

end
