import $axios from "@/services/http"
import {
  Response,
  Service,
  ExternalAuth,
  Status,
  AuthenticationRedirect
} from "@/types/models"

export const ServiceRepository = {
  listServices(): Promise<Response<Array<Service>>> {
    return $axios.get("/api/services")
  },

  listMyServices(): Promise<Response<Array<Service>>> {
    return $axios.get("/api/services/me")
  },

  getService(serviceId: number): Promise<Response<Service>> {
    return $axios.get(`/api/services/${serviceId}`)
  },

  signinToService(
    serviceId: number,
    payload: ExternalAuth
  ): Promise<Response<AuthenticationRedirect>> {
    let url = `/api/services/auth/${serviceId}?redirect_url=${encodeURIComponent(
      payload.redirect_url
    )}`
    if (payload.state) {
      url += `&state=${encodeURIComponent(payload.state)}`
    }
    return $axios.get(url)
  },

  signoutFromService(serviceId: number): Promise<Status> {
    return $axios.delete(`/api/services/auth/${serviceId}`)
  }
}
