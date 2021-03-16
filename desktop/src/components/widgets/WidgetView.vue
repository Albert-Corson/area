<template>
  <div
    class="widget-view"
    :class="{ clickable: isClickable }"
    @click="redirect"
    @mouseenter="unmuteVideo"
    @mouseleave="muteVideo"
  >
    <div class="legend">
      {{ widget.header }}
    </div>
    <div class="media-preview">
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
  </div>
</template>

<script>
export default {
  name: "widget-view",
  props: {
    widget: Object,
    visible: Boolean
  },
  computed: {
    isVideo() {
      const extension = this.widget.image?.split(".").pop()
      return extension === "mp4"
    },
    isImage() {
      return this.widget.image && this.isVideo === false
    },
    isClickable() {
      return Boolean(this.widget.link)
    }
  },
  watch: {
    visible(isVisible) {
      if (isVisible) {
        this.playVideo()
      } else {
        this.stopVideo()
      }
    }
  },
  methods: {
    redirect() {
      if (this.widget.link) {
        window.open(this.widget.link)
      }
    },
    muteVideo() {
      const player = this.$refs.plyr?.player
      if (player) {
        player.volume = 0
      }
    },
    unmuteVideo() {
      const player = this.$refs.plyr?.player
      if (player) {
        player.volume = 1
      }
    },
    playVideo() {
      const player = this.$refs.plyr?.player
      if (player) {
        player.play()
      }
    },
    stopVideo() {
      const player = this.$refs.plyr?.player
      if (player) {
        player.stop()
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.widget-view {
  &.clickable {
    cursor: pointer;
  }

  .legend {
    z-index: 1;
    position: absolute;
    top: 0;
    left: 0;
    height: 2.5em;
    padding: 0 0.5rem;
    width: 100%;
    line-height: 2.5em;
    display: block;
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    border-radius: $borderRadius $borderRadius 0 0;
    background: $bgColor;
    box-shadow: 0 1em 0 0 $bgColor;
  }

  .media-preview {
    position: absolute;
    height: calc(100% - 2.5em);
    width: 100%;
    left: 0;
    bottom: 0;
  }

  .image-preview,
  .video-preview {
    z-index: 2;
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
