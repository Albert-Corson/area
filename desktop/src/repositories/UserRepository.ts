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

  signup(payload: Register): Promise<Status> {
    return $axios.post("/api/users", payload)
  },

  deleteAccount(): Promise<Status> {
    return $axios.delete("/api/users/me")
  },

  listDevices(): Promise<Response<UserDevices>> {
    return $axios.get("/api/users/me/devices")
  },

  forgetDevice(deviceId: number): Promise<Status> {
    return $axios.delete(`/api/users/me/devices/${deviceId}`)
  }
}
