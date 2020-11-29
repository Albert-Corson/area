import ResponseModel from './models/ResponseModel'
import { NuxtAxiosInstance } from '@nuxtjs/axios'
import ServiceModel from './models/ServiceModel'
import dbConnector from '~/tmp/dbConnector'

export interface IServiceRepository {
  
  /**
   * List all services available
   */
  listServices(): Promise<ResponseModel<Array<ServiceModel>>>

  /**
   * List all services the currently logged in user is regestered to
   */
  listRegisteredServices(): Promise<ResponseModel<Array<ServiceModel>>>

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
    return new Promise((resolve, reject) => {
      try {
        const services = dbConnector.listServices()
        return resolve({
          successful: true,
          data: services
        })
      } catch (e) {
        return reject({
          successful: false,
          error: e.message
        })
      }
    })
    // return $axios.$get('/services')
  },

  listRegisteredServices(): Promise<ResponseModel<Array<ServiceModel>>> {
    return new Promise((resolve, reject) => {
      try {
        const services = dbConnector.listRegisteredServices()
        return resolve({
          successful: true,
          data: services
        })
      } catch (e) {
        return reject({
          successful: false,
          error: e.message
        })
      }
    })
    // return $axios.$get('/services/me')
  },

  getService(serviceId: number): Promise<ResponseModel<ServiceModel>> {
    return new Promise((resolve, reject) => {
      try {
        const service = dbConnector.getService(serviceId)
        return resolve({
          successful: true,
          data: service
        })
      } catch (e) {
        return reject({
          successful: false,
          error: e.message
        })
      }
    })
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
    // return $axios.$put(`/services/${serviceId}`, { username, password })
  }

})

export default makeServiceRepository
