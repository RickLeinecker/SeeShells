<template>
  <div id="app">
    <NavigationBar />

    <div id="page">
        <RegisterForm />
    </div>
  </div>
</template>

<script>
    import NavigationBar from './components/NavigationBar.vue'
    import RegisterForm from './components/RegisterForm.vue'

export default {
  name: 'App',
        components: {
          NavigationBar,
          RegisterForm
        }
}
</script>

<style>
#page {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin: 150px;
}
</style>
