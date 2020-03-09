import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';
import TeamMembers from './components/TeamMembers.vue';
import About from './components/About.vue';

const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register },
    { path: root + 'team', component: TeamMembers },
    { path: root + 'about', component: About}
];

export default routes;