<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  name: 'MosaicList',
  render(h) {
    const items = this.$slots?.default?.map(item => h('div', { class: 'list-item' }, [item]))
    return h('div', { class: 'mosaic-list' }, items)
  }
})
export default class MosaicList extends Vue {
}
</script>

<style lang="scss" scoped>
  .mosaic-list {
    display: flex;
    flex-wrap: wrap;

    .list-item {
      margin: .30rem;
    }
  }
</style>
