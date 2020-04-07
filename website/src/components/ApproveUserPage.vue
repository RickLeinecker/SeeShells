<template>
    <div id="userHeader">
        <h1>Add or Reject New Users</h1>
        <br />
        <div id="userContent">
            <NewUsers />
        </div>
    </div>
</template>

<script>
    import NewUsers from './NewUsers.vue';

    export default {
        name: 'ApproveUsers',
        components: { NewUsers },
        beforeMount() {
            var url = this.$baseurl + 'SessionIsActive';

            var xhr = new XMLHttpRequest();
            xhr.open("GET", url, false);
            xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
            xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

            try {
                xhr.send(null);
                var result = JSON.parse(xhr.responseText);

                if (result.success != 1) {
                        this.$session.destroy();
                        this.$router.push('/SeeShells/login');
                        location.reload();
                }
            }
            catch (err) {
                console.info(err);
            }
        }
    }
</script>

<style>
    #userHeader {
        margin: 50px;
    }

    #userContent {
        margin: auto;
        height: 100%;
        width: 60%;
    }
</style>