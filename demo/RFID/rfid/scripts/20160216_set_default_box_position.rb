Position.transaction do
  Warehouse.all.each do |w|
    p= w.positions.build(nr: w.nr, name: w.name)
    p.save
    w.order_boxes.update_all(position_id: p.id)
  end
end