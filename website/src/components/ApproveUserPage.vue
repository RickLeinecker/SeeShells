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