# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)
# ActiveRecord::Base.transaction do
puts 'create setting regex...'
Setting.transaction do
  unless Setting.find_by_code(Setting::NO_NEED_WEIGHT_BOX_TYPES)
    Setting.create(code: Setting::NO_NEED_WEIGHT_BOX_TYPES, value: 'P', name: '不需要称重扣减库存的料盒类型')
  end
end

# end