import $axios from "@/services/http"
import {
  Response,
  Signin,
  UserToken,
  RefreshToken,
  ExchangeCode,
  ExternalAuth,
  AuthenticationRedirect,
  Status
} from "@/types/models"
import { ChangePassword } from "@/types/models/auth/ChangePassword"

export const AuthRepository = {
  signin(payload: Signin): Promise<Response<UserToken>> {
    return $axios.post("/api/auth/token", payload)
  },

  refreshToken(payload: RefreshToken): Promise<Response<UserToken>> {
    return $axios.post("/api/auth/refresh", payload)
  },

  exchangeAuthCode(payload: ExchangeCode): Promise<Response<UserToken>> {
    return $axios.post("/api/auth/code", payload)
  },

  signinWithFacebook(
    payload: ExternalAuth
  ): Promise<Response<AuthenticationRedirect>> {
    return $axios.get(
      `/api/auth/facebook?redirect_url=${
        payload.redirect_url
      }&state=${payload.state || ""}`
    )
  },

  signinWithGoogle(
    payload: ExternalAuth
  ): Promise<Response<AuthenticationRedirect>> {
    return $axios.get(
      `/api/auth/google?redirect_url=${
        payload.redirect_url
      }&state=${payload.state || ""}`
    )
  },

  signinWithMicrosoft(
    payload: ExternalAuth
  ): Promise<Response<AuthenticationRedirect>> {
    return $axios.get(
      `/api/auth/microsoft?redirect_url=${
        payload.redirect_url
      }&state=${payload.state || ""}`
    )
  },

  async changePassword(payload: ChangePassword): Promise<Status> {
    return $axios.patch("/api/auth/password", payload)
  }
}
