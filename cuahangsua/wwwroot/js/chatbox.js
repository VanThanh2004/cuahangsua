document.addEventListener("DOMContentLoaded", function () {
    const chatbox = document.getElementById("chatbox-container");
    const chatToggle = document.getElementById("chatbox-toggle");
    const chatClose = document.getElementById("chatbox-close");
    const chatMessages = document.getElementById("chatbox-messages");
    const chatInput = document.getElementById("chatbox-input");
    const sendButton = document.getElementById("chatbox-send");

    // Mở chatbot khi bấm vào icon
    chatToggle.addEventListener("click", function () {
        chatbox.style.display = "block";
        chatToggle.style.display = "none";
    });

    // Đóng chatbot khi bấm X
    chatClose.addEventListener("click", function () {
        chatbox.style.display = "none";
        chatToggle.style.display = "flex";
    });

    // Gửi tin nhắn
    function sendMessage() {
        const message = chatInput.value.trim();
        if (message === "") return;

        // Hiển thị tin nhắn của người dùng
        chatMessages.innerHTML += `<p><strong>Bạn:</strong> ${message}</p>`;
        chatInput.value = "";

        // Gửi câu hỏi lên API chatbot
        fetch(`/api/chatbot/ask?question=${encodeURIComponent(message)}`)
            .then(response => response.json())
            .then(data => {
                chatMessages.innerHTML += `<p><strong>Bot:</strong> ${data.botResponse}</p>`;
                chatMessages.scrollTop = chatMessages.scrollHeight; // Cuộn xuống tin nhắn mới nhất
            })
            .catch(error => {
                chatMessages.innerHTML += `<p><strong>Bot:</strong> Xin lỗi, có lỗi xảy ra khi xử lý yêu cầu.</p>`;
                console.error("Lỗi chatbot:", error);
            });
    }

    sendButton.addEventListener("click", sendMessage);
    chatInput.addEventListener("keypress", function (event) {
        if (event.key === "Enter") sendMessage();
    });
});
