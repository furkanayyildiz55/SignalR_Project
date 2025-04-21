


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


    ///STREAM İŞLEMLERİ

    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";
    const receiveMessageAsStreamForAllClients = "ReceiveMessageAsStreamForAllClients";

    $("#btn-stream-tohub").click(function () {

        const names = $("#txt-stream").val();
        const nameAsChunk = names.split(";");

        const subject = new signalR.Subject();  //SignalR'da Subject, istemciden sunucuya veri akışı göndermek için kullanılır.

        //Subject üzerinden veri gönderileceği bilgisi sunucuya iletiliyor. Verileri subject gönderecek
        //sunucu verileri dinlemeye başlar
        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err))

        nameAsChunk.forEach(name => {
            subject.next(name)  //Her next işleminde broadcastStreamDataToAllClient metoduna veri gönderimi sağlanır.
        })

        subject.complete(); //Sunucu tarafına veri gönderiminin bittiği söylenir, bu ifadeden sonra next() kullanılamaz
    })

    connection.on(receiveMessageAsStreamForAllClients, (name) => {

        $("#stream-box").append(`<p>${name}</p>`)
    })


        //PRODUCT TYPE İLE STREAM
        const broadcastStreamProductDataToAllClient = "BroadcastStreamProductDataToAllClient";
        const receiveProductAsStreamForAllClients = "ReceiveProductAsStreamForAllClients";

        $("#btn-product-stream-tohub").click(function () {

            var productList = [
                {id: 1, name:"pen1", price:100},
                {id: 2, name:"pen1", price:100},
                {id: 3, name:"pen1", price:100},
                {id: 4, name:"pen1", price:100}
            ]

            const subject = new signalR.Subject();

            connection.send(broadcastStreamProductDataToAllClient, subject).catch(err => console.error(err))

            productList.forEach(product => {
                subject.next(product)
            })

            subject.complete();

        })

        connection.on(receiveProductAsStreamForAllClients, (product) => {

            $("#stream-box").append(`<p>ID: ${product.id}, Name: ${product.name}, Price: ${product.price}</p>`);

        })
        //PRODUCT TYPE İLE STREAM


    const broadcastFromHubToClient = "BroadcastFromHubToClient";

    $("#btn-from-hub-to-client").click(function () {

        connection.stream(broadcastFromHubToClient, 15).subscribe({
            next: (message) => $("#stream-box").append(`<p>${message}</p>`)

        })

    })


    ///STREAM İŞLEMLERİ






})