<template>
  <div class="counter">
    <h1>Counter</h1>
    <button @click="decrement">-</button>
    {{ count }}
    <button @click="increment">+</button>
  </div>
</template>

<script lang="ts">
import Vue from "vue"

export default Vue.extend({
  name: "Counter",
  methods: {
    increment() {
      this.$store.dispatch("increment")
    },
    decrement() {
      this.$store.dispatch("decrement")
    }
  },
  computed: {
    count() {
      return this.$store.getters.count
    }
  }
})
</script>

<style scoped lang="scss"></style>
