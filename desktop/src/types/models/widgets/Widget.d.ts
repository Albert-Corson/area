import Service from "@/types/models"
import Param from "./Param"

export interface Widget {
  id: int
  name: string
  description: string
  requires_auth: boolean
  frequency: int
  service: Service
  params: Array<Param>
}
