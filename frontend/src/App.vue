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