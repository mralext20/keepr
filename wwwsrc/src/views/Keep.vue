<template>
  <div class="container">
    <keep :data="keep" />
  </div>
</template>

<script>
import Keep from "../components/Keep";
export default {
  name: "Keep-vue",
  components: {
    Keep
  },
  mounted() {
    this.$store.dispatch("setActiveKeep", this.$route.params.id);
  },
  computed: {
    keep() {
      return this.$store.state.activeKeep;
    }
  }
};
</script>

<style>
</style>