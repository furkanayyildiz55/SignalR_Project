//cHROME CONSOLE SHORTCUT => CTRL + SHIFT + J
//Sayfa yüklendikten (HTML DOM tamamen yüklendikten) sonra belirli JavaScript kodlarının çalışmasını sağlar.
$(document).ready(function () {

    const broadcastMessageToClientHubMethodCall = "BroadcastMessageToClient";  //Hub'da tanımlı olan metot ismi
    const receiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient"; //Client tarafında tanımlı olan metot ismi

    const receiveConnectedClientCountForAllClient = "ReceiveConnectedClientCountForAllClient";

    const receiveMessageForCallerClient = "ReceiveMessageForCallerClient";
    const broadcastMessageToCallerClient = "BroadcastMessageToCallerClient";

    const receiveMessageForOtherClient = "ReceiveMessageForOtherClient";
    const broadcastMessageToOtherClient = "BroadcastMessageToOtherClient";

    const broadcastMessageToIndividualClient = "BroadcastMessageToIndividualClient";
    const receiveMessageForIndividualClient = "ReceiveMessageForIndividualClient";

    const receiveTypedMessageForAllClient = "ReceiveTypedMessageForAllClient";
    const broadcastTypedMessageToAllClient = "BroadcastTypedMessageToAllClient";

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


    //Butona tıklandığında hub'a mesaj gönderilir
    $("#btn-send-message-all-client").click(function () {
        const message = "Hello World!";
        connection.invoke(broadcastMessageToClientHubMethodCall, message).catch(errr => console.error("Hata", err));
    })

    //Hub'dan gelen mesajı alır. Burası sunucu yani hub tarafından tetiklenir //Subscribe olmak 
    connection.on(receiveMessageForAllClientMethodCall, (message) => {
        console.log("Gelen Mesaj", message)
    })


    //Hub a bağlı sekme sayısını alır
    var span_client_count = $("#connected-client-count");
    connection.on(receiveConnectedClientCountForAllClient, (count) => {
        span_client_count.text(count);
        console.log("connected client count", count)

    })


    //CALLER 
    //Butona tıklandığında hub'a mesaj gönderilir
    $("#btn-send-message-caller-client").click(function () {
        const message = "Hello World! QWER";
        connection.invoke(broadcastMessageToCallerClient, message).catch(errr => console.error("Hata", err));
    })    

    connection.on(receiveMessageForCallerClient, (message) => {
        console.log("(Caller) Gelen Mesaj", message);
    })
    //CALLER

    //OTHER CLİENT MESSAGE EXAMPLE
    connection.on(receiveMessageForOtherClient, (message) => {
        console.log("(Other) Gelen Mesaj", message);
    })

    $("#btn-send-message-other-client").click(function () {
        const message = "Hello World! DİĞERLERİ";
        connection.invoke(broadcastMessageToOtherClient, message).catch(errr => console.error("Hata", err));
    })


    //spesific client'a mesaj gönderme 
    $("#btn-send-message-individual-client").click(function () {
        const connectionId = $("#text-connectiondId").val();
        const message = "Hello World! Bireysel";
        connection.invoke(broadcastMessageToIndividualClient, connectionId, message).catch(errr => console.error("Hata", err));
    })

    connection.on(receiveMessageForIndividualClient, (message) => {
        console.log("(Spesific) Gelen Mesaj", message);
    })
    //OTHER CLİENT MESSAGE EXAMPLE

    //GRUP İŞLEMLERİ ÖRNEĞİ
    const groupA = "GrupA";
    const groupB = "GrupB";
    let currentGroupList = [];

    //Html olarak grup listesini düzenler kullanıcıya gösterir
    function refreshGroupList() {
        $("#groupList").empty();
        currentGroupList.forEach(group => {
            $("#groupList").append(`<p>${group}</p>`);
        })
    }

    $("#btn-groupA-add").click(function () {

        if (currentGroupList.includes(groupA)) return;

        connection.invoke("AddGroup", groupA).then(() => {
            currentGroupList.push(groupA);
            refreshGroupList();
        })
    })

    $("#btn-groupA-remove").click(function () {
        if (!currentGroupList.includes(groupA)) return;
        connection.invoke("RemoveGroup", groupA).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupA);
            refreshGroupList();
        })
    })

    $("#btn-groupB-add").click(function () {
        if (currentGroupList.includes(groupB)) return;

        connection.invoke("AddGroup", groupB).then(() => {
            currentGroupList.push(groupB);
            refreshGroupList();
        })
    })

    $("#btn-groupB-remove").click(function () {
        if (!currentGroupList.includes(groupB)) return;
        connection.invoke("RemoveGroup", groupB).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupB);
            refreshGroupList();
        })
    })


    $("#btn-groupA-send-message").click(function () {

        const message = "Hello World! Grup A";
        connection.invoke("BroadcastMessageToGroupClients", groupA, message).catch(err => console.error("Hata", err));
        console.log("Grup A'ya mesaj gönderildi => İçierik :", message);
    })


    $("#btn-groupB-send-message").click(function () {

        const message = "Hello World! Grup B";
        connection.invoke("BroadcastMessageToGroupClients", groupB, message).catch(err => console.error("Hata", err));
        console.log("Grup B'ya mesaj gönderildi => İçierik :", message);
    })

    connection.on("ReceiveMessageForGroupClients", (message) => {
        console.log("Gelen Mesaj", message);
    })

    //GRUP İŞLEMLERİ ÖRNEĞİ

    //TİP GÜVENLİ MESAJ GÖNDERME

    $("#btn-send-typed-message-all-client").click(function () {

        const product = {
            id: 1,
            name: "Product 1",
            price: 100
        }

        connection.invoke(broadcastTypedMessageToAllClient, product).catch(err => console.error("Hata", err));
        console.log("(Typed Mesaj) Mesaj Gönderildi");

    })

    connection.on(receiveTypedMessageForAllClient, (product) => {

        console.log("(Typed Mesaj) Gelen Mesaj", product);
    })

    //TİP GÜVENLİ MESAJ GÖNDERME



})