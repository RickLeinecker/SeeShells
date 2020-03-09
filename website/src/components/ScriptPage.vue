<template>
    <div id="scripts" align="left">
        <h1>Adding and Modifying scripts for the WPF application</h1>
        <ExampleScript/>
    </div>
</template>

<script>
    import ExampleScript from './ExampleScript.vue';

    export default {
        name: 'Scripts',
        components: { ExampleScript },
    }
</script>

<style>
    #scripts {
        margin:50px;
    }
</style>