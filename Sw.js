
const CACHE = "fates-passage-renamed-v1";

const CORE = [
  "./",
  "./index.html",
  "./charactercreation.html",
  "./fatespassage.html"
];

self.addEventListener("install", event => {
  event.waitUntil((async () => {
    const cache = await caches.open(CACHE);

    await Promise.all(CORE.map(async path => {
      try {
        const response = await fetch(path, { cache: "reload" });
        if (response.ok) {
          await cache.put(path, response);
        }
      } catch (error) {
        console.warn("Could not cache:", path, error);
      }
    }));

    await self.skipWaiting();
  })());
});

self.addEventListener("activate", event => {
  event.waitUntil((async () => {
    const keys = await caches.keys();

    await Promise.all(
      keys
        .filter(key => key !== CACHE)
        .map(key => caches.delete(key))
    );

    await self.clients.claim();
  })());
});

self.addEventListener("fetch", event => {
  const request = event.request;

  if (request.method !== "GET") return;

  const url = new URL(request.url);

  if (url.origin !== self.location.origin) return;

  event.respondWith((async () => {
    const cache = await caches.open(CACHE);
    const cached = await cache.match(request);

    if (cached) return cached;

    try {
      const response = await fetch(request);

      if (response.ok) {
        try {
          await cache.put(request, response.clone());
        } catch (error) {}
      }

      return response;

    } catch (error) {
      return await cache.match(request) ||
        await cache.match("./index.html") ||
        new Response(
          "Fate's Passage is unavailable offline. Open it once while connected.",
          {
            status: 503,
            headers: {
              "Content-Type": "text/plain; charset=utf-8"
            }
          }
        );
    }
  })());
});
