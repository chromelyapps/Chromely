'use strict'

const HtmlWebpackPlugin = require('html-webpack-plugin')
const path = require('path')

const utils = {
  resolve: function (dir) {
    return path.join(__dirname, '..', dir)
  },

  assetsPath: function (_path) {
    const assetsSubDirectory = 'static'
    return path.posix.join(assetsSubDirectory, _path)
  }
}

module.exports = {
  resolve: {
    extensions: ['.js', '.vue', '.json'],
  },

  module: {
    rules: [
        {
          test: /\.vue$/,
          loader: 'vue-loader'
        }, {
          test: /\.css$/,
          loaders: [ 'style-loader', 'css-loader' ]
        }, 
       {
        test: /\.js$/,
        loader: 'babel-loader',
        query: {
          compact: 'false'
        }
      }, {
        test: /\.(png|jpe?g|gif|svg)(\?.*)?$/,
        loader: 'url-loader',
        options: {
          limit: 10000,
          name: utils.assetsPath('img/[name].[hash:7].[ext]')
        }
      }, {
        test: /\.(mp4|webm|ogg|mp3|wav|flac|aac)(\?.*)?$/,
        loader: 'url-loader',
        options: {
          limit: 10000,
          name: utils.assetsPath('media/[name].[hash:7].[ext]')
        }
      }, {
        test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
        loader: 'url-loader',
        options: {
          limit: 10000,
          name: utils.assetsPath('fonts/[name].[hash:7].[ext]')
        }
      }
    ]
  },

  plugins: [
    new HtmlWebpackPlugin({
      template: "src/index.html",
      filename: "index.html",
      inject: true
    })
  ]
}
