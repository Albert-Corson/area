import {loadAsync} from 'expo-font'

interface Font {
  [name: string]: number
}

const fontsPath = '../../assets/fonts'

const fonts: Array<Font> = [
  // Dosis
  {Dosis: require(`${fontsPath}/Dosis/Dosis-Regular.otf`)},
  {DosisLight: require(`${fontsPath}/Dosis/Dosis-Light.otf`)},
  {DosisMedium: require(`${fontsPath}/Dosis/Dosis-Medium.otf`)},
  {DosisSemiBold: require(`${fontsPath}/Dosis/Dosis-SemiBold.otf`)},
  {DosisBold: require(`${fontsPath}/Dosis/Dosis-Bold.otf`)},
  {DosisExtraBold: require(`${fontsPath}/Dosis/Dosis-ExtraBold.otf`)},
  // Louis George Cafe
  {LouisGeorgeCafe: require(`${fontsPath}/LouisGeorgeCafe/LouisGeorgeCafe-Normal.otf`)},
  {LouisGeorgeCafeLight: require(`${fontsPath}/LouisGeorgeCafe/LouisGeorgeCafe-Light.otf`)},
  {LouisGeorgeCafeBold: require(`${fontsPath}/LouisGeorgeCafe/LouisGeorgeCafe-Bold.otf`)},
]

const loadFonts = async (): Promise<void> => {
  await Promise.all(fonts.map(font => loadAsync(font)))
}

export {loadFonts}
