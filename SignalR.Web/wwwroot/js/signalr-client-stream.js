

$(document).ready(function () {

    ///BAĞLANTI İŞLEMLERİ
    const connection = new signalR.HubConnectionBuilder().
        //Bağlantı URL'sini ayarla
        withUrl("/exampleTypeSafeHub")
        //Loglama seviyesini ayarla
        .configureLogging(signalR.LogLevel.Information).build();

    //Hub ile bağlantı başlatılır
    async function start() {

        try {
            await connection.start().then(() => {
                console.log("Hub ile bağlantı kuruldu");
                //Connection ID işlemleri
                $("#connectionId").html(`Connection Id : ${connection.connectionId}`);
            });

        } catch (err) {
            console.error("Bağlantı Hatası (Hub)", err);
            setTimeout(() => start(), 2000)
        }
    }

    //bağlantı koptuğu zaman
    connection.onclose(async () => {
        //bağlanmak için bekleyecek
        await start();
    })

    start();

    ///BAĞLANTI İŞLEMLERİ









})