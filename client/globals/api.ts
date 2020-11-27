import { AuthRepository } from '~/api/auth'

interface Api {
  auth: AuthRepository
}

let $api: Api

export function initializeApi(apiInstance: Api) {
  $api = apiInstance
}

export { $api }
