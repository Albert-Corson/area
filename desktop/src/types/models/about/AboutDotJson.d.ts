export interface AboutDotJsonWidget {
  id: int
  name: string
  description: string
  requires_auth: boolean
}

export interface AboutDotJsonService {
  id: int
  name: string
  widgets: Array<AboutDotJsonWidget>
}

export interface AboutDotJson {
  services: Array<AboutDotJsonService>
}
