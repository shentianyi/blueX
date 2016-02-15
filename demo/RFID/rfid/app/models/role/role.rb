#encoding: utf-8
require 'base_class'
class Role
  RoleMethods=[:admin?, :manager?, :director?, :user?]
  @@roles={
      :'400' => {:name => 'admin', :display => (I18n.t 'manage.user.role.admin')},
      :'300' => {:name => 'director', :display => (I18n.t 'manage.user.role.director')},
      :'200'=>{:name=>'manager',:display=>(I18n.t 'manage.user.role.manager')},
      :'100' => {:name => 'user', :display => (I18n.t 'manage.user.role.user')}
  }

  class<<self
    RoleMethods.each do |m|
      define_method(m) { |id|
        @@roles[id_sym(id)][:name]==m.to_s.sub(/\?/, '')
      }
    end
    @@roles.each do |key,value|
      define_method(value[:name]){
        key.to_s.to_i
      }
    end
  end

  def self.display id
    I18n.t 'manage.user.role.'+@@roles[id_sym(id)][:name]
  end

  def self.id_sym id
    id.to_s.to_sym
  end

  def self.menu
    roles = []
    @@roles.each { |key, value|
      roles <<[value[:display], key.to_s]
    }
    roles
  end
end