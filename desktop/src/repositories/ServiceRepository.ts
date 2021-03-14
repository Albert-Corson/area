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
    return $axios.get(
      `/api/services/auth/${serviceId}?redirect_url=${payload.redirect_url}&state=${payload.state}`
    )
  },

  signoutFromService(serviceId: number): Promise<Status> {
    return $axios.delete(`/api/services/auth/${serviceId}`)
  }
}
