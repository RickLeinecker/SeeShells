<template>
    <div id="guidHeader">
        <div id="container">
            <div id="leftContent"><b-form-input v-model="guid" type="text" placeholder="GUID"></b-form-input></div>
            <div id="middleContent"><b-form-input v-model="name" type="text" placeholder="Name"></b-form-input></div>
            <div id="rightContent"><b-button @click="onSubmit" variant="primary">Submit name for GUID</b-button></div>
        </div>
        <br />

        <p>If this GUID already exists, it will be overwritten with the name you input here.</p>
        <div id="messages"></div>

    </div>
</template>

<script>

    export default {
        name: 'NewGUID',
        data() {
            return {
                guid: '',
                name: ''
            }
        },
        methods: {
            onSubmit() {
                if (this.guid == '' || this.name == '')
                    return;

                var url = this.$baseurl + 'addGUID';

                var jsonPayload = '{"guid":"' + this.guid + '", "name":"' + this.name + '"}';

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(jsonPayload);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        location.reload();
                        (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-success alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>GUID saved! </strong>To use it in the desktop application, just update your GUID configuration file in the application. </div>');       
                    }
                    else if (result.message == "You must log in to perform this action.") {
                            this.$session.destroy();
                            this.$router.push('/SeeShells/login');
                            location.reload();
                    }
                    else {
                        (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to add or update the GUID. </div>');                    
                    }
                }
                catch (err) { 
                    console.log(err);
                    (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to add or update the GUID. </div>');                    
                }
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
        width: 35%; 

    }
    #middleContent {
        width: 35%; 

    }
    #rightContent {
        width: 30%;
        font-size: 15px;
    }
    
    p {
        text-align: center;
    }


</style>