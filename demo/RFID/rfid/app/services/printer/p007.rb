#print fors report
module Printer
  class P007<Base
    HEAD=[:id,:user,:delivery_date,:whouse,:total_package_num,:from_whouse_code,:from_whouse_position]
    BODY=[:part_id,:to_whouse_code,:to_whouse_position,:quantity,:transfer_data]

    def generate_data
      f = LogisticsContainer.find_by_id(self.id)
      packages=PackagePresenter.init_presenters(LogisticsContainerService.get_all_packages_with_detail(f))
      whouse_name = f.destinationable.nil? ? '' : f.destinationable.name
      user = f.user.nil? ? '': f.user.name
      head = {
          id:f.id,
          delivery_date: f.created_at.nil? ? '' : f.created_at.localtime.strftime('%Y.%m.%d %H:%M'),
          whouse: whouse_name,
          user: user,
          total_package_num: packages.count,
          from_whouse_code: SysConfigCache.trans_warehouse_value,
          from_whouse_position: Position.trans_position
      }

      heads=[]
      HEAD.each do |k|
        heads<<{Key: k, Value: head[k]}
      end

      packages.each do |p|

        body = {
            part_id: p.part_id,
            quantity: p.quantity,
            to_whouse_code: whouse_name,
            to_whouse_position:  p.position_nr,
            transfer_data: "\r\r\r#{whouse_name}\r#{p.position_nr}\r#{p.quantity}\r#{p.part_id}"
        }

        bodies=[]
        BODY.each do |k|
          bodies<<{Key: k, Value: body[k]}
        end
        self.data_set <<(heads+bodies)
      end
    end
  end
end