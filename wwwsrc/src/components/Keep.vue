<template>
  <div>
    <div class="card">
      <div class="embed-responsive embed-responsive-16by9">
        <a v-if="onSingleKeep" :href="data.img">
          <img class="card-img-top embed-responsive-item" :src="data.img" />
        </a>
        <img v-else class="card-img-top embed-responsive-item" :src="data.img" />
      </div>
      <div class="card-body">
        <router-link :to="{name:'keep', params:{id:data.id}}">
          <div v-if="!editing">
            <h4 class="card-title">{{data.name}}</h4>
            <p class="card-text">{{data.description}}</p>
            <p class="card-text">
              Views: {{data.views}}
              keeps: {{data.keeps}}
              shares: {{data.shares}}
            </p>
          </div>
          <div v-else>
            <input v-model="edited.name" placeholder="name" />
            <input v-model="edited.description" placeholder="description" />
            <input v-model="edited.img" placeholder="image url" />
            <button class="btn btn-success" @click="submitEdit">Send</button>
          </div>
        </router-link>
        <div v-if="sub == data.userId">
          <button class="btn btn-danger" @click="remove">Delete</button>
          <button class="btn btn-warning" @click="editing = !editing">edit</button>
        </div>
        <div v-if="data.vaultKeepId">
          <button class="btn btn-danger" @click="$emit('remove-from-vault')">Remove from Vault</button>
        </div>
        <div v-if="vaults && Object.keys(vaults).length > 0" class="dropdown">
          <button
            class="btn btn-secondary dropdown-toggle"
            type="button"
            data-toggle="dropdown"
          >Add To Vault</button>
          <div class="dropdown-menu">
            <a
              v-for="vault in vaults"
              @click="addToVault(vault.id)"
              :key="vault.id"
              class="dropdown-item"
            >{{vault.name}}</a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import "bootstrap";
export default {
  data() {
    return {
      editing: false,
      edited: {}
    };
  },
  mounted() {
    this.edited = { ...this.data };
  },
  props: ["data", "vaults"],
  computed: {
    sub() {
      return this.$auth.userInfo.sub;
    },
    onSingleKeep() {
      return this.$route.name == "keep";
    }
  },
  methods: {
    remove() {
      this.$store.dispatch("removeKeep", this.data.id);
    },
    submitEdit() {
      this.$store.dispatch("editKeep", this.edited);
      this.editing = false;
    },
    addToVault(vaultId) {
      this.$store.dispatch("addToVault", { vaultId, keep: this.data });
    }
  }
};
</script>

<style scoped>
.embed-responsive .card-img-top {
  object-fit: cover;
}
/*  https://stackoverflow.com/a/54766582/3236881  */
</style>