module.exports = {
  content: [
    "./App.{js,jsx,ts,tsx}",
    "./app/**/*.{js,jsx,ts,tsx}",
    "./components/**/*.{js,jsx,ts,tsx}",
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  presets: [require('nativewind/preset')],
  theme: {
    extend: {
      colors: {
        vault: {
          background: '#060A12',
          surface: '#0E1624',
          surfaceAlt: '#141F31',
          border: '#223048',
          text: '#EEF3FF',
          muted: '#97A3B6',
          accent: '#59E0A7',
          accentSoft: '#113725',
          danger: '#F36B6B',
          warning: '#F4C95D',
        },
      },
    },
  },
  plugins: [],
};