# print package list
module Printer
  class P001<Base
    HEAD=[:id, :whouse, :delivery_date, :user, :total_packages]
    BODY=[:package_id, :part_id, :quantity, :w_date, :receive_position]

    #拖清单和包装箱的打印模板
    def generate_data
      f=LogisticsContainer.find_by_id(self.id)
      #fp=ForkliftPresenter.new(f)
      p = f.presenter

      head={id: p.container_id,
            total_packages: p.sum_packages,
            whouse: p.destinationable_name,
            delivery_date: f.get_dispatch_time,
            user: f.user_id}
      heads=[]

      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end

      #这里肯定是包装箱
      packages=PackagePresenter.init_presenters(LogisticsContainerService.get_all_packages_with_detail(f, 'part_id,container_id'))
      packages.each do |p|
        body={package_id: p.container_id,
              part_id: p.part_id_display,
              quantity: p.quantity_display,
              w_date: p.fifo_time_display,
              receive_position: p.position_nr}
        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
      end
    end
  end
end
