import Vue from 'vue'
import BootstrapVue from "bootstrap-vue"
import router from './router'
import App from './App'
import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap-vue/dist/bootstrap-vue.css"
import "./assets/css/chromely.css"

Vue.use(BootstrapVue)

Vue.config.devtools = true
Vue.config.performance = true

new Vue({
  el: '#app',
  router,
  render: h => h(App)
})
