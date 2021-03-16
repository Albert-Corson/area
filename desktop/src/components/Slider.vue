<template>
  <div class="slider">
    <div class="counter">
      <div class="prev" @click="slidePrev">&lt;</div>
      {{ index + 1 }} / {{ size + 1 }}
      <div class="next" @click="slideNext">&gt;</div>
    </div>
    <div
      v-for="(item, index) in items"
      :key="index"
      class="slider-item"
      v-show="isSelected(index)"
      ref="sliderItem"
    >
      <slot :item="item" v-bind="$attrs" :visible="isSelected(index)" />
    </div>
  </div>
</template>

<script>
export default {
  name: "slider",
  data() {
    return {
      index: 0
    }
  },
  computed: {
    size() {
      return this.$props.items ? this.$props.items.length - 1 : 0
    }
  },
  props: {
    items: Array
  },
  methods: {
    isSelected(index) {
      return index === this.index
    },
    slidePrev() {
      this.index = this.index - 1
      if (this.index < 0) {
        this.index = this.size
      }
    },
    slideNext() {
      this.index = this.index + 1
      if (this.index > this.size) {
        this.index = 0
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.slider {
  position: relative;

  .counter {
    min-width: 4rem;
    bottom: 0;
    left: 50%;
    transform: translate(-50%, 50%);
    user-select: none;
    z-index: 2;
    opacity: 0.4;
    transition: opacity 0.2s ease-out;

    &:hover {
      opacity: 1;
    }

    &,
    .next,
    .prev {
      position: absolute;
      background-color: rgb(55, 55, 55);
      border-radius: $borderRadius;
      color: white;
      padding: 0.3rem 0.9rem;
    }

    .prev {
      top: 0;
      left: 0;
      transform: translateX(-100%);
    }

    .next {
      top: 0;
      right: 0;
      transform: translateX(100%);
    }
  }
}
</style>
