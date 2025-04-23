
import { login } from "./login.js";
import { setupConnection, startConnection } from "./chat.js";

async function main() {
    try {
        const email = "test@test.com";
        const password = "123abcZYX...";

        const token = await login(email, password);

        if(!token) {
            throw new Error("Failed to get JWT");
        }

        setupConnection(token);

        await startConnection();

        console.log("ready to chat");
    }
    catch (error)
    {
        console.error(error);
    } 
}
