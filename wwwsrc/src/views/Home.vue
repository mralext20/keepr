<template>
  <div class="home container-fluid">
    <div class="row">
      <div :key="keep.id" v-for="keep in publicKeeps" class="card col-md-2 col-12">
        <router-link :to="{name:'keep', params:{id:keep.id}}">
          <keep :data="keep" />
        </router-link>
      </div>
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
