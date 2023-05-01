// chatUi.js
import connection from "./signalrConnection.js";
import {
    createChatTab,
    createChatContentContainer,
    getChatHistory,
    updateChatContainer,
} from "./chatMessages.js";

async function switchToChat(userId) {
    // ...
    const chatHistory = await getChatHistory(currentUserId, userId);
    updateChatContainer(userId, chatHistory);
}

export function initChatUI(users, currentUserId) {
    users.forEach((user) => {
        if (user.Id !== currentUserId) {
            createChatTab(user.Id, user.UserName, user.ImagePath);
            (async function () {
                const chatHistory = await getChatHistory(currentUserId, user.Id);
                createChatContentContainer(user.Id, chatHistory);
            })();
        }
    });
}
