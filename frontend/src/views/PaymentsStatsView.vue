<template>
  <section>
    <h2>Статистика платежей</h2>
    <div class="controls">
      <label>
        From (YYYY-MM-DD)
        <input v-model.trim="from" placeholder="2026-02-01" />
      </label>
      <label>
        To (YYYY-MM-DD)
        <input v-model.trim="to" placeholder="2026-02-22" />
      </label>
      <button @click="load" :disabled="loading">{{ loading ? "Загрузка..." : "Показать" }}</button>
    </div>
    <p v-if="error" class="err">{{ error }}</p>
    <div v-if="stats" class="cards">
      <div class="card">
        <div class="title">Общая сумма</div>
        <div class="value">{{ stats.totalAmount }}</div>
      </div>
      <div class="card">
        <div class="title">Количество платежей</div>
        <div class="value">{{ stats.totalCount }}</div>
      </div>
    </div>
    <div v-if="stats" class="table-wrap">
      <h3>Агрегация по дням</h3>
      <table v-if="stats.byDays?.length">
        <thead>
        <tr>
          <th>Дата</th>
          <th>Кол-во</th>
          <th>Сумма</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="d in stats.byDays" :key="d.date">
          <td>{{ d.date }}</td>
          <td>{{ d.count }}</td>
          <td>{{ d.amount }}</td>
        </tr>
        </tbody>
      </table>
      <p v-else>Нет данных за период.</p>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { getStats } from "../api/payments";

const stats = ref(null);
const loading = ref(false);
const error = ref("");

const from = ref("");
const to = ref("");

function isIsoDate(s) {
  return /^\d{4}-\d{2}-\d{2}$/.test(s);
}

async function load() {
  error.value = "";
  loading.value = true;
  stats.value = null;

  try {
    const params = {};
    if (from.value) {
      if (!isIsoDate(from.value)) throw new Error("From должен быть YYYY-MM-DD");
      params.from = from.value;
    }
    if (to.value) {
      if (!isIsoDate(to.value)) throw new Error("To должен быть YYYY-MM-DD");
      params.to = to.value;
    }

    stats.value = await getStats(params);
  } catch (e) {
    const msg = e?.response?.data || e?.message || "Ошибка загрузки";
    error.value = typeof msg === "string" ? msg : JSON.stringify(msg);
  } finally {
    loading.value = false;
  }
}

onMounted(load);
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
  min-width: 220px;
}

input{
  padding: 10px 12px;
  border: 1px solid rgba(255,255,255,.14);
  border-radius: 14px;
  background: rgba(10,14,28,.55);
  color: var(--text);
  outline:none;
}

input:focus{
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

.cards{
  display:flex;
  gap: 12px;
  margin: 14px 0;
  flex-wrap: wrap;
}

.card{
  flex: 1 1 260px;
  border-radius: calc(var(--radius) + 4px);
  padding: 14px 14px;

  background: linear-gradient(180deg, rgba(255,255,255,.10), rgba(255,255,255,.06));
  border: 1px solid var(--stroke);
  box-shadow: 0 16px 50px rgba(0,0,0,.28);
  backdrop-filter: blur(10px);
}

.title{
  color: var(--muted);
  font-size: 12px;
  letter-spacing: .3px;
}

.value{
  font-size: 28px;
  font-weight: 900;
  margin-top: 6px;
  line-height: 1.1;
}

.table-wrap{
  margin-top: 12px;
  border-radius: calc(var(--radius) + 4px);
  border: 1px solid var(--stroke);
  background: rgba(255,255,255,.06);
  box-shadow: var(--shadow);
  overflow: hidden;
}

.table-wrap h3{
  margin: 0;
  padding: 12px 14px;
  font-size: 14px;
  color: rgba(255,255,255,.88);
  border-bottom: 1px solid rgba(255,255,255,.10);
  background: rgba(15,23,48,.55);
}

table{
  width:100%;
  border-collapse: collapse;
}

th, td{
  text-align:left;
  padding: 12px 14px;
  border-bottom: 1px solid rgba(255,255,255,.08);
}

thead th{
  font-size: 12px;
  color: rgba(255,255,255,.85);
  background: rgba(15,23,48,.50);
}

tbody tr:nth-child(even){
  background: rgba(255,255,255,.03);
}

tbody tr:hover{
  background: rgba(49,201,255,.08);
}
</style>