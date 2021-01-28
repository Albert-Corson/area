import React, {useState} from 'react';
import NavigationContainer from "./src/Navigation/NavigationContainer";
import AppLoading from 'expo-app-loading';
import {AppearanceProvider} from 'react-native-appearance';
import loadResources from "./src/Loader/Loader";

const App = () => {
  const [ready, setReady] = useState(false);

  if (!ready) {
    return (
      <AppLoading
        startAsync={loadResources}
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
