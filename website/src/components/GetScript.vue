<template>
    <div id="current">
        <div id="container">
            <div id="leftContent">Enter shell item identifier:</div>
            <div id="middleContent"><b-form-input v-model="identifier" type="number" @change="onChange" placeholder="113"></b-form-input></div>
            <div id="rightContent">Hex equivalent: {{hexIdentifier}}</div>
        </div>
    </div>
</template>

<script>

    export default {
        name: 'CurrentScript',
        data() {
            return {
                identifier: '',
                hexIdentifier: '0x71'
            }
        },

        methods: {
            onChange() {
                this.hexIdentifier = '0x' + (Number(this.identifier).toString(16)).toUpperCase();
                
          }
        }
    }
</script>

<style scoped>
    #container {
        height: 100%; 
        width:100%; 
        font-size: 0;

    }
    #leftContent, #middleContent, #rightContent {
        display: inline-block; 
        *display: inline; 
        vertical-align: middle;
        font-size: 25px;

    }
    #leftContent {
        width: 50%; 

    }
    #middleContent {
        width: 25%; 

    }
    #rightContent {
        width: 25%;
        font-size: 15px;
    }
</style>