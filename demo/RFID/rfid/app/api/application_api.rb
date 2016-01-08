class ApplicationAPI < Grape::API
  include APIGuard
  format :json

  mount V1::Base
  mount V1::Service::ServiceBase
end
