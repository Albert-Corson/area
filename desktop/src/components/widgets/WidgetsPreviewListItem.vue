<template>
  <widget-preview class="widgets-preview-list-item" :widget="widget" />
</template>

<script>
import WidgetPreview from "./WidgetPreview"

export default {
  name: "widgets-preview-list-item",
  components: {
    WidgetPreview
  },
  props: {
    widget: Object
  }
}
</script>
