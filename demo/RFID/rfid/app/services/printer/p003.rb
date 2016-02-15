# print accepted
module Printer
  class P003<Base
    HEAD=[:id, :receive_addr, :user, :receive_date]
    BODY=[:forklift_id, :quantity, :receive_qty, :status, :whouse]

    def generate_data
      d=LogisticsContainer.find(self.id)
      dp=DeliveryPresenter.new(d)

      head={id: d.container_id,
            receive_addr: d.des_location.nil? ? '' : d.des_location.address,
            user: dp.received_user,
            receive_date: dp.received_date}
      heads=[]
      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end
      forklifts=LogisticsContainerService.get_forklifts(d)
      forklifts.each do |f|
        body={forklift_id: f.container_id,
              quantity: LogisticsContainerService.count_all_packages(f),
              receive_qty: LogisticsContainerService.count_accepted_packages(f),
              status: f.state_display,
              whouse: f.destinationable.nil? ? '' : f.destinationable.name}
        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
      end
    end
  end
end
