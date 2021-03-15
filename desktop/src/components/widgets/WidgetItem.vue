<template>
  <div class="widget-item">
    {{ item.header }}
    <!--<div :style="`background-image: url(${item.image})`">
      {{ item.header }}
    </div>-->
  </div>
</template>

<script>
export default {
  name: "widget-item",
  props: {
    item: Object
  }
}
</script>

<style lang="scss" scoped>
.widget-item {
  & > div {
    background-repeat: no-repeat;
    background-size: cover;
    background-position: center;
    height: 4rem;
  }
}
</style>
