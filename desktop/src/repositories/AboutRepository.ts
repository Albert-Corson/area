import $axios from "@/services/http"
import { Response, AboutDotJson } from "@/types/models"

export const AboutRepository = {
  fetch(): Promise<Response<AboutDotJson>> {
    return $axios.get("/api/about")
  }
}
