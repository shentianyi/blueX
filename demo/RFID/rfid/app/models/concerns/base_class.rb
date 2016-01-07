class BaseClass

  def initialize(args={})

    if self.respond_to?(:default)
      self.default.each do |k, v|
        instance_variable_set "@#{k}", v
      end
    end

    args.each do |k, v|

      instance_variable_set "@#{k}", v
    end
  end

  def self.constant_by_value(v)
    constants.find { |name| const_get(name)==v }
  end

  def self.find_by_id(id)
    if $redis.exists id
      return self.new($redis.hgetall id)
    end
  end

  def self.find_by_ids(ids)
    ids.collect { |id| find_by_id(id) }
  end

  def destroy
    $redis.del self.key
  end
end