import {loadAsync} from 'expo-font';

interface Font {
  [name: string]: number
}

const fonts: Array<Font> = [
  {Dosis: require('../../assets/fonts/Dosis-Regular.otf')},
  {DosisLight: require('../../assets/fonts/Dosis-Light.otf')},
  {DosisMedium: require('../../assets/fonts/Dosis-Medium.otf')},
  {DosisSemiBold: require('../../assets/fonts/Dosis-SemiBold.otf')},
  {DosisBold: require('../../assets/fonts/Dosis-Bold.otf')},
  {DosisExtraBold: require('../../assets/fonts/Dosis-ExtraBold.otf')},
];

const loadFonts = async (): Promise<void> => {
  for (const font of fonts) {
    await loadAsync(font);
  }
};

export {loadFonts};
