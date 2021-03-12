import $axios from "@/services/http"
import { Register, Response, Status, UserInformation } from "@/types/models"

export const UserRepository = {
  fetchInfo(): Promise<Response<UserInformation>> {
    return $axios.get("/api/users/me")
  },

  signup(payload: Register): Promise<Response<Status>> {
    return $axios.post("/api/users", payload)
  }
}
