import site from '~/site.json'
import { Middleware } from '@nuxt/types'

enum AuthState {
  Any,
  Yes,
  No
}

const loginPath = '/login'
const homePath = '/'

const getAuthRequirement = (route: object): AuthState => {
  // @ts-ignore
  const config = site.pages[route.path]
  if (!config || config.authenticated === 'any') {
    return AuthState.Any
  }
  return config.authenticated === 'yes'
    ? AuthState.Yes
    : AuthState.No
}

const authenticated: Middleware = ({ route, store, redirect }) => {
    const authRequirement = getAuthRequirement(route)
    const isAuthenticated = store.getters['auth/authenticated'] || false

    // @ts-ignore
/*    switch (authRequirement) {
      case AuthState.Yes:
        if (!isAuthenticated) {
          return redirect(loginPath)
        }
        break;
      case AuthState.No:
        if (isAuthenticated) {
          return redirect(homePath)
        }
        break
    }
    */
}

export default authenticated
