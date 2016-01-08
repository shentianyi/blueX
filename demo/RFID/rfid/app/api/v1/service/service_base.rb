module V1
  module Service
    class ServiceBase < ApplicationAPI
      include APIGuard
      version 'v1', :using => :path
      namespace :service do
        mount PrintServiceAPI
      end
    end
  end
end
