<template>
    <div class="header">
      <header-button v-on:reload="reload"/>
      <header-glitter/>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'nuxt-property-decorator'
import HeaderButton from "~/components/header/HeaderButton.vue"
import HeaderGlitter from "~/components/header/HeaderGlitter.vue"

@Component({
  name: 'Header',
  components: {
    HeaderButton,
    HeaderGlitter,
  }
})
export default class Header extends Vue {
  // methods
  public reload() {
    this.$emit('reload')
  }
}
</script>

<style scoped>

.content
{
  grid-area: content;
  border: solid 1px black
}

.header
{
  grid-area: header;
}

.header {
  background-color: var(--main-bg-color);
  height: 100%;
  width: 100%;
  display: grid;
  grid-template-columns: 0.75fr 5fr;
  grid-template-rows: 1fr;
  gap: 0px 0px;
  grid-template-areas:
    "header-button header-glitter";
}

</style>
