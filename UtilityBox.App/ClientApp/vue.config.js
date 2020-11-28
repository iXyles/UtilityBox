module.exports = {
  "transpileDependencies": [
    "vuetify"
  ],
  "configureWebpack": {
    target: 'electron-renderer',
  },
  pluginOptions: {
    electronBuilder: {
      nodeIntegration: true
    }
  }
}