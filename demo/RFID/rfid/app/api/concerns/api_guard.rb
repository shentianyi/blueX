# Guard API with OAuth 2.0 Access Token

require 'rack/oauth2'

module APIGuard
  extend ActiveSupport::Concern

  included do |base|
    # OAuth2 Resource Server Authentication
    use Rack::OAuth2::Server::Resource::Bearer, 'The API' do |request|
      request.access_token
    end

    helpers HelperMethods

    install_error_responders(base)
  end

# Helper Methods for Grape Endpoint
  module HelperMethods
    LOCALE_MAP={ZH: 'zh', CN: 'zh', EN: 'en', DE: 'de'}
    # Invokes the doorkeeper guard.
    #
    # If token string is blank, then it raises MissingTokenError.
    #
    # If token is presented and valid, then it sets @current_user.
    #
    # If the token does not have sufficient scopes to cover the requred scopes,
    # then it raises InsufficientScopeError.
    #
    # If the token is expired, then it raises ExpiredError.
    #
    # If the token is revoked, then it raises RevokedError.
    #
    # If the token is not found (nil), then it raises TokenNotFoundError.
    #
    # Arguments:
    #
    #   scopes: (optional) scopes required for this guard.
    #           Defaults to empty array.
    #
    def guard!(scopes= [])
      begin
        if request.env['HTTP_AUTHORIZATION'].present?
          if request.env['HTTP_AUTHORIZATION'].split(' ')[0]=='Bearer'
            guard_by_token(scopes)
          else
            guard_by_basic
          end
        else
          raise NoAuthError
        end
      rescue
        raise NoAuthError
      end
    end

    def guard_locale

      puts '-----------------------------------------------'
      puts request.env['CONTENT_TYPE']

      puts '-----------------------------------------------'

      I18n.locale=locale
    end

    def locale
      @locale||= get_locale
    end

    def current_user
      @current_user
    end

    def current_tenant
      @current_tenant
    end

    def current_ability
      @current_ability
    end

    private
    def guard_by_basic
      auth_header = env['HTTP_AUTHORIZATION'].split(' ')
      user, pass = Base64.decode64(auth_header[1]).split(':')

      Rails.logger.debug '*************use auth basic'
      Rails.logger.debug user
      Rails.logger.debug pass
      Rails.logger.debug '*************use auth basic'

      if (user=User.find_for_database_authentication(:email => user)) && user.valid_password?(pass)
        @current_user=user
        @current_tenant=user.tenant
        @current_ability=Ability.new(@current_user)
      else
        raise BasicAuthError
      end
    end

    def guard_by_token(scopes= [])
      token_string = get_token_string()

      if token_string.blank?
        raise MissingTokenError

      elsif (access_token = find_access_token(token_string)).nil?
        raise TokenNotFoundError

      else
        case validate_access_token(access_token, scopes)
          when Oauth2::AccessTokenValidationService::INSUFFICIENT_SCOPE
            raise InsufficientScopeError.new(scopes)
          when Oauth2::AccessTokenValidationService::EXPIRED
            raise ExpiredError
          when Oauth2::AccessTokenValidationService::REVOKED
            raise RevokedError
          when Oauth2::AccessTokenValidationService::VALID
            @current_user = User.find(access_token.resource_owner_id)
            @current_tenant=@current_user.tenant
            @current_ability=Ability.new(@current_user)
        end
      end
    end


    def get_token_string
      # The token was stored after the authenticator was invoked.
      # It could be nil. The authenticator does not check its existence.
      request.env[Rack::OAuth2::Server::Resource::ACCESS_TOKEN]
    end

    def find_access_token(token_string)
      Doorkeeper::AccessToken.by_token(token_string)
    end

    def validate_access_token(access_token, scopes)
      Oauth2::AccessTokenValidationService.validate(access_token, scopes: scopes)
    end

    def get_locale
      Rails.logger.debug("***http localization header:#{request.env['HTTP_ACCEPT_LANGUAGE'].present?}")

      Rails.logger.debug("***http localization header:#{request.env['HTTP_ACCEPT_LANGUAGE']}")

      locale_header=request.env['HTTP_LOCALIZATION']||request.env['HTTP_ACCEPT_LANGUAGE']

      if locale_header.present?
        LOCALE_MAP[locale_header.upcase.to_sym] || 'en'
      else
        'en'
      end
    end
  end

  module ClassMethods
    # Installs the doorkeeper guard on the whole Grape API endpoint.
    #
    # Arguments:
    #
    #   scopes: (optional) scopes required for this guard.
    #           Defaults to empty array.
    #
    def guard_auth!(scopes=[])
      before do
        guard! scopes: scopes
      end
    end

    def guard_all!(scopes=[])
      before do
        guard_locale
        guard! scopes: scopes
      end
    end

    def guard_locale!
      before do
        guard_locale
      end
    end

    private
    def install_error_responders(base)
      error_classes = [NoAuthError, BasicAuthError, MissingTokenError, TokenNotFoundError,
                       ExpiredError, RevokedError, InsufficientScopeError]

      base.send :rescue_from, *error_classes, oauth2_bearer_token_error_handler
    end

    def oauth2_bearer_token_error_handler
      Proc.new { |e|
        response = case e
                     when NoAuthError
                       Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new(
                           :invalid_tokens,
                           "Invalid User Info.")
                     when BasicAuthError
                       Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new(
                           :invalid_token,
                           "Invalid User Info.")
                     when MissingTokenError
                       Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new

                     when TokenNotFoundError
                       Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new(
                           :invalid_token,
                           "Bad Access Token.")
                     when ExpiredError
                       #Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new(
                       #      :invalid_token,
                       #     "Token is expired. You can either do re-authorization or token refresh.")

                       r= Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new(
                           :expired_token,
                           "Token is expired. You can resign in to get new token.")

                       r.status=403
                       r
                     when RevokedError
                       Rack::OAuth2::Server::Resource::Bearer::Unauthorized.new(
                           :invalid_token,
                           "Token was revoked. You have to re-authorize from the user.")

                     when InsufficientScopeError
                       # FIXME: ForbiddenError (inherited from Bearer::Forbidden of Rack::Oauth2)
                       # does not include WWW-Authenticate header, which breaks the standard.
                       Rack::OAuth2::Server::Resource::Bearer::Forbidden.new(
                           :insufficient_scope,
                           Rack::OAuth2::Server::Resource::ErrorMethods::DEFAULT_DESCRIPTION[:insufficient_scope],
                           {:scope => e.scopes})
                   end

        response.finish
      }
    end
  end

#
# Exceptions
#

  class NoAuthError<StandardError;
  end
  class BasicAuthError<StandardError;
  end
  class MissingTokenError < StandardError;
  end

  class TokenNotFoundError < StandardError;
  end

  class ExpiredError < StandardError;
  end

  class RevokedError < StandardError;
  end

  class InsufficientScopeError < StandardError
    attr_reader :scopes

    def initialize(scopes)
      @scopes = scopes
    end
  end
end
