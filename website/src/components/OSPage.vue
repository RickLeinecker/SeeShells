<template>
    <div id="osHeader">
        <h1>Add a new Windows OS Version</h1>
        <NewOS />
        <br />
        <div id="osContent">
            <ViewOSandFiles />
        </div>
    </div>
</template>

<script>
    import NewOS from './NewOS.vue';
    import ViewOSandFiles from './ViewOSandFiles.vue';

    export default {
        name: 'OSPage',
        components: { NewOS, ViewOSandFiles },
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
        },
    }
</script>

<style scoped>
    #osHeader {
        margin: 50px;
    }

    #osContent {
        margin: auto;
        height: 100%;
        width: 70%;
    }
</style>