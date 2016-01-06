class WarehouseService

  def self.move_by_car user, params
    Storage.transaction do
      PickService.validable_car_and_box(params) do |car, boxs|

      end

    end
  end







  def move(params)
    type = MoveType.find_by_nr('MOVE')

    to_warehouse = Warehouse.find_by_id(params[:to_warehouse_id])
    raise "仓库#{params[:to_warehouse]}未找到" unless to_warehouse

    move_data = {to_warehouse_id: to_warehouse.id, to_position_id: params[:to_position_id], move_type_id: type.id}
    move_data[:user_id] = params[:user_id] if params[:user_id].present?
    move_data[:remarks] = params[:remarks] if params[:remarks].present?

    if params[:uniq_nr].present?
      storage = Storage.find_by_uniq_nr( params[:uniq_nr])
      raise '包装未入库！' unless storage.blank?

      # update parameters of movement creation
      move_data.update({from_warehouse_id: storage.warehouse_id, from_position_id: storage.position_id,
                        uniq_nr: storage.uniq_nr, quantity: storage.quantity, fifo: storage.fifo, part_id: storage.part_id})
      # create movement
      Movement.create!(move_data)

      # update storage
      storage.update!(warehouse_id: to_warehouse.id, position_id: params[:to_position_id])
    elsif params[:package_nr].present?
      storage = nil
      if params[:part_id].blank?
        storage = Storage.find_by(package_nr: params[:package_nr])
        params[:part_id]=storage.part_id if storage
      else
        storage = Storage.find_by(package_nr: params[:package_nr], part_id: params[:part_id])
      end

      raise '包装未入库！' if storage.blank?

      if params[:quantity].blank?
        params[:quantity]=storage.quantity
      end


      puts "#{storage.quantity}:#{params[:quantity]}"

      if storage.quantity > 0
        # validate package qty
        # 正库存
        raise '移库量大于剩余量' if params[:quantity].to_f > storage.quantity
        if params[:quantity].to_f == storage.quantity
          storage.update!(warehouse_id: to_warehouse.id, position_id: params[:to_position_id])
          return
        end
      end
      # no means ignore pACKID BUT INCLUDE
      noPackIdStorages = Storage.where(part_id: storage.part_id,
                                       warehouse_id: storage.warehouse_id,
                                       position_id: storage.position_id)
                             .where("storages.quantity > ?", 0).select("storages.*, SUM(storages.quantity) as total_qty").order("storages.fifo asc")

      # adjust storage
      ## adjust to storage

      noPackIdStorages.reduce(params[:quantity].to_f) do |restqty, noPackIdStorage|
        break if restqty.to_f <= 0
        move_data.update({from_warehouse_id: storage.warehouse_id, from_position_id: storage.position_id,
                          fifo: noPackIdStorage.fifo, part_id: storage.part_id})

        tostorage = Storage.where(warehouse_id: to_warehouse.id, part_id: params[:part_id],
                                  position_id: params[:to_position_id], package_nr: nil).first

        if restqty.to_f >= noPackIdStorage.quantity

          move_data[:quantity] = noPackIdStorage.quantity
          if tostorage.nil?
            noPackIdStorage.update!(warehouse_id: to_warehouse.id, position_id: params[:to_position_id])
          else
            tostorage.update!(quantity: tostorage.quantity + noPackIdStorage.quantity)
            noPackIdStorage.destroy!
          end
          restqty = restqty.to_f - noPackIdStorage.quantity
        else

          move_data[:quantity] = restqty
          noPackIdStorage.update!(quantity: storage.quantity - restqty.to_f)
          if tostorage.nil?
            data = {part_id: noPackIdStorage.part_id, quantity: restqty,
                    fifo: noPackIdStorage.fifo, warehouse_id: to_warehouse.id,
                    position_id: params[:to_position_id]}
            Storage.create!(data)
          else
            tostorage.update!(quantity: tostorage.quantity + restqty.to_f)
          end
          restqty = 0
        end

        # create movements
        Movement.create!(move_data)
        restqty
      end

    elsif [:part_id, :quantity].reduce(true) { |seed, i| seed and params.include? i }

      if params[:to_position_id].blank?
        raise "目标库位:#{params[:to_position_id]}不可空"
      end

      # Move(partNr, qty, fromWh,fromPosition,toWh,toPosition,type)
      # Move(partNr, qty, fifo,fromWh,fromPosition,toWh,toPosition,type)
      from_warehouse = Warehouse.find_by_id(params[:from_warehouse_id])
      raise "目标仓库:#{params[:from_warehouse_id]}未找到" unless from_warehouse

      # find storage records
      if params[:from_position_id].present?
        storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse.id, position: params[:from_position_id]).where("storages.qty > ?", 0)
      else
        storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse.id).where("storages.qty > ?", 0)
      end

      # add fifo condition if fifo param exists
      if params[:fifo]
        fifo = validate_fifo_time(params[:fifo])
        storages.where(fifo: fifo)
      end
      # order by fifo
      storages.order(fifo: :asc)
      # validate sum of storage qty is enough
      #支持负库存#raise 'No enough qty in source' if sumqty = storages.reduce(0) { |seed, s| seed + s.qty } < params[:qty]
      lastqty = params[:quantity].to_f

      if storages.present?

        storages.reduce(params[:quantity].to_f) do |restqty, storage|

          break if restqty <= 0

          tostorage = Storage.where(warehouse_id: to_warehouse.id, part_id: params[:part_id], position: params[:to_position_id]).first
          # update parameters of movement creation
          move_data.update({from_warehouse_id: storage.warehouse_id, from_position_id: storage.position_id,
                            fifo: storage.fifo, part_id: storage.part_id})

          if restqty.to_f >= storage.quantity.to_f

            move_data[:quantity] = storage.quantity
            lastqty = restqty = restqty.to_f - storage.quantity.to_f

            # move all storage
            if !tostorage.nil?
              if (tostorage.quantity.to_f + storage.quantity.to_f) == 0
                tostorage.destroy!
              else
                tostorage.update!(quantity: tostorage.quantity + storage.quantity)
              end
              storage.destroy!
            else

              storage.update!(warehouse_id: to_warehouse.id, position: params[:to_position_id])
            end
          else

            move_data[:quantity] = restqty
            # adjust source storage
            storage.update!(quantity: storage.quantity - restqty)
            # create target storage
            if !tostorage.nil?
              if (tostorage.quantity.to_f + restqty.to_f) == 0
                tostorage.destroy!
              else
                tostorage.update!(quantity: tostorage.quantity + restqty)
              end
            else
              data = {part_id: storage.part_id, quantity: restqty, fifo: storage.fifo,warehouse_id: to_warehouse.id,
                      position_id: params[:to_position_id]}
              Storage.create!(data)
            end

            lastqty = restqty = 0
          end

          # create movement
          Movement.create!(move_data)
          restqty
        end

      end

      if lastqty > 0
        #src

        if params[:from_position_id].present?
          negatives_storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse.id, positio_id: params[:from_position_id]).where("storages.qty < ?", 0)
        else
          negatives_storages = Storage.where(part_id: params[:part_id], warehouse_id: from_warehouse.id).where("storages.qty < ?", 0)
        end

        if negatives_storages.present?
          negatives_storages.first.update!(quantity: negatives_storages.first.quantity - lastqty)
        else
          data = {part_id: params[:part_id], quantity: -lastqty, warehouse_id: from_warehouse.id, position: params[:from_position_id]||''}
          Storage.create!(data)
        end

        #dse
        tostorage = Storage.where(warehouse_id: to_warehouse.id, part_id: params[:part_id], position_id: params[:to_position_id]).first
        if !tostorage.nil?
          if (tostorage.quantity.to_f + lastqty.to_f) == 0
            tostorage.destroy!
          else
            tostorage.update!(quantity: tostorage.quantity + lastqty)
          end
        else
          data = {part_id: params[:part_id], quantity: lastqty, warehouse_id: to_warehouse.id, position_id: params[:to_position_id}

          Storage.create!(data)
        end

        #movement ?
        move_data.update({from_id: params[:toWh], fromPosition: params[:toPosition], part_id: params[:part_id], quantity: lastqty})
        Movement.create!(move_data)

      end

    end
    {result: 1, content: 'move success'}

  end

end