(() => {
    const connectionStatus = document.getElementById("dashboardConnectionStatus");

    if (!connectionStatus || typeof signalR === "undefined") {
        return;
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(connectionStatus.dataset.hubUrl)
        .withAutomaticReconnect([0, 2000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Warning)
        .build();

    const updateText = (elementId, value) => {
        const element = document.getElementById(elementId);

        if (element) {
            element.textContent = value ?? "-";
        }
    };

    const setConnectionStatus = (message, cssClass) => {
        connectionStatus.textContent = message;
        connectionStatus.className = cssClass;
    };

    const updateDashboard = (statistics) => {
        updateText("productCount", statistics.productCount);
        updateText("categoryCount", statistics.categoryCount);
        updateText("brandCount", statistics.brandCount);
        updateText("userCount", statistics.userCount);
        updateText("totalCommentCount", statistics.totalCommentCount);
        updateText("approvedCommentCount", statistics.approvedCommentCount);
        updateText("pendingCommentCount", statistics.pendingCommentCount);
        updateText("totalMessageCount", statistics.totalMessageCount);
        updateText("discountCouponCount", statistics.discountCouponCount);
        updateText(
            "averageProductPrice",
            `${new Intl.NumberFormat("tr-TR", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
                .format(statistics.averageProductPrice ?? 0)} ₺`);
        updateText(
            "mostExpensiveProductName",
            statistics.mostExpensiveProductName || "Ürün bulunmuyor.");
        updateText(
            "leastExpensiveProductName",
            statistics.leastExpensiveProductName || "Ürün bulunmuyor.");
    };

    const requestDashboardStatistics = async () => {
        try {
            await connection.invoke("RequestDashboardStatisticsAsync");
        } catch {
            setConnectionStatus("Canlı istatistikler alınamadı.", "text-warning");
        }
    };

    connection.on("DashboardConnected", async () => {
        setConnectionStatus("Canlı bağlantı kuruldu.", "text-success");
        await requestDashboardStatistics();
    });

    connection.on("DashboardStatisticsUpdated", updateDashboard);

    connection.onreconnecting(() => {
        setConnectionStatus("Canlı bağlantı yeniden kuruluyor…", "text-warning");
    });

    connection.onreconnected(async () => {
        setConnectionStatus("Canlı bağlantı yeniden kuruldu.", "text-success");
        await requestDashboardStatistics();
    });

    connection.onclose(() => {
        setConnectionStatus("Canlı bağlantı kapandı.", "text-danger");
    });

    const startConnection = async () => {
        try {
            await connection.start();
        } catch {
            setConnectionStatus("Canlı bağlantı kurulamadı, yeniden denenecek.", "text-warning");
            window.setTimeout(startConnection, 5000);
        }
    };

    window.setInterval(() => {
        if (connection.state === signalR.HubConnectionState.Connected) {
            void requestDashboardStatistics();
        }
    }, 30000);

    void startConnection();
})();
