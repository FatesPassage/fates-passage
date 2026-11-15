// Fate's Passage service worker
// Same-origin: network-first (always the live file), falls back to cache when offline.
// CDN (three.js) + Google Fonts: cache-first, so the game runs fully offline after one online visit.
// Bump CACHE_VERSION any time you want to wipe old caches.
const CACHE_VERSION = "v3";
const CACHE_NAME = "fates-passage-" + CACHE_VERSION;

const PAGES = [
  "./", "./index.html", "./studio-title.html", "./character-creation.html",
  "./fates-passage.html", "./character-sheet.html", "./melding-forge.html",
  "./spell-tome.html", "./arkaedon-map.html"
];
const CDN = ["https://cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js"];
const isCdn = (u) => /(^|\.)(cdnjs\.cloudflare\.com|fonts\.googleapis\.com|fonts\.gstatic\.com)$/.test(u.hostname);

self.addEventListener("install", (event) => {
  self.skipWaiting();
  event.waitUntil((async () => {
    const cache = await caches.open(CACHE_NAME);
    // each file is cached on its own, so a file that doesn't exist yet never breaks the install
    await Promise.all([
      ...PAGES.map((u) => cache.add(new Request(u, { cache: "reload" })).catch(() => {})),
      ...CDN.map((u) => fetch(u, { mode: "cors" }).then((r) => (r && r.ok ? cache.put(u, r) : null)).catch(() => {}))
    ]);
  })());
});

self.addEventListener("activate", (event) => {
  event.waitUntil(
    caches.keys()
      .then((keys) => Promise.all(keys.filter((k) => k !== CACHE_NAME).map((k) => caches.delete(k))))
      .then(() => self.clients.claim())
  );
});

self.addEventListener("fetch", (event) => {
  const req = event.request;
  if (req.method !== "GET") return;
  const url = new URL(req.url);

  if (url.origin !== self.location.origin) {
    if (!isCdn(url)) return;
    event.respondWith(
      caches.match(req).then((hit) => hit || fetch(req).then((res) => {
        if (res && (res.ok || res.type === "opaque")) {
          const copy = res.clone();
          caches.open(CACHE_NAME).then((c) => c.put(req, copy)).catch(() => {});
        }
        return res;
      }))
    );
    return;
  }

  event.respondWith(
    fetch(req, { cache: "no-cache" })
      .then((res) => {
        if (res && res.status === 200) {
          const copy = res.clone();
          caches.open(CACHE_NAME).then((c) => c.put(req, copy)).catch(() => {});
        }
        return res;
      })
      .catch(() => caches.match(req, { ignoreSearch: true }))
  );
});
