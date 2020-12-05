import { NuxtAxiosInstance } from '@nuxtjs/axios'
import UserModel from './models/UserModel'
import ResponseModel from './models/ResponseModel'

export interface IUserRepository {

  /**
   * Create a user
   *
   * @param username user name
   * @param password user password
   * @param email user email
   */
  createUser(username: string, password: string, email: string): Promise<ResponseModel>
  
  /**
   * Delete user by id
   *
   * @param userId user id
   */
  deleteUser(userId: number): Promise<ResponseModel>

  /**
   * Get user info by id
   * 
   * To get the currently logged in user's info, don't provide the userId
   *
   * @param userId user id
   */
  getUser(userId?: number): Promise<ResponseModel<UserModel>>
}

const makeUserRepository = ($axios: NuxtAxiosInstance): IUserRepository => ({

  createUser(username: string, password: string, email: string): Promise<ResponseModel> {
    return $axios.$post('/users', { username, password, email })
  },

  deleteUser(userId: number): Promise<ResponseModel> {
    return $axios.$delete(`/users/${userId}`)
  },

  getUser(userId?: number): Promise<ResponseModel<UserModel>> {
    if (userId === undefined) {
      return $axios.$get(`/users/me`)
    } else {
      return $axios.$get(`/users/${userId}`)
    }
  }

})

export default makeUserRepository
