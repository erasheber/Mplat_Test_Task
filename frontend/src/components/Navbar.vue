<template>
  <nav class="nav">
    <RouterLink to="/create" class="link" active-class="active">Создать платёж</RouterLink>
    <RouterLink to="/history" class="link" active-class="active">История</RouterLink>
    <RouterLink to="/stats" class="link" active-class="active">Статистика</RouterLink>
  </nav>
</template>

<style scoped>
.nav{
  position: sticky;
  top: 14px;
  z-index: 10;

  display:flex;
  gap:10px;
  padding: 10px;
  border-radius: var(--radius);

  background: linear-gradient(180deg, rgba(255,255,255,.10), rgba(255,255,255,.06));
  border: 1px solid var(--stroke);
  box-shadow: var(--shadow);
  backdrop-filter: blur(10px);
}

.link{
  text-decoration:none;
  color: var(--muted);
  padding: 10px 12px;
  border-radius: 12px;
  font-weight: 600;
  font-size: 14px;

  transition: transform .15s ease, background .15s ease, color .15s ease;
}

.link:hover{
  color: var(--text);
  background: rgba(255,255,255,.08);
  transform: translateY(-1px);
}

.active{
  color: var(--text);
  background: linear-gradient(135deg, rgba(124,92,255,.35), rgba(49,201,255,.18));
  border: 1px solid rgba(255,255,255,.18);
}
</style>