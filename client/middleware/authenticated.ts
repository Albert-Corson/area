import site  from '~/site.json'
import { Middleware } from '@nuxt/types'

interface Route {
  path: string
}

interface RouteConfig {
  authenticated: string
}

enum AuthState {
  Any,
  Yes,
  No
}

const loginPath = '/login'
const homePath = '/'

const getPageConfig = (route: Route): RouteConfig | null => {
  let config: RouteConfig | null = null

  const key: string | undefined = Object.keys(site.pages).find(page => {
    return route.path.match(`^${ page }$`)
  })

  if (key) {
    // @ts-ignore
    config = site.pages[key]
  }
  return config
}

const getAuthRequirement = (route: Route): AuthState => {
  const config = getPageConfig(route)

  switch (config?.authenticated) {
    case 'yes':
      return AuthState.Yes
    case 'no':
      return AuthState.No
    default:
      return AuthState.Any
  }
}

const authenticated: Middleware = ({ route, store, redirect }) => {
    const authRequirement = getAuthRequirement(route)
    const isAuthenticated = store.getters['auth/authenticated'] ?? false

    switch (authRequirement) {
      case AuthState.Yes:
        if (!isAuthenticated) {
          return redirect(loginPath)
        }
      break
      case AuthState.No:
        if (isAuthenticated) {
          return redirect(homePath)
        }
      break
    }
}

export default authenticated
