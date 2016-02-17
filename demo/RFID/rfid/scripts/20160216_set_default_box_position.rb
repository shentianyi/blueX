Position.transaction do
  Warehouse.all.each do |w|
  unless p=w.positions.where(nr:w.nr).first
    p= w.positions.build(nr: w.nr, name: w.name)
    p.save
end
    w.order_boxes.update_all(position_id: p.id)
  end
end
