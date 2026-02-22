<template>
  <section>
    <h2>История платежей</h2>
    <div class="controls">
      <label>
        Страница:
        <input type="number" min="1" v-model.number="page" />
      </label>
      <label>
        PageSize:
        <select v-model.number="pageSize">
          <option :value="10">10</option>
          <option :value="20">20</option>
          <option :value="50">50</option>
        </select>
      </label>
      <label>
        Сортировка:
        <select v-model="sort">
          <option value="desc">Сначала новые</option>
          <option value="asc">Сначала старые</option>
        </select>
      </label>
      <button @click="load" :disabled="loading">{{ loading ? "Загрузка..." : "Обновить" }}</button>
    </div>
    <p v-if="error" class="err">{{ error }}</p>
    <div class="table-wrap" v-if="items.length">
      <table>
        <thead>
        <tr>
          <th>Дата</th>
          <th>Аккаунт</th>
          <th>Email</th>
          <th>Сумма</th>
          <th>Валюта</th>
          <th>Статус</th>
          <th>Комментарий</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(p, idx) in items" :key="idx">
          <td>{{ formatDate(p.createdAt) }}</td>
          <td>{{ p.account }}</td>
          <td>{{ p.email }}</td>
          <td>{{ p.amount }}</td>
          <td>{{ p.currency }}</td>
          <td>{{ p.status }}</td>
          <td>{{ p.comment || "-" }}</td>
        </tr>
        </tbody>
      </table>
    </div>
    <p v-else-if="!loading">Платежей пока нет.</p>
  </section>
</template>

<script setup>
import {onMounted, ref, watch} from "vue";
import {getPayments} from "../api/payments";

const items = ref([]);
const loading = ref(false);
const error = ref("");

const page = ref(1);
const pageSize = ref(20);
const sort = ref("desc");

function formatDate(iso) {
  try { return new Date(iso).toLocaleString(); }
  catch { return iso; }
}

async function load() {
  error.value = "";
  loading.value = true;
  try {
    items.value = await getPayments({
      page: page.value,
      pageSize: pageSize.value,
      sort: sort.value,
    });
  } catch (e) {
    const msg = e?.response?.data || e?.message || "Ошибка загрузки";
    error.value = typeof msg === "string" ? msg : JSON.stringify(msg);
  } finally {
    loading.value = false;
  }
}

onMounted(load);

watch([page, pageSize, sort], () => load());
</script>

<style scoped>
section{ margin-top: 18px; }
h2{ margin: 18px 0 10px; font-size: 22px; }

.controls{
  display:flex;
  gap:12px;
  align-items:flex-end;
  flex-wrap:wrap;

  padding: 14px;
  border-radius: var(--radius);
  background: linear-gradient(180deg, rgba(255,255,255,.10), rgba(255,255,255,.06));
  border: 1px solid var(--stroke);
  box-shadow: 0 18px 50px rgba(0,0,0,.30);
  backdrop-filter: blur(10px);

  margin: 10px 0 14px;
}

label{
  display:flex;
  flex-direction:column;
  gap:7px;
  color: var(--muted);
  font-size: 13px;
  min-width: 160px;
}

input, select{
  padding: 10px 12px;
  border: 1px solid rgba(255,255,255,.14);
  border-radius: 14px;
  background: rgba(10,14,28,.55);
  color: var(--text);
  outline:none;
}

input:focus, select:focus{
  border-color: rgba(49,201,255,.45);
  box-shadow: 0 0 0 4px rgba(49,201,255,.12);
}

button{
  padding: 10px 14px;
  border-radius: 14px;
  border: 1px solid rgba(124,92,255,.35);
  background: linear-gradient(135deg, rgba(124,92,255,.45), rgba(49,201,255,.25));
  color: var(--text);
  font-weight: 800;
  cursor:pointer;
  transition: transform .15s ease, box-shadow .15s ease, opacity .15s ease;
}

button:hover{
  transform: translateY(-1px);
  box-shadow: 0 16px 40px rgba(0,0,0,.25);
}
button:disabled{ opacity:.55; cursor:not-allowed; }

.err{
  margin: 10px 0 0;
  padding: 12px;
  border-radius: 14px;
  border: 1px solid rgba(255,77,109,.25);
  background: rgba(255,77,109,.10);
  color: rgba(255,220,228,.95);
}

.table-wrap{
  overflow:auto;
  border-radius: calc(var(--radius) + 4px);
  border: 1px solid var(--stroke);
  background: rgba(255,255,255,.06);
  box-shadow: var(--shadow);
}

table{
  width:100%;
  border-collapse: collapse;
  min-width: 920px;
}

thead th{
  position: sticky;
  top: 0;
  z-index: 1;

  text-align:left;
  padding: 12px 12px;
  font-size: 12px;
  color: rgba(255,255,255,.85);
  letter-spacing: .4px;
  background: rgba(15,23,48,.92);
  border-bottom: 1px solid rgba(255,255,255,.10);
}

tbody td{
  padding: 12px 12px;
  border-bottom: 1px solid rgba(255,255,255,.08);
  color: rgba(255,255,255,.86);
  white-space: nowrap;
}

tbody tr:nth-child(even){
  background: rgba(255,255,255,.03);
}

tbody tr:hover{
  background: rgba(49,201,255,.08);
}
</style>