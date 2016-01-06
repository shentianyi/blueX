class StorageService

  def self.stock warehouse, part
    return 0 if Warehouse.find_by_id(warehouse).blank? || Part.find_by_id(part).blank?
    storage=Storage.select("SUM(storages.quantity) as stock").where(warehouse_id: warehouse, part_id: part)
    if storage.blank?
      0
    else
      storage.first.stock
    end
  end

  def self.positions warehouse, part
    Position.joins('RIGHT OUTER JOIN storages ON positions.id = storages.position_id').where(storages: {warehouse_id: warehouse, part_id: part}).select(:nr).pluck(:nr).uniq
  end

end