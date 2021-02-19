import React, { useState } from 'react';
import NavigationContainer from "./src/Navigation/NavigationContainer";
import AppLoading from 'expo-app-loading';
import { AppearanceProvider } from 'react-native-appearance';
import loadResources from "./src/Loader/Loader";
import * as ScreenOrientation from 'expo-screen-orientation';

const App = () => {
  const [ready, setReady] = useState(false);



  if (!ready) {
    return (
      <AppLoading
        startAsync={async () => {
          await Promise.all([
            loadResources(),
            ScreenOrientation.lockAsync(ScreenOrientation.OrientationLock.PORTRAIT_UP),
          ]);
        }}
        onFinish={() => setReady(true)}
        onError={console.warn}
      />
    );
  }

  return (
    <AppearanceProvider>
      <NavigationContainer />
    </AppearanceProvider>
  );
};

export default App;
