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