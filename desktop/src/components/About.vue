<template>
  <div class="about">
    <h1>About</h1>
    <pre>
      {{ JSON.stringify(about, null, 2) }}
    </pre>
  </div>
</template>

<script lang="ts">
import Vue from "vue"

export default Vue.extend({
  name: "About",
  mounted() {
    this.$store.dispatch("About/fetchAbout")
  },
  computed: {
    about() {
      return this.$store.state.About.about
    }
  }
})
</script>

<style scoped lang="scss"></style>
