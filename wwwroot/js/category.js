$('#tblData').DataTable({
    "ajax": {
        "url": "/Admin/category/getall"
    },
    "language": {
        "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
    },
    "columns": [
        { "data": "name", "width": "40%" },
        { "data": "displayOrder", "width": "30%" },
        {
            "data": "id",
            "render": function (data) {
                return `
                        <div class="text-end">
                            <a href="/Admin/Category/Update?id=${data}" class="btn btn-sm btn-outline-success me-1">
                                <i class="bi bi-pencil-square"></i> Sửa
                            </a>
                            <a onclick="Delete('/Admin/Category/Delete/${data}')"
                                class="btn btn-sm btn-outline-danger">
                                <i class="bi bi-trash"></i> Xóa
                            </a>
                        </div>
                    `;
            },
            "width": "30%"
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
                                .ajax.reload();

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