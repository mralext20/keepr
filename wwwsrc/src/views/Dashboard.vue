<template>
  <div class="dashboard container">
    <ul class="nav justify-content-center nav-tabs">
      <li class="nav-item">
        <router-link
          :class="{active:$route.path.endsWith('vaults')}"
          :to="{name:'dashboard vaults'}"
          class="nav-link"
        >vaults</router-link>
      </li>
      <li class="nav-item">
        <router-link
          :class="{active:$route.path.endsWith('keeps')}"
          :to="{name:'dashboard keeps'}"
          class="nav-link"
        >keeps</router-link>
      </li>
    </ul>
    <div class="row" v-if="$route.path.endsWith('vaults')">
      <div v-for="vault in yourVaults" :key="vault.id" class="col-md-4 col-12">
        <div class="card">
          <div class="card-body">
            <h4 class="card-title">{{vault.name}}</h4>
            <p class="card-text">{{vault.description}}</p>
          </div>
        </div>
      </div>
    </div>
    <div class="row" v-else-if="$route.path.endsWith('keeps')">
      <div v-for="keep in yourKeeps" :key="keep.id" class="col-md-4 col-12">
        <router-link class="card" :to="{name:'keep', params:{id:keep.id}}">
          <keep :data="keep" />
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { onAuth } from "@bcwdev/auth0-vue";
import Keep from "../components/Keep";
export default {
  name: "dashboard",
  async mounted() {
    await onAuth();
    await this.$store.dispatch("setBearer", this.$auth.bearer);
    this.$store.dispatch("getYourVaults");
    this.$store.dispatch("getYourKeeps");
  },
  computed: {
    yourKeeps() {
      return this.$store.state.yourKeeps;
    },
    yourVaults() {
      return this.$store.state.yourVaults;
    }
  },
  components: {
    Keep
  }
};
</script>

<style></style>
