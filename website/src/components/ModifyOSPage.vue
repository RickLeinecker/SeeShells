<template>
    <div id="osHeader">
        <h1>Modify an OS Version or group of files</h1>

        <br />
        <div id="osContent">
            <ViewOSandFiles />
        </div>
    </div>
</template>

<script>

    import ViewOSandFiles from './ViewOSandFiles.vue';

    export default {
        name: 'ModifyOSPage',
        components: { ViewOSandFiles },
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