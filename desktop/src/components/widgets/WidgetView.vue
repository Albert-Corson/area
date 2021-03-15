<template>
  <div class="widget-view">
    <div class="header">
      {{ widget.header }}
    </div>
    <div
      v-if="isImage"
      class="image-preview"
      :style="`background-image: url(${widget.image})`"
    ></div>
    <div v-else-if="isVideo" class="video-preview">
      <vue-plyr ref="plyr">
        <video crossorigin>
          <source :src="widget.image" type="video/mp4" />
        </video>
      </vue-plyr>
    </div>
  </div>
</template>

<script>
export default {
  name: "widget-view",
  props: {
    widget: Object,
    visible: Boolean
  },
  watch: {
    visible(isVisible) {
      const player = this.$refs.plyr?.player
      if (!player) {
        return
      }
      if (isVisible) {
        player.play()
      } else {
        player.stop()
      }
    }
  },
  computed: {
    isVideo() {
      const extension = this.widget.image?.split(".").pop()
      return extension === "mp4"
    },
    isImage() {
      return this.widget.image && this.isVideo === false
    }
  },
  mounted() {
    const player = this.$refs.plyr?.player
    if (!player) {
      return
    }
    if (this.visible) {
      player.play()
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.widget-view {
  .header {
    z-index: 2;
  }

  .image-preview,
  .video-preview {
    z-index: 1;
    height: 100%;
    width: 100%;
    left: 0;
    top: 0;
    border-radius: $borderRadius;
    position: absolute;
    overflow: hidden;
  }

  .image-preview {
    background-position: center;
    background-size: cover;
    background-repeat: no-repeat;
  }

  video {
    object-fit: cover;
  }
}
</style>
