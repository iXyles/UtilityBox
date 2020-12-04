module.exports = {
    "transpileDependencies": [
        "vuetify"
    ],
    devServer: {
        proxy: {
            '/dotnetify': {
                target: 'http://localhost:47220'
            }
        }
    }
};