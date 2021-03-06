module V1
  class Base < ApplicationAPI

    version 'v1', :using => :path

    mount OrderCarAPI
    mount OrderBoxAPI
    mount UserAPI
    mount OrderAPI
    mount PickAPI
    mount WarehouseAPI
  end
end
