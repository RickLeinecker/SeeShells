'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

//module.exports = merge(prodEnv, {
//  NODE_ENV: '"development"'
//})
module.exports = {
    PublicPath: process.env.NODE_ENV === 'production'
        ? '/SeeShells/'
        : '/'
}
