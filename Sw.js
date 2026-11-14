const CACHE="fates-passage-v2";
const CORE=[
"./","./index.html","./manifest.webmanifest",
"./Fate's Passage - Character Creation 3.html",
"./Fate's Passage 2.html",
"./character-sheet.html",
"./Arkaedon — Level 1 Map 3.html",
"./triune skill melding forge landscape fit.html",
"./spell-tome.html",
"./icons/fates-passage-180.png",
"./icons/fates-passage-512.png"
];
const EXTERNAL=["https://cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js"];

self.addEventListener("install",e=>e.waitUntil((async()=>{
 const c=await caches.open(CACHE);
 await c.addAll(CORE);
 for(const u of EXTERNAL){
   try{
     const r=await fetch(u,{mode:"no-cors",cache:"no-store"});
     await c.put(u,r);
   }catch(_){}
 }
 await self.skipWaiting();
})()));

self.addEventListener("activate",e=>e.waitUntil((async()=>{
 const keep=CACHE;
 for(const k of await caches.keys()) if(k!==keep) await caches.delete(k);
 await self.clients.claim();
})()));

self.addEventListener("fetch",e=>{
 const u=new URL(e.request.url);
 if(e.request.method!=="GET") return;
 e.respondWith((async()=>{
   const c=await caches.open(CACHE);
   const hit=await c.match(e.request);
   if(hit) return hit;
   try{
     const r=await fetch(e.request);
     if(u.origin===location.origin || u.href.includes("cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js")){
       try{await c.put(e.request,r.clone())}catch(_){}
     }
     return r;
   }catch(_){
     return c.match(e.request)||c.match("./index.html");
   }
 })());
});
