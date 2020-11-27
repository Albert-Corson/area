import { NuxtAxiosInstance } from '@nuxtjs/axios'
import AuthToken from './models/AuthToken'
import RequestStatus from './models/RequestStatus'

export interface AuthRepository {
  login(username: string, password: string): Promise<RequestStatus<AuthToken>>
}

export default ($axios: NuxtAxiosInstance): AuthRepository => ({
  login(username: string, password: string): Promise<RequestStatus<AuthToken>> {
    return new Promise((resolve) => resolve(new RequestStatus(AuthToken, JSON.stringify({
      successful: true,
      data: JSON.stringify({
        access_token: 'fake_access_token',
        refresh_token: 'fake_refresh_token',
        expires_in: 0
      })
    }))))
    // return $axios.$post('/auth/token', {username, password})
  }
})
