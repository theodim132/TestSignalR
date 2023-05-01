// chatMessages.js
import connection from "./signalrConnection.js";

export async function getChatHistory(currentUser, otherUser) {
    // ...
}

export function updateChatContainer(userId, chatHistory) {
    // ...
}

export function createChatTab(userId, userName, userImage) {
    // ...
}

export function createChatContentContainer(userId, chatHistory) {
    // ...
}

export async function sendMessage(targetUserId) {
    // ...
}
