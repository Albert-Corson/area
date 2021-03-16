<template>
  <div class="widgets-info-list-item" @click="select">
    <span class="widget-name">{{ widget.name }}</span>
    <span class="widget-description">{{ widget.description }}</span>
  </div>
</template>

<script>
export default {
  name: "widgets-info-list-item",
  props: {
    widget: Object
  },
  methods: {
    select() {
      this.$emit("select", this.widget.id)
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars.scss";

.widgets-info-list-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;

  padding: 1rem;
  margin: 1rem;
  min-height: 15rem;
  width: 15rem;
  margin: 1rem;
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
