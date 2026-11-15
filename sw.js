// Fate's Passage service worker
// Network-first: always tries the live site, falls back to cache only when offline.
// Bump CACHE_VERSION any time you want to wipe old caches.
const CACHE_VERSION = "v2";
const CACHE_NAME = "fates-passage-" + CACHE_VERSION;

self.addEventListener("install", (event) => {
  // Activate this worker immediately instead of waiting for old tabs to close
  self.skipWaiting();
});

self.addEventListener("activate", (event) => {
  event.waitUntil(
    caches
      .keys()
      .then((keys) =>
        Promise.all(
          keys
            .filter((key) => key !== CACHE_NAME)
            .map((key) => caches.delete(key))
        )
      )
      .then(() => self.clients.claim())
  );
});

self.addEventListener("fetch", (event) => {
  const req = event.request;

  // Only handle same-origin GET requests
  if (req.method !== "GET") return;
  if (new URL(req.url).origin !== self.location.origin) return;

  event.respondWith(
    // cache: "no-cache" makes the browser revalidate instead of using a stale HTTP cache
    fetch(req, { cache: "no-cache" })
      .then((res) => {
        if (res && res.status === 200) {
          const copy = res.clone();
          caches.open(CACHE_NAME).then((cache) => cache.put(req, copy)).catch(() => {});
        }
        return res;
      })
      .catch(() => caches.match(req))
  );
});
