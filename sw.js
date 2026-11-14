/* Fate's Passage PWA service worker.
   Cache version is intentionally separate from saved character data. */
const CACHE_NAME = "fates-passage-offline-v3";
const BASE = self.registration.scope;

const CORE_FILES = [
  "./",
  "./index.html",
  "./manifest.webmanifest",
  "./CharacterCreation.html",
  "./Fatespassage.html",
  "./character-sheet.html",
  "./Arkaedon — Level 1 Map 3.html",
  "./triune skill melding forge landscape fit.html",
  "./spell-tome.html",
  "./icons/fates-passage-180.png",
  "./icons/fates-passage-512.png"
];

const EXTERNAL_FILES = [
  "https://cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js",
  "https://fonts.googleapis.com/css2?family=Cinzel:wght@400;500;600;700;800&family=Cormorant+Garamond:ital,wght@0,500;0,600;0,700;1,500;1,600&family=Outfit:wght@300;400;500;600&display=swap",
  "https://fonts.googleapis.com/css2?family=Rajdhani:wght@500;600;700&family=Cinzel:wght@600;800&display=swap"
];

const CACHEABLE_EXTERNAL_HOSTS = new Set([
  "cdnjs.cloudflare.com",
  "fonts.googleapis.com",
  "fonts.gstatic.com"
]);

async function cacheOne(cache, url) {
  try {
    const request = new Request(url, { cache: "reload" });
    const response = await fetch(request);
    if (response && response.ok) {
      await cache.put(request, response.clone());
      return true;
    }
  } catch (error) {
    // Missing optional files or an unavailable CDN must not break installation.
  }
  return false;
}

self.addEventListener("install", event => {
  event.waitUntil((async () => {
    const cache = await caches.open(CACHE_NAME);
    // Fetch files individually: one missing optional screen must not cancel the cache.
    for (const path of CORE_FILES) {
      await cacheOne(cache, new URL(path, BASE).href);
    }
    for (const url of EXTERNAL_FILES) {
      await cacheOne(cache, url);
    }
    await self.skipWaiting();
  })());
});

self.addEventListener("activate", event => {
  event.waitUntil((async () => {
    const keys = await caches.keys();
    await Promise.all(keys
      .filter(key => key.startsWith("fates-passage-offline-") && key !== CACHE_NAME)
      .map(key => caches.delete(key)));
    await self.clients.claim();
  })());
});

self.addEventListener("fetch", event => {
  const request = event.request;
  if (request.method !== "GET") return;

  const url = new URL(request.url);
  const sameOrigin = url.origin === self.location.origin;
  const allowedExternal = CACHEABLE_EXTERNAL_HOSTS.has(url.hostname);
  if (!sameOrigin && !allowedExternal) return;

  event.respondWith((async () => {
    const cache = await caches.open(CACHE_NAME);
    const cached = await cache.match(request, { ignoreSearch: true, ignoreVary: true });
    if (cached) return cached;

    try {
      const response = await fetch(request);
      if (response && response.ok) {
        try { await cache.put(request, response.clone()); } catch (error) {}
      }
      return response;
    } catch (error) {
      const fallback = await cache.match(request, { ignoreSearch: true, ignoreVary: true });
      if (fallback) return fallback;

      if (sameOrigin && request.mode === "navigate") {
        return await cache.match(new URL("./CharacterCreation.html", BASE).href)
          || await cache.match(new URL("./index.html", BASE).href)
          || new Response("Fate's Passage is not cached yet. Connect to the internet and open the game once to prepare offline play.", {
            status: 503,
            headers: { "Content-Type": "text/plain; charset=utf-8" }
          });
      }
      return new Response("This Fate's Passage resource is not available offline yet.", {
        status: 503,
        headers: { "Content-Type": "text/plain; charset=utf-8" }
      });
    }
  })());
});
