import {Asset} from 'expo-asset'

const assets: number[] = [
  require('../../assets/avatar/preview.png')
]

const loadImages = async (): Promise<void> => {
  await Promise.all(assets.map(image => Asset.loadAsync(image)))
}

export {loadImages}
