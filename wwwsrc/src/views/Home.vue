<template>
  <div class="home container-fluid">
    <div class="row">
      <keep :key="keep.id" v-for="keep in publicKeeps" class="col-md-2 col-12" :data="keep" />
    </div>
  </div>
</template>

<script>
import Keep from "../components/Keep.vue";
export default {
  name: "home",
  components: {
    Keep
  },
  mounted() {
    this.$store.dispatch("getKeeps");
  },
  computed: {
    user() {
      return this.$store.state.user;
    },
    publicKeeps() {
      return this.$store.state.publicKeeps;
    }
  },
  methods: {
    logout() {
      this.$store.dispatch("logout");
    }
  }
};
</script>
