<template>
  <div id="register">
    <b-form class="m-5" @submit="onSubmit" @reset="onReset" v-if="show">
      <b-form-group label="Username:" >
        <b-form-input v-model="form.name" required placeholder="Enter username"></b-form-input>
      </b-form-group>

      <b-form-group label="Password:" description="Make a strong password!">
          <b-form-input v-model="form.password" type="password" required placeholder="Enter password"></b-form-input>
          <b-form-input v-model="form.passwordconfirm" type="password" required placeholder="Re-enter password"></b-form-input>
      </b-form-group>

      <b-button type="submit" variant="primary">Submit</b-button>
      <b-button type="reset" variant="danger">Reset</b-button>
    </b-form>
  </div>
</template>

<script>
    export default {
    name: 'RegisterForm',
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
      onSubmit(evt) {
            evt.preventDefault()

            if (this.form.password == this.form.passwordconfirm)
                alert(JSON.stringify(this.form));
            else
                alert("Fail!");
      },
      onReset(evt) {
          evt.preventDefault()

        // Reset our form values
        this.form.name = ''
        this.form.password = ''
          this.form.passwordconfirm = ''

        // Trick to reset/clear native browser form validation state
        this.show = false
        this.$nextTick(() => {
            this.show = true
        })
      }
    }
  }
</script>

<style>
    #register{
        margin-left: 100px;
        margin-right: 100px;
        margin-top: 10px;
        margin-bottom: 10px;
    }
</style>