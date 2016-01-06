# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)
ActiveRecord::Base.transaction do

  unless user=User.find_by_nr('admin')
    user = User.create({nr: 'admin', name: 'Admin', password: '123456@', password_confirmation: '123456@', role_id: 100, email: 'admin@ci.com'})
  end

  unless user=User.find_by_nr('leoni_wms')
    user = User.create({nr: 'leoni_wms', name: 'leoni_wms', password: '123456@', password_confirmation: '123456@', role_id: 100, email: 'leoni@warehouse.com'})
  end

  unless user=User.find_by_nr('leoni1')
    user = User.create({nr: 'leoni1', name: 'leoni1', password: '123456@', password_confirmation: '123456@', role_id: 400, email: 'leoni1@warehouse.com'})
  end

end