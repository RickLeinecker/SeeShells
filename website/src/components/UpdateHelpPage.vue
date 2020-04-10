<template>
    <div id="helpHeader">
        <h1>Update the Help Information</h1>
        <div id="helpContent">
            <b-form-group label="Title:">
                <b-form-input v-model="helpTitle" type="text" placeholder="TITLE" />
            </b-form-group>
            <div id="editor">
                <b-form-group label="Content:">
                    <b-form-textarea v-model="helpText" rows="20" />
                </b-form-group>
            </div>
            <b-button @click="onSubmit" variant="primary">Update Help Content</b-button>
            <div id="messages"></div>
            <div>
                <br />
                <h2>Preview of the Help Content: </h2>
                <br />
                <h1>{{helpTitle}}</h1>
            </div>
            <div id="previewContent">
                <VueShowdown :markdown="helpText" />
            </div>
        </div>
    </div>
</template>

<script>
    import CheckIfAuthenticated from '../mixins/CheckIfAuthenticated';

    export default {
        name: 'HelpPage',
        mixins: [CheckIfAuthenticated],
        data() {
            return {
                helpTitle: '',
                helpText: '<h5>Loading help...</h5>',
            }
        },
        methods: {
            populatePage() {
                var url = this.$baseurl + 'getHelpInformation';

                var xhr = new XMLHttpRequest();
                xhr.open("GET", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                try {
                    xhr.send(null);
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        this.helpTitle = result.json[0].title;
                        this.helpText = result.json[0].description;
                    }
                    else {
                        this.helpText = 'Failed to get the documentation!';
                    }

                }
                catch (err) {
                    console.info(err);
                }
            },
            onSubmit() {
                if (this.helpTitle == '' || this.helpText == '')
                    return;

                var url = this.$baseurl + 'changeHelpInformation';

                var jsonPayload = {title: this.helpTitle, description: this.helpText};

                var xhr = new XMLHttpRequest();
                xhr.open("POST", url, false);
                xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");
                xhr.setRequestHeader("X-Auth-Token", this.$session.get('session'));

                try {
                    xhr.send(JSON.stringify(jsonPayload));
                    var result = JSON.parse(xhr.responseText);

                    if (result.success == 1) {
                        (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-success alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Help saved! </strong>The updated help information can now be seen on the website and in the application. </div>');       
                    }
                    else if (result.message == "You must log in to perform this action.") {
                            this.$session.destroy();
                            this.$router.push('/SeeShells/login');
                            location.reload();
                    }
                    else {
                        (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to update the help information. </div>');                    
                    }
                }
                catch (err) { 
                    console.log(err);
                    (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong> Failed to update the help information. </div>');                    
                }
            }
        },
        mounted() {
            this.$nextTick().then(this.populatePage);
        }
    }
</script>

<style scoped>
    #helpHeader {
        margin: 50px;
    }
    #helpContent {
        margin: auto;
        height: 100%;
        width: 90%;
    }
    #editor {
      margin: 0;
      height: 100%;
      font-family: "Helvetica Neue", Arial, sans-serif;
      color: #333;
    }

    textarea,
    #editor div {
      display: inline-block;
      height: 100%;
      vertical-align: top;
      box-sizing: border-box;
      padding: 0 20px;
    }
    textarea {
      border: none;
      border-right: 1px solid #ccc;
      background-color: #f6f6f6;
      font-size: 16px;
      font-family: "Monaco", courier, monospace;
      height: 100%;
    }

    #previewContent {
        margin: auto;
        height: 100%;
        width: 90%;
        text-align: left;
    }

</style>