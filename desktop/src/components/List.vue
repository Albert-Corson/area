<template>
  <div class="list">
    <div class="list-item" v-for="(item, index) in items" :key="index">
      <slot :item="item" v-bind="$attrs" />
    </div>
  </div>
</template>

<script>
export default {
  name: "list",
  props: {
    items: Array
  }
}
</script>
