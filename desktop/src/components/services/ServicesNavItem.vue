<template>
  <div class="services-nav-item">
    <a :href="`#service-${service.id}`" v-smooth-scroll>
      {{ service.name }}
    </a>
  </div>
</template>

<script>
export default {
  name: "services-nav-item",
  props: {
    service: Object
  }
}
</script>
