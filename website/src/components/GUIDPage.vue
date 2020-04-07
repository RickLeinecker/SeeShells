<template>
    <div id="guidHeader">
        <h1>Add or Modify GUID Values</h1>
        <NewGUID />
        <div id="guidContent">
            <GUIDList />
        </div>
    </div>
</template>

<script>
    import GUIDList from './ViewGUIDs.vue';
    import NewGUID from './NewGUID.vue';

    export default {
        name: 'GUIDPage',
        components: { GUIDList, NewGUID },
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

<style scoped>
    #guidHeader {
        margin: 50px;
    }

    #guidContent {
        margin: auto;
        height: 100%;
        width: 70%;
    }

</style>