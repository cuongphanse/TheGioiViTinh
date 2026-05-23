$.getJSON('/Admin/Dashboard/GetChartData', function (data) {

    // ===== BIỂU ĐỒ DOANH THU =====

    new Chart(

        document
            .getElementById('revenueChart')
            .getContext('2d'),

        {

            type: 'bar',

            data: {

                labels: data.monthlyRevenue.map(r => r.label),

                datasets: [{

                    label: 'Doanh thu',

                    data: data.monthlyRevenue.map(r => r.revenue),

                    borderWidth: 1,

                    borderRadius: 8
                }]
            },

            options: {

                responsive: true,

                maintainAspectRatio: false,

                scales: {

                    y: {

                        beginAtZero: true,

                        ticks: {

                            callback: function (value) {

                                return value.toLocaleString('vi-VN') + 'đ';
                            }
                        }
                    }
                },

                plugins: {

                    tooltip: {

                        callbacks: {

                            label: function (context) {

                                return context.raw.toLocaleString('vi-VN') + 'đ';
                            }
                        }
                    }
                }
            }
        });

    // ===== BIỂU ĐỒ ĐƠN HÀNG =====

    new Chart(

        document
            .getElementById('orderChart')
            .getContext('2d'),

        {

            type: 'line',

            data: {

                labels: data.monthlyOrders.map(r => r.label),

                datasets: [{

                    label: 'Số đơn hàng',

                    data: data.monthlyOrders.map(r => r.count),

                    borderWidth: 2,

                    tension: 0.3
                }]
            },

            options: {

                responsive: true,

                maintainAspectRatio: false,

                scales: {

                    y: {

                        beginAtZero: true
                    }
                }
            }
        });

});