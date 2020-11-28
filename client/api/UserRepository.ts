import { NuxtAxiosInstance } from '@nuxtjs/axios'
import UserModel from './models/UserModel'
import ResponseModel from './models/ResponseModel'

export interface IUserRepository {
  
  /**
   * Create a user
   *
   * @param username
   * @param password 
   * @param email
   */
  createUser(username: string, password: string, email: string): Promise<ResponseModel>
  
  /**
   * Delete user by id
   *
   * @param userId 
   */
  deleteUser(userId: number): Promise<ResponseModel>
  
  /**
   * Get user info by id
   *
   * @param userId
   */
  getUser(userId: number): Promise<ResponseModel<UserModel>>
}

const makeUserRepository = ($axios: NuxtAxiosInstance): IUserRepository => ({
  
  createUser(username: string, password: string, email: string): Promise<ResponseModel> {
    return new Promise((resolve) => resolve({
      successful: true
    }))
    // return $axios.$post('/users', { username, password, email })
  },

  deleteUser(userId: number): Promise<ResponseModel> {
    return new Promise((resolve) => resolve({
      successful: true
    }))
    // return $axios.$delete(`/users/${userId}`)
  },

  getUser(userId: number): Promise<ResponseModel<UserModel>> {
    return new Promise((resolve) => resolve({
      successful: true,
      data: {
        id: userId,
        username: 'lorem',
        email: 'lorem@ipsum.dolor'
      }
    }))
    // return $axios.$get(`/users/${userId}`)
  }

})

export default makeUserRepository