const CACHE_NAME = "fates-passage-offline-v5";
const FILES_TO_CACHE = [
  "./",
  "./index.html",
  "./FatesPassage.html",
  "./ArkaedonMap.html",
  "./CharacterSheet.html",
  "./SpellTome.html",
  "./MeldingForge.html",
  "./manifest.webmanifest"
];
self.addEventListener("install", event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => cache.addAll(FILES_TO_CACHE))
      .then(() => self.skipWaiting())
  );
});
self.addEventListener("activate", event => {
  event.waitUntil(
    caches.keys()
      .then(names =>
        Promise.all(
          names
            .filter(
              name =>
                name.startsWith("fates-passage-") &&
                name !== CACHE_NAME
            )
            .map(name => caches.delete(name))
        )
      )
      .then(() => self.clients.claim())
  );
});
self.addEventListener("fetch", event => {
  const request = event.request;
  if (request.method !== "GET") {
    return;
  }
  const url = new URL(request.url);
  if (url.origin !== self.location.origin) {
    return;
  }
  event.respondWith(
    fetch(request)
      .then(response => {
        if (response.ok) {
          const copy = response.clone();
          caches.open(CACHE_NAME)
            .then(cache => cache.put(request, copy))
            .catch(() => {});
        }
        return response;
      })
      .catch(async () => {
        const cached = await caches.match(request);
        if (cached) {
          return cached;
        }
        /*
         * IMPORTANT:
         * Never send a failed navigation to the obsolete
         * CharacterCreation.html page.
         *
         * The current game entry point is FatesPassage.html.
         */
        if (request.mode === "navigate") {
          const gamePage = await caches.match("./FatesPassage.html");
          if (gamePage) {
            return gamePage;
          }
          const indexPage = await caches.match("./index.html");
          if (indexPage) {
            return indexPage;
          }
        }
        return new Response(
          "Offline resource unavailable",
          {
            status: 503,
            statusText: "Offline"
          }
        );
      })
  );
});
