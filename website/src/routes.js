import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';
import TeamMembers from './components/TeamMembers.vue';
import About from './components/About.vue';
import Login from './components/LoginForm.vue';


const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register },
    { path: root + 'team', component: TeamMembers },
    { path: root + 'about', component: About},
    { path: root + 'login', component: Login }
];

export default routes;