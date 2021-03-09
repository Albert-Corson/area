import { NuxtAxiosInstance } from '@nuxtjs/axios'
import AuthTokenModel from './models/AuthTokenModel'
import ResponseModel from './models/ResponseModel'

export interface IAuthRepository {

  /**
   * Login to the API and get the access and refresh tokens
   *
   * @param username user name
   * @param password user password
   */
  getToken(username: string, password: string): Promise<ResponseModel<AuthTokenModel>>

  /**
   * Refresh the access token
   *
   * @param refresh_token  of user
   */
  refreshToken(refresh_token: string): Promise<ResponseModel<AuthTokenModel>>

  /**
   * Revoke the access token and refresh token
   */
  revokeToken(): Promise<ResponseModel>
}

const makeAuthRepository = ($axios: NuxtAxiosInstance): IAuthRepository => ({

  getToken(username: string, password: string): Promise<ResponseModel<AuthTokenModel>> {
    return $axios.$post('/auth/token', { username, password })
  },

  refreshToken(refresh_token: string): Promise<ResponseModel<AuthTokenModel>>{
    return $axios.$post('/auth/refresh', { refresh_token })
  },

  revokeToken(): Promise<ResponseModel>{
    return $axios.$delete('/auth/revoke')
  }

})


export default makeAuthRepository
