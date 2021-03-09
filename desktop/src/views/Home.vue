<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png" />
    <Counter />
  </div>
</template>

<script lang="ts">
import Vue from "vue"
import Counter from "@/components/Counter.vue"

export default Vue.extend({
  name: "Home",
  components: {
    Counter
  }
})
</script>
