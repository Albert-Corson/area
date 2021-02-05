import React, {useContext} from 'react';
import {loadFonts} from './Fonts';

const loadResources = async (): Promise<void> => {
  await loadFonts();
};

export default loadResources;
