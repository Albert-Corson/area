import $axios from "@/services/http"
import {
  Response,
  Signin,
  UserToken,
  RefreshToken,
  ExchangeCode,
  ExternalAuth
} from "@/types/models"

export const AboutRepository = {
  signin(payload: Signin): Promise<Response<UserToken>> {
    return $axios.post("/api/auth/token", payload)
  },

  refreshToken(payload: RefreshToken): Promise<Response<UserToken>> {
    return $axios.post("/api/auth/refresh", payload)
  },

  exchangeCode(payload: ExchangeCode): Promise<Response<UserToken>> {
    return $axios.post("/api/auth/code", payload)
  },

  signinWithFacebook(payload: ExternalAuth): Promise<Response<UserToken>> {
    throw new Error("not implemented")
    // return $axios.post("/api/auth/facebook", payload)
  },

  signinWithGoogle(payload: ExternalAuth): Promise<Response<UserToken>> {
    throw new Error("not implemented")
    // return $axios.post("/api/auth/google", payload)
  },

  signinWithMicrosoft(payload: ExternalAuth): Promise<Response<UserToken>> {
    throw new Error("not implemented")
    // return $axios.post("/api/auth/microsoft", payload)
  }
}
