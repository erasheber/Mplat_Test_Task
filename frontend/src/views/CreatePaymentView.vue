<template>
  <section>
    <h2>Создание платежа</h2>
    <form class="card" @submit.prevent="onSubmit">
      <div class="grid">
        <label>
          Номер кошелька *
          <input v-model.trim="form.walletNumber" />
          <small v-if="errors.walletNumber">{{ errors.walletNumber }}</small>
        </label>
        <label>
          Аккаунт / UserId *
          <input v-model.trim="form.account" />
          <small v-if="errors.account">{{ errors.account }}</small>
        </label>
        <label>
          Email *
          <input v-model.trim="form.email" />
          <small v-if="errors.email">{{ errors.email }}</small>
        </label>
        <label>
          Телефон
          <input v-model.trim="form.phone" placeholder="+77001234567" />
          <small v-if="errors.phone">{{ errors.phone }}</small>
        </label>
        <label>
          Сумма *
          <input v-model="form.amount" inputmode="decimal" />
          <small v-if="errors.amount">{{ errors.amount }}</small>
        </label>
        <label>
          Валюта *
          <select v-model="form.currency">
            <option value="RUB">RUB</option>
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
            <option value="KZT">KZT</option>
          </select>
          <small v-if="errors.currency">{{ errors.currency }}</small>
        </label>
        <label class="full">
          Комментарий
          <textarea v-model.trim="form.comment" rows="3"></textarea>
        </label>
      </div>
      <div class="actions">
        <button :disabled="loading" type="submit">
          {{ loading ? "Отправка..." : "Создать" }}
        </button>
        <button :disabled="loading" type="button" class="secondary" @click="reset">
          Очистить
        </button>
      </div>
      <p v-if="success" class="ok">
        ✅ Платёж создан. Id: <b>{{ success.id }}</b>, Status: <b>{{ success.status }}</b>
      </p>
      <p v-if="serverError" class="err">{{ serverError }}</p>
    </form>
  </section>
</template>

<script setup>
import {reactive, ref} from "vue";
import {createPayment} from "../api/payments";

const loading = ref(false);
const serverError = ref("");
const success = ref(null);

const form = reactive({
  walletNumber: "",
  account: "",
  email: "",
  phone: "",
  amount: "",
  currency: "USD",
  comment: "",
});

const errors = reactive({
  walletNumber: "",
  account: "",
  email: "",
  phone: "",
  amount: "",
  currency: "",
});

function clearErrors() {
  Object.keys(errors).forEach((k) => (errors[k] = ""));
}

function validate() {
  clearErrors();
  let ok = true;

  if (!form.walletNumber) { errors.walletNumber = "Обязательное поле"; ok = false; }
  if (!form.account) { errors.account = "Обязательное поле"; ok = false; }

  if (!form.email) {
    errors.email = "Обязательное поле"; ok = false;
  } else {
    const emailOk = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email);
    if (!emailOk) { errors.email = "Некорректный email"; ok = false; }
  }

  if (form.phone) {
    const phoneOk = /^\+?[0-9]{7,15}$/.test(form.phone);
    if (!phoneOk) { errors.phone = "Телефон: 7-15 цифр, можно с +"; ok = false; }
  }

  const amountNum = Number(String(form.amount).replace(",", "."));
  if (!form.amount) {
    errors.amount = "Обязательное поле"; ok = false;
  } else if (!Number.isFinite(amountNum) || amountNum <= 0) {
    errors.amount = "Сумма должна быть положительной"; ok = false;
  }

  if (!form.currency) { errors.currency = "Выберите валюту"; ok = false; }

  return { ok, amountNum };
}

function reset() {
  form.walletNumber = "";
  form.account = "";
  form.email = "";
  form.phone = "";
  form.amount = "";
  form.currency = "USD";
  form.comment = "";
  clearErrors();
  serverError.value = "";
  success.value = null;
}

async function onSubmit() {
  serverError.value = "";
  success.value = null;

  const { ok, amountNum } = validate();
  if (!ok) return;

  loading.value = true;
  try {
    const payload = {
      walletNumber: form.walletNumber,
      account: form.account,
      email: form.email,
      phone: form.phone || null,
      amount: amountNum,
      currency: form.currency,
      comment: form.comment || null,
    };

    success.value = await createPayment(payload);
  } catch (e) {
    const msg =
        e?.response?.data?.message ||
        e?.response?.data ||
        e?.message ||
        "Ошибка запроса";
    serverError.value = typeof msg === "string" ? msg : JSON.stringify(msg);
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
section{
  margin-top: 18px;
}

h2{
  margin: 18px 0 10px;
  font-size: 22px;
}

.card{
  border-radius: calc(var(--radius) + 4px);
  padding: 18px;
  background: linear-gradient(180deg, rgba(255,255,255,.10), rgba(255,255,255,.06));
  border: 1px solid var(--stroke);
  box-shadow: var(--shadow);
  backdrop-filter: blur(10px);
}

.grid{
  display:grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}

label{
  display:flex;
  flex-direction: column;
  gap: 7px;
  font-size: 13px;
  color: var(--muted);
}

.full{ grid-column: 1 / -1; }

input, select, textarea{
  width: 100%;
  color: var(--text);
  background: rgba(10,14,28,.55);
  border: 1px solid rgba(255,255,255,.14);
  border-radius: 14px;
  padding: 11px 12px;
  outline: none;

  transition: border-color .15s ease, box-shadow .15s ease, transform .1s ease;
}

input::placeholder, textarea::placeholder{
  color: rgba(255,255,255,.35);
}

input:focus, select:focus, textarea:focus{
  border-color: rgba(49,201,255,.45);
  box-shadow: 0 0 0 4px rgba(49,201,255,.12);
}

small{
  color: rgba(255,77,109,.95);
  display:block;
  margin-top: 2px;
  font-size: 12px;
}

.actions{
  display:flex;
  gap: 10px;
  margin-top: 14px;
  flex-wrap: wrap;
}

button{
  border: 1px solid rgba(255,255,255,.14);
  border-radius: 14px;
  padding: 10px 14px;
  cursor: pointer;
  font-weight: 700;
  color: var(--text);
  background: rgba(255,255,255,.08);

  transition: transform .15s ease, box-shadow .15s ease, background .15s ease, opacity .15s ease;
}

button:hover{
  transform: translateY(-1px);
  background: rgba(255,255,255,.10);
  box-shadow: 0 14px 30px rgba(0,0,0,.25);
}

button:active{ transform: translateY(0); }
button:disabled{ opacity: .55; cursor:not-allowed; }

button[type="submit"]{
  border-color: rgba(124,92,255,.35);
  background: linear-gradient(135deg, rgba(124,92,255,.45), rgba(49,201,255,.25));
}

.secondary{
  background: rgba(255,255,255,.06);
}

.ok{
  margin-top: 14px;
  padding: 12px 12px;
  border-radius: 14px;
  border: 1px solid rgba(42,224,138,.25);
  background: rgba(42,224,138,.10);
  color: rgba(219,255,235,.95);
}

.err{
  margin-top: 14px;
  padding: 12px 12px;
  border-radius: 14px;
  border: 1px solid rgba(255,77,109,.25);
  background: rgba(255,77,109,.10);
  color: rgba(255,220,228,.95);
}

@media (max-width: 820px){
  .grid{ grid-template-columns: 1fr; }
}
</style>