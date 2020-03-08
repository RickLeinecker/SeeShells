import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';
import TeamMembersVue from './components/TeamMembers.vue';

const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register },
    { path: root + 'team', component: TeamMembersVue}
];

export default routes;