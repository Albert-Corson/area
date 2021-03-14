<template>
  <div class="widgets-list-item">
    <span class="widget-name">{{ widget.name }}</span>
    <span class="widget-description">{{ widget.description }}</span>
  </div>
</template>

<script>
export default {
  name: "widgets-list-item",
  props: {
    widget: Object
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars.scss";

.widgets-list-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-radius: $borderRadius;
  box-shadow: $upShadow, $downShadow;
  text-align: center;
  padding: 1rem;
  height: 12rem;
  width: 12rem;
  opacity: 1;
  margin: 1rem;
  cursor: pointer;
  transition: 0.15s opacity ease-in-out;
  &:hover {
    opacity: 0.6;
  }

  .widget-name {
    display: block;
    font-weight: bolder;
    margin-bottom: 0.3rem;
  }
}
</style>
