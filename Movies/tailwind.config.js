module.exports = {
    content: ["./Views/**/*.cshtml",
        "./wwwroot/js/*.js"     ],
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {},
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
        require('daisyui')
    ],
}
