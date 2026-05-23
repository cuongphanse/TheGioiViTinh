
$('#tblData').DataTable({

        "ajax": {
            "url": "/Admin/Product/GetAll"
        },

        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        },

        "columns": [

            {
                "data": "mainImageUrl",
                "render": function (data) {

                    return `
                        <img src="${data}"
                                class="rounded border"
                                style="width:70px;height:70px;object-fit:cover;" />
                    `;
                },
                "width": "10%"
            },

            {
                "data": "name",
                "width": "25%"
            },

            {
                "data": "category.name",
                "width": "15%"
            },

            {
                "data": "brand.name",
                "width": "15%"
            },

            {
                "data": "price",
                "render": function (data) {

                    return data.toLocaleString('vi-VN') + ' đ';
                },
                "width": "10%"
            },

            {
                "data": "stockQuantity",
                "width": "10%"
            },

            {
                "data": "id",

                "render": function (data) {

                    return `
                        <div class="text-end">

                            <a href="/Admin/Product/Update?id=${data}"
                                class="btn btn-sm btn-outline-success me-1">

                                <i class="bi bi-pencil-square"></i>
                                Sửa
                            </a>

                            <a onclick="Delete('/Admin/Product/Delete/${data}')"
                                class="btn btn-sm btn-outline-danger">

                                <i class="bi bi-trash"></i>
                                Xóa
                            </a>

                        </div>
                    `;
                },

                "width": "15%"
            }
        ]
    });

function Delete(url) {

    Swal.fire({
        title: "Bạn chắc chắn?",
        text: "Dữ liệu sẽ bị xóa!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Xóa",
        cancelButtonText: "Hủy"
    })
        .then((result) => {

            if (result.isConfirmed) {

                $.ajax({

                    url: url,
                    type: "DELETE",
                    success: function (data) {

                        if (data.success) {

                            $('#tblData')
                                .DataTable()
                                .ajax.reload();// o day reload lai du lieu sau khi xoa

                            Swal.fire({
                                title: "Đã xóa!",
                                text: data.message,
                                icon: "success"
                            });
                        }
                        else {

                            Swal.fire({
                                title: "Lỗi!",
                                text: data.message,
                                icon: "error"
                            });
                        }
                    }
                });
            }
        });
}