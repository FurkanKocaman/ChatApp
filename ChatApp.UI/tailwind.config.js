/** @type {import('tailwindcss').Config} */
export default {
  darkMode: "selector",
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      fontFamily: {
        "funnel-display": ['"Funnel Display"', "sans-serif"],
        "funnel-sans": ['"Funnel Sans"', "sans-serif"],
        "lexend-deca": ['"Lexend Deca"', "sans-serif"],
        lexend: ['"Lexend"', "sans-serif"],
        outfit: ['"Outfit"', "sans-serif"],
        zain: ['"Zain"', "sans-serif"],
      },
    },
  },
};
