#encoding: utf-8
require 'base_class'

class Setting<CZ::BaseClass
  attr_accessor :ios_app_version, :ios_app_update_is_option

  def self.find
    if $redis.exists(key)
      cache=self.new($redis.hgetall key)
      return cache
    end
  end

  def ios_app_version_is_old version
    (version||default_app_version)<self.ios_app_version
  end


  def self.key
    'epm:admin:setting'
  end

  def save
    $redis.hmset Setting.key, 'ios_app_version', self.ios_app_version, 'ios_app_update_is_option', self.ios_app_update_is_option
  end

  def default_app_version
    '0.1'
  end

end