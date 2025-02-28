const ctx = document.getElementById('categoryLineChart').getContext('2d');

const primaryColor = localStorage.getItem('primaryColor');
const selectedTheme = localStorage.getItem('theme');

let fontColor = (selectedTheme === "dark") ? "#fff" : "#444";



const data = {
    labels: ['LC', 'CC', 'RC', 'RM', 'MC', 'MS', 'PRES', 'CF', 'ERC', 'SC', 'DR'],
    datasets: [{
        label: 'Cases Filed',
        data: [250, 130, 220, 70, 150, 100, 50, 150, 200, 150, 60],
        borderColor: primaryColor,
        backgroundColor: 'rgba(255, 51, 102, 0.2)',
        pointBackgroundColor: '#ffffff',
        pointBorderColor: primaryColor,
        pointRadius: 5,
        pointHoverRadius: 7,
        tension: 0
    }]
};

const config = {
    type: 'line',
    data: data,
    options: {
        responsive: true,
        plugins: {
            legend: {
                display: false,
                labels: {
                    color: fontColor
                }
            },
            tooltip: {
                titleColor: fontColor,
                bodyColor: fontColor,
                callbacks: {
                    label: function (context) {
                        return `Cases: ${context.raw}`;
                    }
                }
            }
        },
        scales: {
            x: {
                ticks: {
                    color: fontColor
                },
                grid: {
                    color: selectedTheme === "dark" ? "#444" : "#ddd"
                }
            },
            y: {
                ticks: {
                    color: fontColor
                },
                grid: {
                    color: selectedTheme === "dark" ? "#444" : "#ddd"
                }
            }
        }

    }
};

new Chart(ctx, config);
