<template>
  <div class="hoverable">
    <slot/>
  </div>
</template>

<style lang="scss" scoped>
.hoverable {
  cursor: pointer;
  transition: background .1s ease-in-out;

  &:hover {
    background: var(--focus-color);
  }
}
</style>
