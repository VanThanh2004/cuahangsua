document.addEventListener("DOMContentLoaded", function () {
    const searchBox = document.getElementById("search-box");
    const searchForm = document.getElementById("search-form");
    const clearBtn = document.getElementById("clear-btn");
    const suggestionsBox = document.getElementById("search-suggestions");

    // 1️⃣ Hiện nút xóa khi nhập
    searchBox.addEventListener("input", function () {
        clearBtn.style.display = this.value.length > 0 ? "inline-block" : "none";

        // Gợi ý sản phẩm từ server
        if (this.value.length > 0) {
            fetch(`/Store/GetProducts?search=${this.value}`)
                .then(response => response.text())
                .then(data => {
                    suggestionsBox.innerHTML = data;
                    suggestionsBox.style.display = data.trim() ? "block" : "none";
                });
        } else {
            suggestionsBox.style.display = "none";
        }
    });

    // 2️⃣ Xóa nội dung tìm kiếm
    clearBtn.addEventListener("click", function () {
        searchBox.value = "";
        clearBtn.style.display = "none";
        suggestionsBox.style.display = "none";
    });

    // 3️⃣ Xử lý tìm kiếm khi nhấn Enter hoặc kính lúp
    searchForm.addEventListener("submit", function (event) {
        event.preventDefault(); // Không tải lại trang

        const query = searchBox.value.trim();
        if (query) {
            window.location.href = `/Store/Index?query=${encodeURIComponent(query)}`;
        }
    });

    // 4️⃣ Ẩn gợi ý khi click ngoài
    document.addEventListener("click", function (event) {
        if (!searchBox.contains(event.target) && !suggestionsBox.contains(event.target)) {
            suggestionsBox.style.display = "none";
        }
    });
    $(document).ready(function () {
        $("#searchInput").on("keyup", function () {
            let keyword = $(this).val().trim();
            if (keyword.length < 2) {
                $("#searchResults").hide();
                return;
            }

            $.ajax({
                url: "/Product/Search",
                type: "GET",
                data: { keyword: keyword },
                success: function (data) {
                    if (data.length === 0) {
                        $("#searchResults").hide();
                        return;
                    }

                    let resultsHtml = data.map(product => `
                    <div class="search-item">
                        <a href="/Product/Details/${product.id}">
                            <img src="${product.imageUrl}" alt="${product.name}" />
                            <span>${product.name}</span>
                        </a>
                    </div>
                `).join("");

                    $("#searchResults").html(resultsHtml).show();
                }
            });
        });

        $(document).on("click", function (e) {
            if (!$(e.target).closest(".search-bar").length) {
                $("#searchResults").hide();
            }
        });
    });

});
