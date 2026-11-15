const CACHE_NAME = "fates-passage-offline-v2";
const FILES_TO_CACHE = [
  "./",
  "./index.html",
  "./manifest.json",
  "./IMG_7733.jpeg",
  "./character-creation.html",
  "./fates-passage.html",
  "./arkaedon-map.html",
  "./character-sheet.html",
  "./spell-tome.html",
  "./melding-forge.html"
];
/* =========================================================
   INSTALL
   ========================================================= */
self.addEventListener("install", event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => cache.addAll(FILES_TO_CACHE))
      .then(() => self.skipWaiting())
  );
});
/* =========================================================
   ACTIVATE
   ========================================================= */
self.addEventListener("activate", event => {
  event.waitUntil(
    caches.keys()
      .then(cacheNames => {
        return Promise.all(
          cacheNames
            .filter(name =>
              name.startsWith("fates-passage-offline-")
            )
            .filter(name =>
              name !== CACHE_NAME
            )
            .map(name =>
              caches.delete(name)
            )
        );
      })
      .then(() => self.clients.claim())
  );
});
/* =========================================================
   FETCH
   ========================================================= */
self.addEventListener("fetch", event => {
  const request = event.request;
  /*
    Only handle normal GET requests.
  */
  if (request.method !== "GET") {
    return;
  }
  /*
    Only handle requests belonging
    to the Fate's Passage site.
  */
  const url = new URL(request.url);
  if (url.origin !== self.location.origin) {
    return;
  }
  event.respondWith(
    /*
      Try the internet first.
      This keeps the online version current.
    */
    fetch(request)
      .then(response => {
        /*
          Save successful responses so they
          can be used when offline later.
        */
        if (response.ok) {
          const responseCopy =
            response.clone();
          caches.open(CACHE_NAME)
            .then(cache => {
              cache.put(
                request,
                responseCopy
              );
            })
            .catch(() => {});
        }
        return response;
      })
      /*
        Internet unavailable.
        Try the cached version.
      */
      .catch(async () => {
        const cached =
          await caches.match(request);
        if (cached) {
          return cached;
        }
        /*
          If the requested page isn't cached
          but this is a navigation request,
          return Character Creation as the
          game's safe entry point.
        */
        if (request.mode === "navigate") {
          const characterCreation =
            await caches.match(
              "./character-creation.html"
            );
          if (characterCreation) {
            return characterCreation;
          }
        }
        /*
          Nothing available offline.
        */
        return new Response(
          `
          <!DOCTYPE html>
          <html lang="en">
          <head>
            <meta charset="UTF-8">
            <meta name="viewport"
                  content="width=device-width,
                           initial-scale=1.0">
            <title>Fate's Passage - Offline</title>
          </head>
          <body>
            <h1>Fate's Passage</h1>
            <p>
              This part of the game has not been
              cached for offline use yet.
            </p>
          </body>
          </html>
          `,
          {
            status: 503,
            statusText: "Offline",
            headers: {
              "Content-Type": "text/html"
            }
          }
        );
      })
  );
});
