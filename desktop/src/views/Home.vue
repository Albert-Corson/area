<template>
  <div class="home">
    <About />
  </div>
</template>

<script lang="ts">
import Vue from "vue"
import About from "@/components/About.vue"

export default Vue.extend({
  name: "Home",
  components: {
    About
  }
})
</script>
