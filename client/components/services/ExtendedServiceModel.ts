
import ServiceModel from "@/api/models/ServiceModel"

export default interface ExtendedServiceModel extends ServiceModel
{
    registered ?: boolean
    empty ?: boolean
}