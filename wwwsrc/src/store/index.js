import Vue from "vue";
import Vuex, { Store } from "vuex";
import Axios from "axios";
import router from "../router";

Vue.use(Vuex);

let baseUrl = location.host.includes("localhost")
  ? "https://localhost:5001/"
  : "/";

let api = Axios.create({
  baseURL: baseUrl + "api/",
  timeout: 5000,
  withCredentials: true
});

export default new Vuex.Store({
  state: {
    publicKeeps: {},
    activeKeep: {},
    yourKeeps: {},
    yourVaults: {},
    activeVault: {}

  },
  mutations: {
    setPublicKeeps(state, payload) {
      let data = {}
      payload.forEach(each => {
        data[each.id] = each;
      });
      state.publicKeeps = data;
    },
    addPublicKeep(state, payload) {
      Vue.set(state.publicKeeps, payload.id, payload)
    },
    removeKeep(state, id) {
      Vue.delete(state.publicKeeps, id)
    },
    editKeep(state, payload) {
      Vue.set(state.publicKeeps, payload.id, payload)
    },
    setActiveKeep(state, payload) {
      state.activeKeep = payload;
    },
    viewBump(state, id) {
      state.publicKeeps[id].views += 1;
    },
    addYourKeep(state, payload) {
      Vue.set(state.yourKeeps, payload.id, payload)
    },
    setYourKeeps(state, payload) {
      let data = {}
      payload.forEach(each => {
        data[each.id] = each
      });
      state.yourKeeps = data
    },
    addManyPublicKeeps(state, payload) {
      payload.forEach(each => {
        Vue.set(state.publicKeeps, each.id, each);
      });
    },
    setYourVaults(state, payload) {
      payload.forEach(e => {
        Vue.set(state.yourVaults, e.id, e);
      })
    },
    addAVault(state, payload) {
      Vue.set(state.yourVaults, payload.id, payload);
    },
    setVaultKeeps(state, { vaultId, data }) {
      state.yourVaults[vaultId].keeps = data
    },
    setActiveVault(state, payload) {
      state.activeVault = payload;
    },
    deleteVaultKeep(state, { vaultId, vkid, }) {
      state.activeVault.keeps = state.activeVault.keeps.filter(keep => keep.vaultKeepId != vkid);
      state.yourVaults[vaultId].keeps = state.yourVaults[vaultId].keeps.filter(keep => keep.vaultKeepId != vkid)
    },
    addKeepToVault(state, { vaultId, keep }) {
      keep.keeps += 1;
      if (state.yourVaults[vaultId].keeps == undefined) {
        state.yourVaults[vaultId].keeps = []
      }
      state.yourVaults[vaultId].keeps.push(keep);
    },
    deleteVault(state, vaultId) {
      Vue.delete(state.yourVaults, vaultId)
      if (state.activeVault.id == vaultId) {
        state.activeVault = {};
      }
    },
    bumpShares(state, keepId) {
      if (state.activeKeep.id == keepId) {
        state.activeKeep.share++;
      }
    }
  },



  actions: {
    setBearer({ }, bearer) {
      api.defaults.headers.authorization = bearer;
    },
    resetBearer() {
      api.defaults.headers.authorization = "";
    },
    //#region points
    async getKeeps({ commit }) {
      let res = await api.get("keeps")
      commit("setPublicKeeps", res.data);
    },
    async removeKeep({ commit }, id) {
      await api.delete(`keeps/${id}`)
      commit("removeKeep", id);
    },
    async editKeep({ commit }, data) {
      let res = await api.put(`keeps/${data.id}`, data);
      commit("editKeep", res.data)
    },
    async setActiveKeep({ commit, state }, id) {
      api.post(`keeps/${id}/view`)
      if (state.publicKeeps[id]) {
        commit("viewBump", id)
        commit("setActiveKeep", state.publicKeeps[id])
        return
      }
      let res = await api.get(`keeps/${id}`);
      commit("setActiveKeep", res.data);
      if (!res.data.isPrivate) {
        commit("addPublicKeep", res.data);
      }
    },
    async createKeep({ commit }, newKeep) {
      let res = await api.post("keeps", newKeep)

      if (!res.data.isPrivate) {
        commit("addPublicKeep", res.data)
      }
      commit("addYourKeep", res.data)
      router.push({ name: "keep", params: { id: res.data.id } })
    },
    async getYourKeeps({ commit }) {
      let res = await api.get("keeps/mine")
      commit("setYourKeeps", res.data);
      let filtered = res.data.filter(k => !k.isPrivate);
      commit("addManyPublicKeeps", filtered);
    },

    async shareBump({ commit }, keepId) {
      await api.post(`keeps/${keepId}/share`);
      commit("bumpShares", keepId);
    },
    //#endregion

    //#region vaults
    async createVault({ commit }, newVault) {
      let res = await api.post("vaults", newVault)
      commit("addAVault", res.data)
      router.push({ name: "vault", params: { id: res.data.id } })
    },
    async getYourVaults({ commit }) {
      let res = await api.get("vaults");
      commit("setYourVaults", res.data);
    },
    async getVaultKeeps({ commit }, id) {
      let res = await api.get(`vaults/${id}/keeps`);
      commit("setVaultKeeps", { vaultId: id, data: res.data })
    },
    async getAVault({ commit }, id) {
      let res = await api.get(`vaults/${id}`);
      commit("addAVault", res.data);
    },
    async setActiveVault({ commit, dispatch, state }, id) {
      if (state.yourVaults[id]) {
        if (state.yourVaults[id].keeps) {

          // we already have all the data! dont do any network requests, just set the active vault.
          commit("setActiveVault", state.yourVaults[id])
          return;
        }
        // we only need to get the keeps
        await dispatch("getVaultKeeps", id)
        commit("setActiveVault", state.yourVaults[id])
      }
      // we need to get the vault too
      await dispatch("getAVault", id);
      await dispatch("getVaultKeeps", id);
      commit("setActiveVault", state.yourVaults[id]);
    },

    async deleteVaultKeep({ commit }, { vaultId, vkid }) {
      await api.delete(`vaultkeeps/${vkid}`)
      commit("deleteVaultKeep", { vaultId, vkid });
    },

    async addToVault({ commit }, { vaultId, keep }) {
      await api.post("vaultkeeps", { vaultId, keepId: keep.id });
      // we MUST have the vault if we have the ID already.
      commit("addKeepToVault", { vaultId, keep })
      router.push({ name: "vault", params: { id: vaultId } })
    },
    async deleteVault({ commit }, vaultId) {
      await api.delete(`vaults/${vaultId}`);
      commit("deleteVault", vaultId);
      router.push({ name: "dashboard vaults" })
    }
    //#endregion
  }
});
