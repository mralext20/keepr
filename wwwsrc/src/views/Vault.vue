<template>
  <div>
    <div class="container">
      <div class="card">
        <div class="card-body">
          <h4 class="card-title">{{vault.name}}</h4>
          <p class="card-text">{{vault.description}}</p>
          <button class="btn btn-danger" @click="deleteVault">Delete Vault</button>
        </div>
      </div>
      <div class="row">
        <keep
          :data="keep"
          v-for="keep in vault.keeps"
          :key="keep.vaultKeepId"
          class="col-12 col-md-4"
          @remove-from-vault="RemoveFromVault(keep.vaultKeepId)"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { onAuth } from "@bcwdev/auth0-vue";
import Keep from "../components/Keep";
export default {
  name: "vault-view",
  async mounted() {
    await onAuth();
    await this.$store.dispatch("setBearer", this.$auth.bearer);
    this.$store.dispatch("setActiveVault", this.$route.params.id);
  },
  components: {
    Keep
  },
  computed: {
    vault() {
      return this.$store.state.activeVault;
    }
  },
  methods: {
    RemoveFromVault(vkid) {
      this.$store.dispatch("deleteVaultKeep", {
        vaultId: this.$route.params.id,
        vkid
      });
    },
    deleteVault() {
      this.$store.dispatch("deleteVault", this.vault.id);
    }
  }
};
</script>

<style>
</style>