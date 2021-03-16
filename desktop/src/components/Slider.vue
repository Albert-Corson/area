<template>
  <div class="slider" @mouseenter="stopSliding" @mouseleave="resumeSliding">
    <div class="counter">
      <div class="prev" @click="slidePrev">&lt;</div>
      {{ index + 1 }} / {{ size + 1 }}
      <div class="next" @click="slideNext">&gt;</div>
    </div>
    <transition-group :name="transition" mode="out-in">
      <div
        v-for="(item, index) in items"
        :key="index"
        class="slider-item"
        v-show="isSelected(index)"
        ref="sliderItem"
      >
        <slot :item="item" v-bind="$attrs" :visible="isSelected(index)" />
      </div>
    </transition-group>
  </div>
</template>

<script>
export default {
  name: "slider",
  props: {
    items: Array,
    transition: {
      type: String,
      default: "none"
    }
  },
  data() {
    return {
      index: 0,
      isSliding: true
    }
  },
  computed: {
    size() {
      return this.$props.items ? this.$props.items.length - 1 : 0
    }
  },
  mounted() {
    window.setInterval(() => {
      if (this.isSliding) {
        this.slideNext()
      }
    }, 5 * 1000)
  },
  methods: {
    stopSliding() {
      this.isSliding = false
    },
    resumeSliding() {
      this.isSliding = true
    },
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
@import "@/styles/transitions";

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
      z-index: 3;
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
