import Vue from 'vue'
import Router from 'vue-router'

import App from './App.vue'
import Home from './Home.vue'
import Demo from './Demo.vue'

Vue.use(Router)

const routes =  [
    { path: '/', component: Home },
    { path: '/demo', component: Demo },
    { path: '*', redirect: '/' }
  ]

export default new Router({
  routes
})
