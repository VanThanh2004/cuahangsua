document.addEventListener("DOMContentLoaded", function () {
    const searchBox = document.getElementById("searchInput");
    const searchResults = document.getElementById("searchResults");

    // Xử lý khi nhập từ khóa
    searchBox.addEventListener("input", function () {
        let query = this.value.trim();

        if (query.length > 1) {
            fetch(`/Home/GetProducts?search=${query}`)
                .then(response => response.json())
                .then(data => {
                    if (data.length === 0) {
                        searchResults.style.display = "none";
                        return;
                    }

                    let html = data.map(product => `
                        <div class="search-item">
                            <a href="/Product/Details/${product.id}">
                                <img src="${product.imageUrl}" alt="${product.name}">
                                <span>${product.name}</span>
                            </a>
                        </div>
                    `).join("");

                    searchResults.innerHTML = html;
                    searchResults.style.display = "block";
                });
        } else {
            searchResults.style.display = "none";
        }
    });

    // Ẩn gợi ý khi click ra ngoài
    document.addEventListener("click", function (event) {
        if (!searchBox.contains(event.target) && !searchResults.contains(event.target)) {
            searchResults.style.display = "none";
        }
    });

    // Xử lý khi nhấn Enter
    searchBox.addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();
            window.location.href = `/Home/Search?query=${encodeURIComponent(searchBox.value.trim())}`;
        }
    });
});
