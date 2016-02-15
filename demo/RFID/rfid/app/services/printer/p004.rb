# print rejected packages detail
module Printer
  class P004<Base
    HEAD=[:id, :receive_addr, :user, :receive_date]
    BODY=[:forklift_id, :package_id, :whouse]


    def generate_data
      d=LogisticsContainer.find(self.id)
      #dp=d.presenter #DeliveryPresenter.new(d)

      r_record = d.get_receive_record

      head={id: d.container_id,
            receive_addr: d.des_location.nil? ? '' : d.des_location.address,
            user: (r_record.impl.name if r_record),
            receive_date: d.get_receive_time}

      heads=[]
      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end

      packages=LogisticsContainerService.get_all_rejected_packages(d)
      packages.each do |p|
        f=p.parent
        forklift_id = f.nil? ? ' ':f.container_id
        body={forklift_id: forklift_id, package_id: p.container_id, whouse: p.destinationable.nil? ? '' : p.destinationable.name}
        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
      end
    end
  end
end
