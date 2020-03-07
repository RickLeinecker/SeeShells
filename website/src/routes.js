import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';

const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register }
];

export default routes;