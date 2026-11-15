const CACHE = "fates-passage-github-v2";

const CORE = [
  "./",
  "./index.html",
  "./Fate%27s%20Passage%20%E2%80%94%20Character%20Creation%203.html",
  "./Fate%27s%20Passage%202.html",
  "./Arkaedon%20%E2%80%94%20Level%201%20Map%203.html",
  "./character-sheet.html"
];

self.addEventListener("install", event => {
  self.skipWaiting();

  event.waitUntil(
    caches.open(CACHE).then(cache => {
      return cache.addAll(CORE);
    })
  );
});

self.addEventListener("activate", event => {
  event.waitUntil(
    caches.keys().then(keys => {
      return Promise.all(
        keys
          .filter(key => key !== CACHE)
          .map(key => caches.delete(key))
      );
    }).then(() => self.clients.claim())
  );
});

self.addEventListener("fetch", event => {
  const request = event.request;

  // Always let non-GET requests pass through.
  if (request.method !== "GET") return;

  // HTML navigation should prefer the live GitHub Pages file.
  // This prevents an obsolete cached HTML page from being served.
  if (
    request.mode === "navigate" ||
    request.destination === "document" ||
    request.url.endsWith(".html")
  ) {
    event.respondWith(
      fetch(request, {
        cache: "no-store"
      }).then(response => {
        const copy = response.clone();

        caches.open(CACHE).then(cache => {
          cache.put(request, copy);
        });

        return response;
      }).catch(() => {
        return caches.match(request);
      })
    );

    return;
  }

  // Other assets can use the cache, with the network as fallback.
  event.respondWith(
    caches.match(request).then(cached => {
      return cached || fetch(request);
    })
  );
});
