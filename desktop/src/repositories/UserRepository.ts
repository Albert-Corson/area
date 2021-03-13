import $axios from "@/services/http"
import {
  Register,
  Response,
  Status,
  UserInformation,
  UserDevices
} from "@/types/models"

export const UserRepository = {
  fetchInfo(): Promise<Response<UserInformation>> {
    return $axios.get("/api/users/me")
  },

  signup(payload: Register): Promise<Response<Status>> {
    return $axios.post("/api/users", payload)
  },

  deleteAccount(): Promise<Response> {
    return $axios.delete("/api/users/me")
  },

  listDevices(): Promise<Response<UserDevices>> {
    return $axios.get("/api/users/me/devices")
  },

  forgetDevice(deviceId: number): Promise<Response> {
    return $axios.delete(`/api/users/me/devices/${deviceId}`)
  }
}
