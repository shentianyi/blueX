# This file should contain all the record creation needed to seed the database with its default values.
# The data can then be loaded with the rake db:seed (or created alongside the db with db:setup).
#
# Examples:
#
#   cities = City.create([{ name: 'Chicago' }, { name: 'Copenhagen' }])
#   Mayor.create(name: 'Emanuel', city: cities.first)
# ActiveRecord::Base.transaction do

  unless location=Location.find_by_nr('Leoni')
    location= Location.create(nr: 'Leoni', name: 'Leoni')
  end

  unless warehouse_store=Warehouse.find_by_nr('MB_STORE')
    warehouse_store=Warehouse.create(nr: 'MB_STORE', location: location)
  end

  unless warehouse_produce=Warehouse.find_by_nr('MB_PRODUCE')
    warehouse_produce=Warehouse.create(nr: 'MB_PRODUCE', location: location)
  end


  unless order_box_type=OrderBoxType.find_by_name('BigBox')
    order_box_type=OrderBoxType.create(name:'BigBox')
  end

  100.times do |i|
    unless position=Position.find_by_nr("POSI#{i}")
      position=Position.create(nr: "POSI#{i}", warehouse: warehouse_store)
    end

    unless part=Part.find_by_nr("PART#{i}")
      part=Part.create(nr: "PART#{i}")
    end

    unless part_position=PartPosition.where(part_id: part.id, position_id: position.id).first
      part_position=PartPosition.create(part_id: part.id, position_id: position.id)
    end

    nr= '%04d' % i.to_s
    unless order_box=OrderBox.find_by_nr(nr)
      order_box=OrderBox.create(nr:nr,rfid_nr:nr,quantity:i,part:part,
                                warehouse:warehouse_produce,
                                source_warehouse:warehouse_store,
                                order_box_type:order_box_type)
    end
  end

  9.times do |i|
    unless order_car=OrderCar.find_by_nr( "A00#{i}")
      order_car=OrderCar.create(nr: "A00#{i}",rfid_nr:"A00#{i}", warehouse: warehouse_produce)
    end
  end




# end