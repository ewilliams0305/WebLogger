// if("serviceWorker" in Navigator){
    navigator.serviceWorker.register("sw.js").then(registration => {
        console.log("Server Worker Registered");
        console.log(registration);
    }).catch( error =>{
        console.log("Service Worker Failed")
        console.error(error);
    })
// } else {
    // console.error("Not Supported")
// }