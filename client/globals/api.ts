import { IAuthRepository } from '~/api/AuthRepository'
import { IServiceRepository } from '~/api/ServiceRepository'
import { IUserRepository } from '~/api/UserRepository'
import { IWidgetRepository } from '~/api/WidgetRepository'

interface Api {
  auth: IAuthRepository,
  user: IUserRepository,
  service: IServiceRepository,
  widget: IWidgetRepository
}

let $api: Api

export function initializeApi(apiInstance: Api) {
  $api = apiInstance
}

export { $api }
