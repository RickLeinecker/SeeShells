<template>
  <div id="register">
    <b-form class="m-5" @submit="onRegister" v-if="show">
      <b-form-group label="Username:" >
        <b-form-input v-model="form.name" required placeholder="Enter username"></b-form-input>
      </b-form-group>

      <b-form-group label="Password:">
          <b-form-input v-model="form.password" type="password" required placeholder="Enter password"></b-form-input>
          <password v-model="form.password" :strength-meter-only="true" />
          <b-form-input v-model="form.passwordconfirm" type="password" required placeholder="Re-enter password"></b-form-input>
          <div id="messages"></div>
      </b-form-group>

      <b-button type="register" variant="primary">Register</b-button>
    </b-form>
  </div>
</template>

<script>
    import Password from 'vue-password-strength-meter'
    export default {
        name: 'RegisterForm',
        components: { Password },
        data() {
          return {
            form: {
                  name: '',
                  password: '',
                  passwordconfirm: '',
            },
            show: true
          }
        },
        methods: {
            onRegister(event) {
                event.preventDefault();
                var baseurl = 'https://seeshells.herokuapp.com/';
                

                if (this.form.password == this.form.passwordconfirm) {
                    var jsonPayload = '{"username":"' + this.form.name + '", "password":"' + this.form.password + '"}';
                    var url = baseurl + 'register';

                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", url, false);
                    xhr.setRequestHeader("Content-type", "application/json; charset=UTF-8");

                    try {
                        xhr.send(jsonPayload);
                        var result = JSON.parse(xhr.responseText);

                        if (result.success == 1) {
                            (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-info alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Registration sent! </strong>You must wait for a current administrator to approve you now. </div>');
        
                        }
                        else {
                            (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Error! </strong>' + result.error + ' Please try again later. </div>');
                        }

                    }
                    catch (err) { 
                        alert(err.message)
                    }
                }
                else {
                    (document.getElementById('messages')).insertAdjacentHTML('afterend', '<div class="alert alert-danger alert-dismissible">  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>  <strong>Passwords don\'t match! </strong>Re-enter the passwords. </div>');
                }
          }
        }
    }
</script>

<style>
    #register{
        margin: auto;
        height: 100%;
        width: 40%;
    }
</style>