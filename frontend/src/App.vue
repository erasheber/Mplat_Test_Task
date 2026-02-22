<template>
  <div class="app">
    <div class="container">
      <NavBar />
      <RouterView />
    </div>
  </div>
</template>

<script setup>
import NavBar from "./components/Navbar.vue";
</script>

<style scoped>
.wrap { max-width: 980px; margin: 0 auto; padding: 16px; }
</style>