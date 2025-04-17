
//Sayfa yüklendikten (HTML DOM tamamen yüklendikten) sonra belirli JavaScript kodlarının çalışmasını sağlar.
$(document).ready(function () {

    const broadcastMessageToClientHubMethodCall = "BroadcastMessageToClient";  //Hub'da tanımlı olan metot ismi
    const ReceiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient"; //Client tarafında tanımlı olan metot ismi

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
        setTimeout(() => start(), 5000)
    }

    //Hub'dan gelen mesajı alır. Burası sunucu yani hub tarafından tetiklenir
    connection.on(ReceiveMessageForAllClientMethodCall, (message) => {
        console.log("Gelen Mesaj", message)
    })


    //Butona tıklandığında hub'a mesaj gönderilir
    $("#btn-send-message-all-client").click(function () {

        const message = "Hello World!";
        connection.invoke(broadcastMessageToClientHubMethodCall, message).catch(errr => console.error("Hata", err));
    })

})