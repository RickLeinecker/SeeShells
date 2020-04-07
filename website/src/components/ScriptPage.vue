<template>
    <div id="scriptsHeader">
        <h1>Adding and Modifying Scripts for the WPF application</h1>
        <br />
        <div id="scriptsContent">
            <CurrentScript />
            <br />
            <ExampleScript />
        </div>
    </div>
</template>

<script>
    import ExampleScript from './ExampleScript.vue';
    import CurrentScript from './GetScript.vue';

    export default {
        name: 'Scripts',
        components: { ExampleScript, CurrentScript },
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
    #scriptsHeader {
        margin:50px;
    }
    #scriptsContent {
        margin: auto;
        height: 100%;
        width: 60%;  
    }
</style>