import React, {useContext} from 'react'
import {loadFonts} from './Fonts'
import {loadImages} from './Images'

const loadResources = async (): Promise<void> => {
  await Promise.all([
    loadFonts(),
    loadImages()
  ])
}

export default loadResources
