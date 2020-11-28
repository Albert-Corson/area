import ResponseModel from './models/ResponseModel'
import { NuxtAxiosInstance } from '@nuxtjs/axios'
import ServiceModel from './models/ServiceModel'

export interface IServiceRepository {
  
  /**
   * List all services available
   */
  listServices(): Promise<ResponseModel<Array<ServiceModel>>>

  /**
   * Get a service by id
   * 
   * @param serviceId service id
   */
  getService(serviceId: number): Promise<ResponseModel<ServiceModel>>

  /**
   * Log out of a service
   *
   * @param serviceId, id of service
   */
  unregisterService(serviceId: number): Promise<ResponseModel>
  
  /**
   * Register to a service, and proceed to login 
   * 
   * @param serviceId service id
   * @param username user name on the given service
   * @param password user password on the given service
   */
  registerService(serviceId: number, username: string, password: string): Promise<ResponseModel>
}

const makeServiceRepository = ($axios: NuxtAxiosInstance): IServiceRepository => ({

  listServices(): Promise<ResponseModel<Array<ServiceModel>>> {
    return new Promise((resolve) => resolve({
      successful: true,
      data: [
        {
          id: 1,
          name: 'lorem'
        },
        {
          id: 2,
          name: 'ipsum'
        }
      ]
    }))
    // return $axios.$get('/services')
  },

  getService(serviceId: number): Promise<ResponseModel<ServiceModel>> {
    return new Promise((resolve) => resolve({
      successful: true,
      data: {
        id: serviceId,
        name: 'lorem'
      }
    }))
    // return $axios.$get(`/services/${serviceId}`)
  },

  unregisterService(serviceId: number): Promise<ResponseModel> {
    return new Promise((resolve) => resolve({
      successful: true
    }))
    // return $axios.$delete(`/services/${serviceId}`)
  },

  registerService(serviceId: number, username: string, password: string): Promise<ResponseModel> {
    return new Promise((resolve) => resolve({
      successful: true
    }))
    // return $axios.$post(`/services/${serviceId}`, { username, password })
  }

})

export default makeServiceRepository