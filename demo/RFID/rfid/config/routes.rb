Rails.application.routes.draw do
  use_doorkeeper
  resources :files

  resources :pick_orders do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :pick_items do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :picks do
    collection do
      get :search
      get :exports
      match :import, action: :import, via: [:get, :post]
    end

    member do
      get :pick_items
      get :pick_end_items
    end
  end

  resources :order_items do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :orders do
    collection do
      get :search
      get :exports
      match :import, action: :import, via: [:get, :post]
    end

    member do
      get :order_items
    end
  end

  resources :order_box_types do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :order_boxes do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :order_cars do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :part_positions do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :parts do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :units do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :unit_groups do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :colors do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :part_types do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :movements do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :move_types do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :storages do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :positions do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :warehouses do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :locations do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  devise_for :users, path: "auth", path_names: { sign_in: 'login',
                                                 sign_out: 'logout',
                                                 password: 'secret',
                                                 confirmation: 'verification',
                                                 unlock: 'unblock',
                                                 registration: 'register',
                                                 sign_up: 'cmon_let_me_in' }

  devise_scope :user do
    get "sign_in", to: "devise/sessions#new"
  end

  resources :users do
    collection do
      get :search
      match :import, action: :import, via: [:get, :post]
    end
  end

  resources :settings
  # api routes
  mount ApplicationAPI => '/api'

  root 'welcome#index'

  # The priority is based upon order of creation: first created -> highest priority.
  # See how all your routes lay out with "rake routes".

  # You can have the root of your site routed with "root"
  # root 'welcome#index'

  # Example of regular route:
  #   get 'products/:id' => 'catalog#view'

  # Example of named route that can be invoked with purchase_url(id: product.id)
  #   get 'products/:id/purchase' => 'catalog#purchase', as: :purchase

  # Example resource route (maps HTTP verbs to controller actions automatically):
  #   resources :products

  # Example resource route with options:
  #   resources :products do
  #     member do
  #       get 'short'
  #       post 'toggle'
  #     end
  #
  #     collection do
  #       get 'sold'
  #     end
  #   end

  # Example resource route with sub-resources:
  #   resources :products do
  #     resources :comments, :sales
  #     resource :seller
  #   end

  # Example resource route with more complex sub-resources:
  #   resources :products do
  #     resources :comments
  #     resources :sales do
  #       get 'recent', on: :collection
  #     end
  #   end

  # Example resource route with concerns:
  #   concern :toggleable do
  #     post 'toggle'
  #   end
  #   resources :posts, concerns: :toggleable
  #   resources :photos, concerns: :toggleable

  # Example resource route within a namespace:
  #   namespace :admin do
  #     # Directs /admin/products/* to Admin::ProductsController
  #     # (app/controllers/admin/products_controller.rb)
  #     resources :products
  #   end
end
