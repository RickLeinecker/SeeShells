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