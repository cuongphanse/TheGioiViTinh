let dataTable;

$(document).ready(function () {

    loadDataTable();

});

function loadDataTable() {

    dataTable = $('#tblData').DataTable({

        "ajax": {

            "url": "/Admin/Discount/GetAll"

        },

        "language": {

            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"

        },

        "columns": [

            {
                "data": "name",
                "width": "25%"
            },

            {
                "data": "discountPercent",

                "render": function (data) {

                    return `
                        <span class="badge bg-danger">
                            ${data}%
                        </span>
                    `;
                },

                "width": "10%"
            },

            {
                "data": "startDate",

                "render": function (data) {

                    return new Date(data)
                        .toLocaleDateString('vi-VN');
                },

                "width": "15%"
            },

            {
                "data": "endDate",

                "render": function (data) {

                    return new Date(data)
                        .toLocaleDateString('vi-VN');
                },

                "width": "15%"
            },

            {
                "data": "isActive",

                "render": function (data) {

                    if (data) {

                        return `
                            <span class="badge bg-success">
                                Đang hoạt động
                            </span>
                        `;
                    }

                    return `
                        <span class="badge bg-secondary">
                            Đã tắt
                        </span>
                    `;
                },

                "width": "15%"
            },

            {
                "data": "id",

                "render": function (data) {

                    return `
                        <div class="text-end">

                            <a href="/Admin/Discount/Update?id=${data}"
                               class="btn btn-sm btn-outline-success me-1">

                                <i class="bi bi-pencil-square"></i>
                                Sửa
                            </a>

                            <a onclick="Delete('/Admin/Discount/Delete/${data}')"
                               class="btn btn-sm btn-outline-danger">

                                <i class="bi bi-trash"></i>
                                Xóa
                            </a>

                        </div>
                    `;
                },

                "width": "20%"
            }
        ]
    });
}

function Delete(url) {

    Swal.fire({

        title: "Bạn chắc chắn?",

        text: "Khuyến mãi sẽ bị xóa!",

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

                            dataTable.ajax.reload();

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