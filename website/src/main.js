import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VueRouter from 'vue-router'
import VueSession from 'vue-session'
import App from './App.vue'
import routes from './routes';

Vue.config.productionTip = false
Vue.prototype.$baseurl = (Vue.config.productionTip) ? 'https://seeshells.herokuapp.com/' : 'http://localhost:3000/'

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)
Vue.use(VueRouter)
Vue.use(VueSession)

const router = new VueRouter({ mode: 'history', routes });

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');