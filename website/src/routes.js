import Home from './components/Home.vue';
import Register from './components/RegisterForm.vue';
import Login from './components/LoginForm.vue';
import Scripts from './components/ScriptPage.vue';
import ApproveUsers from './components/ApproveUserPage.vue';
import GUIDPage from './components/GUIDPage.vue';
import OSPage from './components/OSPage.vue';

const root = '/SeeShells/'

const routes = [
    { path: root, component: Home },
    { path: root + 'register', component: Register },
    { path: root + 'login', component: Login },
    { path: root + 'scripts', component: Scripts },
    { path: root + 'approveusers', component: ApproveUsers },
    { path: root + 'guids', component: GUIDPage },
    { path: root + 'os', component: OSPage }
];

export default routes;