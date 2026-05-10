import Constants from 'expo-constants';
import { Platform } from 'react-native';

// Minimal and explicit API base selection.
const extra = Constants?.expoConfig?.extra ?? {};
const env = process.env;
const PORT = extra.apiPort ?? env.EXPO_PUBLIC_API_PORT ?? 65243;

const getWeb = () => extra.apiBaseUrlWeb || env.EXPO_PUBLIC_API_BASE_URL_WEB || `http://localhost:${PORT}/api`;
const getAndroid = () => extra.apiBaseUrlAndroid || env.EXPO_PUBLIC_API_BASE_URL_ANDROID || `http://10.0.2.2:${PORT}/api`;
const getIos = () => extra.apiBaseUrlIos || env.EXPO_PUBLIC_API_BASE_URL_IOS || extra.apiBaseUrl || env.EXPO_PUBLIC_API_BASE_URL || `http://localhost:${PORT}/api`;

const base = Platform.OS === 'web' ? getWeb() : Platform.OS === 'android' ? getAndroid() : getIos();

export const API_BASE_URL = String(base).replace(/\/+$/, '');