var userTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    userTable = $('#tblData').DataTable({

        "ajax": {
            "url": "/Admin/User/GetAll"
        },

        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        },

        "columns": [

            {
                "data": "name",
                "width": "15%"
            },

            {
                "data": "email",
                "width": "15%"
            },

            {
                "data": "phoneNumber",
                "width": "15%"
            },

            {
                "data": "role",
                "width": "15%",
                "render": function (data) {

                    let roleText = "";
                    let roleClass = "";

                    switch (data) {

                        case "Admin":
                            roleText = "Quản trị viên";
                            roleClass = "bg-danger";
                            break;

                        case "Employee":
                            roleText = "Nhân viên";
                            roleClass = "bg-warning text-dark";
                            break;

                        case "Customer":
                            roleText = "Khách hàng";
                            roleClass = "bg-success";
                            break;

                        default:
                            roleText = data;
                            roleClass = "bg-secondary";
                            break;
                    }

                    return `
            <span class="badge ${roleClass}">
                ${roleText}
            </span>
        `;
                }
            },

            {
                "data": {
                    id: "id",
                    lockoutEnd: "lockoutEnd"
                },

                "render": function (data) {

                    var today = new Date().getTime();

                    var lockout = new Date(data.lockoutEnd).getTime();

                    var isLocked = lockout > today;

                    return `
                        <div class="text-end">

                              <a onclick="LockUnlock('${data.id}')"
                                   class="btn btn-sm ${isLocked ? 'btn-success' : 'btn-danger'}">

                                    <i class="bi bi-${isLocked ? 'unlock-fill' : 'lock-fill'}"></i>

                                    ${isLocked ? 'Mở khóa' : 'Khóa'}

                                </a>

                            <a href="/Admin/User/RoleManagment?userId=${data.id}"
                               class="btn btn-sm btn-outline-secondary me-1">

                                <i class="bi bi-person-badge"></i>

                                Vai trò

                            </a>

                            <a href="/Admin/User/ChangePassword?userId=${data.id}"
                               class="btn btn-sm btn-outline-danger">

                                <i class="bi bi-key-fill"></i>

                                Mật khẩu

                            </a>

                        </div>
                    `;
                },

                "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {

    $.ajax({

        type: "POST",

        url: "/Admin/User/LockUnlock",

        data: JSON.stringify(id),

        contentType: "application/json",

        success: function (data) {

            if (data.success) {

                Swal.fire({
                    icon: 'success',
                    title: data.message,
                    confirmButtonColor: '#16a34a'
                }).then(() => {

                    userTable.ajax.reload();
                });
            }
            else {

                Swal.fire({
                    icon: 'error',
                    title: data.message,
                    confirmButtonColor: '#dc2626'
                });
            }
        },

        error: function () {

            Swal.fire({
                icon: 'error',
                title: 'Có lỗi xảy ra',
                confirmButtonColor: '#dc2626'
            });
        }
    });
}