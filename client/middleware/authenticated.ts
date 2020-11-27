import { Middleware } from '@nuxt/types'

const authenticated: Middleware = ({ route, store, redirect }) => {
    const loginPath = '/login'
    if (route.fullPath === loginPath) {
        return
    }
    if (!store.getters['auth/authenticated']) {
        return redirect(loginPath)
    }
}

export default authenticated
