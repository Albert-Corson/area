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
  text-align: center;

  padding: 1rem;
  &:not(:last-of-type) {
    margin-bottom: 2rem;
  }
  min-height: 12rem;
  width: 100%;
  cursor: pointer;

  border-radius: $borderRadius;
  box-shadow: $upShadow, $downShadow;

  opacity: 1;
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
