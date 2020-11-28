import { NuxtAxiosInstance } from '@nuxtjs/axios'
import WidgetModel from './models/WidgetModel'
import ResponseModel from './models/ResponseModel'

export interface IWidgetRepository {

  /**
   * List all widgets available
   * 
   * To list widgets of a specific service, provide the serviceId parameter
   *
   * @param serviceId id of service to list widgets of
   */
  listWidgets(serviceId?: number): Promise<ResponseModel<Array<WidgetModel>>>
  
  /**
   * Get a widget by id
   * 
   * @param widgetId widget id
   */
  getWidget(widgetId: number): Promise<ResponseModel<WidgetModel>>

  /**
   * Register the user to a widget
   * 
   * @param widgetId widget id
   */
  registerWidget(widgetId: number): Promise<ResponseModel>

  /**
   * Unregister the user from a widget
   * 
   * @param widgetId widget id
   */
  unregisterWidget(widgetId: number): Promise<ResponseModel>
}

const makeWidgetRepository = ($axios: NuxtAxiosInstance): IWidgetRepository => ({

  listWidgets(serviceId?: number): Promise<ResponseModel<Array<WidgetModel>>> {
    return new Promise((resolve) => resolve({
      successful: true,
      data: [
        {
          id: 1,
          name: 'ipsum',
          parent_service: {
            id: 1,
            name: 'lorem'
          }
        },
        {
          id: 2,
          name: 'dolor',
          parent_service: {
            id: 1,
            name: 'lorem'
          }
        }
      ]
    }))
    /*
    if (serviceId) {
      return $axios.$get(`/widgets?serviceId=${serviceId}`)
    } else {
      return $axios.$get(`widgets`)
    }
    */
  },

  getWidget(widgetId: number): Promise<ResponseModel<WidgetModel>> {
    return new Promise((resolve) => resolve({
      successful: true,
      data: {
        id: widgetId,
        name: 'lorem',
        parent_service: {
          id: 1,
          name: 'lorem'
        }
      }
    }))
    // return $axios.$get(`/widgets/${widgetId}`)
  },

  registerWidget(): Promise<ResponseModel> {
    return new Promise((resolve) => resolve({
      successful: true
    }))
    // return $axios.$post('/widgets', { username, password, email })
  },

  unregisterWidget(widgetId: number): Promise<ResponseModel> {
    return new Promise((resolve) => resolve({
      successful: true
    }))
    // return $axios.$delete(`/widgets/${widgetId}`)
  }

})

export default makeWidgetRepository