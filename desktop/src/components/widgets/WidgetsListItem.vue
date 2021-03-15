<template>
  <widget class="widgets-list-item" :widget="widget" />
</template>

<script>
import Widget from "./Widget"

export default {
  name: "widgets-list-item",
  components: {
    Widget
  },
  props: {
    widget: Object
  }
}
</script>
