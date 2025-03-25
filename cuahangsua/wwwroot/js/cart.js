$(document).ready(function () {
    updateCartCount(); // Cập nhật số lượng giỏ hàng khi tải trang

    function updateCartCount() {
        $.get("/Cart/GetCartCount", function (data) {
            $("#cartCount").text(data.count);
        });
    }

    $(".add-to-cart-btn").on("click", function (e) {
        e.preventDefault();
        let productId = $(this).data("id");

        $.post("/Cart/AddToCartAjax", { id: productId }, function (data) {
            if (data.success) {
                updateCartCount();
                Swal.fire({
                    icon: 'success',
                    title: 'Thêm vào giỏ hàng thành công!',
                    showConfirmButton: false,
                    timer: 1500
                });
            } else {
                Swal.fire('Lỗi!', 'Không thể thêm sản phẩm vào giỏ hàng!', 'error');
            }
        });
    });

    $(".update-qty").on("change", function () {
        let productId = $(this).data("id");
        let newQuantity = parseInt($(this).val());

        if (newQuantity < 1) {
            Swal.fire('Cảnh báo!', 'Số lượng không hợp lệ!', 'warning');
            return;
        }

        $.post("/Cart/UpdateQuantityAjax", { id: productId, quantity: newQuantity }, function (data) {
            if (data.success) {
                updateCartCount();
                $("#totalPrice").text(data.totalPrice);
                $("#itemTotal-" + productId).text(data.itemTotal);
            } else {
                Swal.fire('Lỗi!', 'Không thể cập nhật số lượng!', 'error');
            }
        });
    });

    $(".remove-item").on("click", function () {
        let productId = $(this).data("id");

        Swal.fire({
            title: 'Xóa sản phẩm?',
            text: 'Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post("/Cart/RemoveAjax", { id: productId }, function (data) {
                    if (data.success) {
                        updateCartCount();
                        location.reload();
                    } else {
                        Swal.fire('Lỗi!', 'Không thể xóa sản phẩm!', 'error');
                    }
                });
            }
        });
    });

    

        $.post("/Cart/CheckoutAjax", orderData, function (data) {
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: '🛍 Thanh Toán Thành Công!',
                    text: 'Cảm Ơn Quý Khách Đã Ghé Thăm Shop Em! 🎉',
                    showConfirmButton: false,
                    timer: 3000
                }).then(() => {
                    updateCartCount();
                    window.location.href = "/Home/Index";
                });
            } else {
                Swal.fire('Lỗi!', data.message || 'Không thể thanh toán!', 'error');
            }
        });
    });
});
