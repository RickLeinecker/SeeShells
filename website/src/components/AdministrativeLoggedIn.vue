<template>
    <div id='items'>
        <b-dropdown-item>Approve New Admins</b-dropdown-item>
        <b-dropdown-item>Add New Script</b-dropdown-item>
        <b-dropdown-item v-on:click="Logout">Logout</b-dropdown-item>
    </div>
</template>

<script>

    export default {
        name: 'LoggedIn',
        methods: {
            Logout() {
                this.$session.destroy();
                var baseurl = 'https://seeshells.herokuapp.com/'; //http://localhost:3000/
                var url = baseurl + 'logout';
                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send();
                }
                catch (err) { 
                        alert(err.message)
                }

                location.reload();
            }
        }
    }
</script>