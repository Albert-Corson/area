<template>
  <div
    class="popup-container"
    :class="{ open: isOpen, closed: isClosed }"
    @click="closeIfContainer"
    ref="container"
  >
    <div class="popup">
      <div class="popup-header">
        <p class="popup-title" v-if="title">{{ title }}</p>
        <button class="popup-close" @click="close">×</button>
      </div>
      <div class="popup-body" v-if="hasBody">
        <slot name="body" />
      </div>
      <div class="popup-footer" v-if="hasFooter">
        <slot name="footer" />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "popup",
  props: {
    title: String
  },
  data() {
    return {
      isOpen: false,
      isClosed: true
    }
  },
  computed: {
    hasBody() {
      return this.$slots.body
    },
    hasFooter() {
      return this.$slots.footer
    }
  },
  beforeDestroy() {
    this.close()
  },
  methods: {
    open() {
      this.isClosed = false
      window.setTimeout(() => {
        this.isOpen = true
        this.$emit("open")
        document.addEventListener("keydown", this.handleEscapeKey)
      }, 10)
    },
    close() {
      document.removeEventListener("keydown", this.handleEscapeKey)
      this.isOpen = false
      window.setTimeout(() => {
        this.isClosed = true
        this.$emit("close")
      }, 300)
    },
    handleEscapeKey(e) {
      if (e.key === "Escape") {
        this.close()
      }
    },
    closeIfContainer(e) {
      if (e.target === this.$refs.container) {
        this.close()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.popup-container {
  text-align: left;
  left: 0;
  top: 0;
  z-index: 100;
  position: fixed;
  width: 100vw;
  height: 100vh;
  background: rgba(0, 0, 0, 0.5);
  transition-duration: 300ms;
  transition-property: opacity;
  opacity: 0;

  &.open {
    opacity: 1;

    .popup {
      top: 10vh;
    }
  }

  &:not(.open) {
    .popup {
      top: 0;
    }
  }

  &.closed {
    display: none;
  }

  .popup {
    left: 50%;
    transform: translateX(-50%);
    min-width: 500px;
    max-width: 95vw;
    max-height: 80vh;
    overflow-y: auto;
    z-index: 100;
    position: fixed;
    background: $bgColor;
    box-shadow: rgba(0, 0, 0, 0.04) 0px 3px 5px;
    border-radius: $borderRadius;
    transition-duration: 300ms;
    transition-property: opacity, top;

    .popup-header,
    .popup-body,
    .popup-footer {
      padding: 1rem;

      &:not(:last-child) {
        border-bottom: 1px solid #ccc;
      }
    }

    .popup-header {
      font-weight: bold;
      font-size: larger;
      display: flex;
      align-items: flex-start;
      justify-content: space-between;

      .popup-title {
        margin: 0;
        display: inline-block;
      }

      .popup-close {
        padding: 1rem;
        margin: -1rem -1rem -1rem auto;
        cursor: pointer;
        background-color: transparent;
        border: 0;
        appearance: none;
        float: right;
        line-height: 1;
        font-weight: bold;
        font-size: 1.5rem;
        color: #000;
        opacity: 0.5;

        &:hover {
          opacity: 1;
        }
      }
    }

    .popup-footer {
      text-align: right;

      button {
        margin-top: 0;
        padding: 0.7em;
        display: inline-block;
        width: auto;
        cursor: pointer;
        text-align: center;
        border-radius: $borderRadius;
        transition: background 100ms;

        &.primary {
          background: $accentColor;
          color: white;

          &:hover {
            background: darken($accentColor, 6%);
          }
        }

        &.secondary {
          background: $gray;
          color: white;

          &:hover {
            background: darken($gray, 6%);
          }
        }

        &:not(:last-child) {
          margin-right: 0.25rem;
        }

        &:not(:first-child) {
          margin-left: 0.25rem;
        }
      }
    }
  }
}
</style>
