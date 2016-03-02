class String
  def is_date?
    true if Date.parse(self) && Time.parse(self) rescue false
  end
end

module ApplicationHelper
  def search
    file_name=@model.pluralize+".xlsx"

    @condition=params[@model]
    query=model.all #.unscoped
    @condition.each do |k, v|
      if (v.is_a?(Fixnum) || v.is_a?(String)) && !v.blank?
        puts @condition.has_key?(k+'_fuzzy')
        if @condition.has_key?(k+'_fuzzy')
          query=query.where("#{k} like ?", "%#{v}%")
        else
          query=query.where(Hash[k, v])
        end
        instance_variable_set("@#{k}", v)
      end
      #if v.is_a?(Array) && !v.empty?
      #  query= v.size==1 ? query.where(Hash[k, v[0]]) : query.in(Hash[k, v]
      #end
      #query=query.where(Hash[k, v]) if v.is_a?(Range)
      if v.is_a?(Hash) && v.values.count==2 && v.values.uniq!=['']
        values=v.values.sort
        file_name=@model.pluralize+values[0]+"--"+values[1]+".xlsx"
        values[0]=Time.parse(values[0]).utc.to_s if values[0].is_date? & values[0].include?('-')
        values[1]=(Time.parse(values[1]).utc-1.second).to_s if values[1].is_date? & values[1].include?('-')
        query=query.where(Hash[k, (values[0]..values[1])])
        v.each do |kk, vv|
          instance_variable_set("@#{k}_#{kk}", vv)
        end
      end
    end

    if block_given?
      query=(yield query)
    end

    if params.has_key? "download"
      send_data(query.to_xlsx(query),
                :type => "application/vnd.openxmlformates-officedocument.spreadsheetml.sheet",
                :filename => file_name)
      #render :json => query.to_xlsx(query)
    else
      instance_variable_set("@#{@model.pluralize}", query.paginate(:page => params[:page], :per_page => 100).all)
      render :index
    end

  end

  def scope_search
    model = params[:model].classify.constantize
    @q = params[:q]
    resultes = model.search_for(@q).paginate(:page => params[:page])
    instance_variable_set("@#{params[:controller]}", resultes)
    render :index
  end

  def strf_time(time)
    time.blank? ? '' : time.localtime.strftime('%Y-%m-%d')
  end
end
