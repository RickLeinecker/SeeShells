import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';
import Login from './components/LoginForm.vue';

const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register },
    { path: root + 'login', component: Login }
];

export default routes;