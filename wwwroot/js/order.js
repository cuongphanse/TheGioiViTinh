var DataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("cancelled")) {
        orderDataTable("cancelled");
    } else if (url.includes("completed")) {
        orderDataTable("completed");
    } else if (url.includes("pending")) {
        orderDataTable("pending");
    }else if (url.includes("approved")) {
        orderDataTable("approved");
    } else if (url.includes("processing")) {
        orderDataTable("processing");
    } else if (url.includes("shipped")) {
        orderDataTable("shipped");
    } else {
        orderDataTable("all");
    }
})
function orderDataTable(status) {
    DataTable = $('#tblData').DataTable({

        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },

        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        },

        "columns": [

            {
                "data": "id",
                "width": "10%"
            },

            {
                "data": "name",
                "width": "25%"
            },

            {
                "data": "phoneNumber",
                "width": "10%"
            },

            {
                "data": "applicationUser.email",
                "width": "15%"
            },
            {
                "data": "orderStatus",
                "width": "10%",
                "render": function (data) {

                    const statusMap = {

                        "Pending": {
                            text: "Đang chờ",
                            class: "status-pending"
                        },

                        "Approved": {
                            text: "Đã xác nhận",
                            class: "status-approved"
                        },

                        "Processing": {
                            text: "Đang xử lý",
                            class: "status-processing"
                        },

                        "Shipped": {
                            text: "Đang giao",
                            class: "status-shipped"
                        },

                        "Completed": {
                            text: "Hoàn thành",
                            class: "status-completed"
                        },

                        "Cancelled": {
                            text: "Đã hủy",
                            class: "status-cancelled"
                        }
                    };

                    const status = statusMap[data];

                    return `
                        <span class="status-badge ${status.class}">
                            ${status.text}
                        </span>
                    `;
                }
            },
            {
                "data": "orderTotal",
                "render": function (data) {

                    return data.toLocaleString('vi-VN') + ' đ';
                },
                "width": "15%"
            },

            {
                "data": "id",

                "render": function (data) {

                    return `
                        <div class="text-end">

                            <a href="/Admin/Order/Details?orderId=${data}"
                                class="btn btn-sm btn-outline-success me-1">

                                <i class="bi bi-pencil-square"></i>
                                Chi tiết
                            </a>

                        </div>
                    `;
                },

                "width": "15%"
            }
        ]
    });
}