<template>
    <div id="nav">
        <b-navbar toggleable="lg" type="dark" variant="dark">
            <img alt="Vue logo" src="./../assets/logo.png" width="75" height="75">
            <b-navbar-brand href="#">SeeShells</b-navbar-brand>

            <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

            <b-collapse id="nav-collapse" is-nav>
                <b-navbar-nav>
                    <b-nav-item href="#">About</b-nav-item>
                    <b-nav-item href="#">Download the Program</b-nav-item>
                    <b-nav-item href="#">Developer Team</b-nav-item>
                    <b-nav-item href="https://github.com/RickLeinecker/SeeShells">GitHub Page</b-nav-item>
                </b-navbar-nav>

                <!-- Right aligned nav items -->
                <b-navbar-nav class="ml-auto">

                    <b-nav-item-dropdown right>
                        <!-- Using 'button-content' slot -->
                        <template v-slot:button-content>
                            <em>Administrative</em>
                        </template>
                        <b-dropdown-item href="#">Register</b-dropdown-item>
                        <b-dropdown-item href="#">Login</b-dropdown-item>
                    </b-nav-item-dropdown>
                </b-navbar-nav>
            </b-collapse>
        </b-navbar>
    </div>
</template>

<script>
    export default {
        name: 'NavigationBar'
    }
</script>

<style scoped>
    #nav{
          margin-left: 0px;
          margin-right: 0px;
    }
</style>
