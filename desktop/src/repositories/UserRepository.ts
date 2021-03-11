import $axios from "@/services/http"
import { Register, Response, Status } from "@/types/models"

export const UserRepository = {
  signup(payload: Register): Promise<Response<Status>> {
    console.log("signup -> repository")
    return $axios.post("/api/users", payload)
  }
}
