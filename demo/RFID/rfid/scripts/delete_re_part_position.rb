PartPosition.all.each do |pp|
  pps=PartPosition.where(part_id: pp.part_id, position_id: pp.position_id)
  pps.each do |l|
    if l==pps.first!
    else
      l.destroy
    end
  end
end