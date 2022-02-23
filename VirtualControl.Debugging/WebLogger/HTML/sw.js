self.addEventListener("install", e => {
    console.log("installing");

    e.waitUntil(
        caches.open('static').then(cache =>{
            return cache.addAll(["./","./src/style.css","./images/alien-192x192.png","./images/alien-512x512.png",]);
        })
    )
});

self.addEventListener("fetch", e => {
    console.log(`Intercepting Fetch request ${e.request.url}`);

    e.respondWith(
        caches.match(e.request).then(response => {
            return response || fetch(e.request);
        })
    )
})