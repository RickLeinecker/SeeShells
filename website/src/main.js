import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
import VueRouter from 'vue-router'
import App from './App.vue'
import routes from './routes';

Vue.config.productionTip = false

Vue.use(BootstrapVue)
Vue.use(IconsPlugin)

Vue.use(VueRouter)

const router = new VueRouter({ mode: 'history', routes });

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
