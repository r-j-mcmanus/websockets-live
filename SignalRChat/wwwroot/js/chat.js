
let connection = null;


export function setupConnection(token) {

    const connection = new signalR.HubConnectionBuilder() 
        .withUrl(`http://localhost:5235/chathub?access_token=${token}`) 
        .configureLogging(signalR.LogLevel.Information)
        .build()

    connection.on("ReceiveMessage", (user, message) => { addMessageToList(user, message) });

    connection.onclose(async () => {
        await startConnection(); 
    })

    return connection
}

export async function startConnection() {
    if(!connection) {
        throw Error("Connection does not exist to start.");
    }
    try 
    {
        await connection.start();
        console.log("connection started");
    }
    catch (error) 
    {
        console.error(error);
        setTimeout(startConnection, 5000);
    }
}

document.getElementById("send-button").addEventListener("click", async () => {
    const user = document.getElementById("user-input").value;
    const message = document.getElementById("message-input").value;

    try {
        await connection.invoke("SendMessage", user, message);
    } catch (err) {
        console.error(err);
    }
}); 


function addMessageToList(user, message) {
    const li = document.createElement("li");
    li.textContent = `${user}: ${message}`;
    document.getElementById("message-list").appendChild(li);
}
