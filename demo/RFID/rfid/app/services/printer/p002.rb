# print delivery
module Printer
  class P002<Base
    HEAD=[:id, :send_addr, :receive_addr, :delivery_date, :total_packages]
    BODY=[:forklift_id, :quantity, :whouse]


    def generate_data
      d=LogisticsContainer.find(self.id)
      dp=DeliveryPresenter.new(d)

      head={id: d.container_id,
            total_packages: LogisticsContainerService.count_all_packages(d),
            send_addr: d.source_location.nil? ? '' : d.source_location.address,
            receive_addr: d.des_location.nil? ? '' : d.des_location.address,
            delivery_date: dp.delivery_date}
      heads=[]
      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end
      forklifts=LogisticsContainerService.get_forklifts(d)
      forklifts.each do |f|
        body={forklift_id: f.container_id, quantity: LogisticsContainerService.count_all_packages(f), whouse: f.destinationable.nil? ? '' : f.destinationable.name}
        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
      end
    end
  end
end
