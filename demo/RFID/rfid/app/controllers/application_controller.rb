class ApplicationController < ActionController::Base
  # Prevent CSRF attacks by raising an exception.
  # For APIs, you may want to use :null_session instead.
  protect_from_forgery with: :exception

  include ApplicationHelper
  before_filter :set_model
  before_action :configure_permitted_parameters, if: :devise_controller?
  before_action :authenticate_user!

  protected

  def configure_permitted_parameters
    devise_parameter_sanitizer.for(:sign_up) << :user_id
  end

  private
  def set_model
    @model=self.class.name.gsub(/Controller/, '').tableize.singularize
  end

  def model
    self.class.name.gsub(/Controller/, '').classify.constantize
  end
end
