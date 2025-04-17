
$(document).ready(function () {

    const connection = new signalR.HubConnectionBuilder().
        //Bağlantı URL'sini ayarla
        withUrl("/examplehub")
        //Loglama seviyesini ayarla
        .configureLogging(signalR.LogLevel.Information).build();

    //Hub ile bağlantı başlatılır
    function start() {
        connection.start().then(() => console.log("Hub ile bağlantı kuruldu"));
    }

    //Bağlantı başlatılır sorun olur ise 5 saniye sonra tekrar başlatılır
    try {
        start();
    } catch (e) {
        setTimeout(() => start(),5000)
    }

})