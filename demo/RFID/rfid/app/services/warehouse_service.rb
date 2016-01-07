class WarehouseService

  def self.move_by_car user, params
    # begin
      Storage.transaction do
        PickService.validable_car_and_box(params) do |car, boxes|
          boxes.each do |order_box|
            self.move({
                          user_id: user.id,
                          part_id: order_box.part_id,
                          quantity: order_box.quantity,
                          from_warehouse_id: order_box.source_warehouse_id,
                          to_warehouse_id: order_box.warehouse_id,
                          to_position_id: order_box.position_id,
                          remarks: "RFID MOVE:#{order_box.nr}"
                      })
            order_box.update_attributes(status: OrderBoxStatus::INIT)
          end
          car.update_attributes(status: OrderCarStatus::INIT)
        end
        {
            meta: {
                code: 200,
                message: 'Move Success'
            }
        }
      end
    # rescue => e
    #   {
    #       meta: {
    #           code: 400,
    #           message: e.message
    #       }
    #   }
    # end
  end


  def self.move(params)
    type = MoveType.find_by_nr('MOVE')

    to_warehouse = Warehouse.find_by_id(params[:to_warehouse_id])
    raise "仓库#{params[:to_warehouse]}未找到" unless to_warehouse

    move_data = {to_warehouse_id: to_warehouse.id, to_position_id: params[:to_position_id], move_type_id: type.id}
    move_data[:user_id] = params[:user_id] if params[:user_id].present?
    move_data[:remarks] = params[:remarks] if params[:remarks].present?


    if params[:uniq_nr].present?
      #Move(uniq_nr,toWh,to_position_id,type)
      # find from wh
      storage = Storage.find_by(uniq_nr: params[:uniq_nr])
      raise '包装未入库！' unless storage.blank?

      # update parameters of movement creation
      move_data.update({from_warehouse_id: storage.warehouse_id, from_position_id: storage.position_id,
                        uniq_nr: params[:uniq_nr], quantity: storage.quantity, fifo: storage.fifo,
                        part_id: storage.part_id})
      # create movement
      Movement.create!(move_data)
      # update storage
      storage.update!(warehouse_id: to_warehouse.id, position_id: params[:to_position_id])
    elsif params[:package_nr].present?
      # Move(package_nr,part_id, quantity,toWh, to_position_id,type)
      # find from wh
      if params[:to_position_id].blank?
        raise "目标库位不可空"
      end

      storage = nil
      if params[:part_id].blank?
        if params[:from_warehouse_id].present?
          storage = Storage.find_by(package_nr: params[:package_nr], warehouse_id: params[:from_warehouse_id])
        else
          storage = Storage.find_by(package_nr: params[:package_nr])
          params[:from_warehouse_id] = storage.warehouse_id if storage
        end
        params[:part_id]=storage.part_id if storage
      else
        if params[:from_warehouse_id].present?
          storage = Storage.find_by(package_nr: params[:package_nr], part_id: params[:part_id], warehouse_id: params[:from_warehouse_id])
        else
          storage = Storage.find_by(package_nr: params[:package_nr], part_id: params[:part_id])
          params[:from_warehouse_id] = storage.warehouse_id if storage
        end
      end

      puts "############{storage.to_json}"
      raise "源仓库不存在该唯一码:#{params[:package_nr]}！" if storage.nil? || storage.quantity < 0
      if params[:quantity].blank?
        params[:quantity]=storage.quantity
      end
      raise '移库数量为 0 ！' if params[:quantity].to_i <= 0

      puts "#{storage.quantity}:#{params[:quantity]}"

      if params[:quantity].to_f > storage.quantity
        raise "移库量大于剩余量,唯一码:#{params[:package_nr]}"
      elsif params[:quantity].to_f == storage.quantity
        storage.update!(warehouse_id: to_warehouse.id, position_id: params[:to_position_id], created_at: Time.now)
        move_data[:quantity] = storage.quantity
        move_data[:from_warehouse_id] = params[:from_warehouse_id]
        move_data[:part_id] = storage.part_id
        move_data[:from_position_id] = params[:from_position_id]
        move_data[:package_nr] = params[:package_nr]
        Movement.create!(move_data)
      else
        tostorage = Storage.where(warehouse_id: to_warehouse.id,
                                  part_id: params[:part_id], position_id: params[:to_position_id],
                                  package_nr: params[:package_nr]).order("quantity asc").first

        if tostorage.blank?
          #create n_storage remarks
          data = {part_id: params[:part_id], quantity: params[:quantity], fifo: storage.fifo, warehouse_id: to_warehouse.id, position_id: params[:to_position_id]}
          Storage.create!(data)
        else
          if (tostorage.quantity.to_f + params[:quantity].to_f) == 0
            tostorage.destroy!
          else
            tostorage.update!(quantity: tostorage.quantity + params[:quantity].to_f)
          end
        end

        storage.update!(quantity: storage.quantity - params[:quantity].to_f)
        move_data[:quantity] = params[:quantity]
        move_data[:from_warehouse_id] = params[:from_warehouse_id]
        move_data[:part_id] = storage.part_id
        move_data[:from_position_id] = params[:from_position_id]
        move_data[:package_nr] = params[:package_nr]
        Movement.create!(move_data)
      end

    elsif [:part_id, :quantity].reduce(true) { |seed, i| seed and params.include? i }

      if params[:to_position_id].blank?
        raise "目标库位不可空"
      end

      from_warehouse_id = Warehouse.find_by_id(params[:from_warehouse_id])
      raise "源仓库未找到" unless from_warehouse_id

      #raise "移库数量必须大于零" if  params[:quantity].to_f < 0
      #validate_position(from_warehouse_id, params[:from_position_id])
      # find storage records
      if params[:from_position_id].present?
        storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id, position_id: params[:from_position_id]).where("quantity > ?", 0).order(fifo: :asc)
      else
        storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id).where("quantity > ?", 0).order(fifo: :asc)
      end
      #   if params[:from_position_id].present?
      #   negatives_storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id, position_id: params[:from_position_id]).where("stroages.quantity < ?", 0)
      #   else
      #   negatives_storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id).where("stroages.quantity < ?", 0)
      # end

      # add fifo condition if fifo param exists
      if params[:fifo]
        fifo = validate_fifo_time(params[:fifo])
        storages.where(fifo: fifo)
      end

      # validate sum of storage quantity is enough
      #支持负库存#raise 'No enough quantity in source' if sumquantity = storages.reduce(0) { |seed, s| seed + s.quantity } < params[:quantity]
      lastquantity = params[:quantity].to_f

      if storages.present?

        storages.reduce(params[:quantity].to_f) do |restquantity, storage|

          break if restquantity <= 0

          tostorage = Storage.where(warehouse_id: to_warehouse.id, part_id: params[:part_id], position_id: params[:to_position_id]).order("quantity asc").first
          # update parameters of movement creation
          move_data.update({from_warehouse_id: storage.warehouse_id, from_position_id: storage.position_id,
                            fifo: storage.fifo, part_id: storage.part_id})

          if restquantity.to_f >= storage.quantity.to_f

            move_data[:quantity] = storage.quantity
            lastquantity = restquantity = restquantity.to_f - storage.quantity.to_f

            # move all storage
            if !tostorage.nil?
              if (tostorage.quantity.to_f + storage.quantity.to_f) == 0
                tostorage.destroy!
              else
                if storage.package_nr.blank?
                  tostorage.update!(quantity: tostorage.quantity + storage.quantity)
                else
                  tostorage.update!(quantity: tostorage.quantity + storage.quantity)
                end
              end
              storage.destroy!
            else

              storage.update!(warehouse_id: to_warehouse.id, position_id: params[:to_position_id])
            end
          else

            move_data[:quantity] = restquantity
            # adjust source storage
            storage.update!(quantity: storage.quantity - restquantity)
            # create target storage
            if !tostorage.nil?
              if (tostorage.quantity.to_f + restquantity.to_f) == 0
                tostorage.destroy!
              else
                if storage.package_nr.blank?
                  tostorage.update!(quantity: tostorage.quantity + restquantity)
                else
                  tostorage.update!(quantity: tostorage.quantity + restquantity)
                end
              end
            else
              data = {part_id: storage.part_id, quantity: restquantity,
                      fifo: storage.fifo, warehouse_id: to_warehouse.id,
                      position_id: params[:to_position_id]}
              Storage.create!(data)
            end

            lastquantity = restquantity = 0
          end

          # create movement
          Movement.create!(move_data)
          restquantity
        end

      end

      # #negatives storage default position
      # default_position = ""
      # if params[:from_position_id].blank?
      #   if storages.blank?
      #     default_position = Part.find_by_id(params[:part_id]).default_position(from_warehouse_id.id)
      #   else
      #     default_position = storages.last.position_id
      #   end
      # end

      if lastquantity > 0
        #src
        # negatives_storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id, position_id: params[:from_position_id])

        if params[:from_position_id].present?
          negatives_storage = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id, position_id: params[:from_position_id]).where("quantity < ?", 0).first
        else
          negatives_storage = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse_id.id).where("quantity < ?", 0).first
        end

        if !negatives_storage.blank?
          negatives_storage.update!(quantity: negatives_storage.quantity - lastquantity)
        else
          data = {part_id: params[:part_id], quantity: -lastquantity, warehouse_id: from_warehouse_id.id, position_id: params[:from_position_id]}
          Storage.create!(data)
        end

        #dse
        tostorage = Storage.where(warehouse_id: to_warehouse.id, part_id: params[:part_id], position_id: params[:to_position_id]).order("quantity asc").first
        if !tostorage.blank?
          if (tostorage.quantity.to_f + lastquantity.to_f) == 0
            tostorage.destroy!
          else
            tostorage.update!(quantity: tostorage.quantity + lastquantity)
          end
        else
          data = {part_id: params[:part_id], quantity: lastquantity, warehouse_id: to_warehouse.id, position_id: params[:to_position_id]}
          Storage.create!(data)
        end

        #movement
        move_data.update({from_warehouse_id: params[:from_warehouse_id], from_position_id: params[:from_position_id], part_id: params[:part_id], quantity: lastquantity})
        Movement.create!(move_data)

      end

    end
  end

end