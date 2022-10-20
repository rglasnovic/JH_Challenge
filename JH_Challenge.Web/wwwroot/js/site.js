var countLabel = document.getElementById("count-label");
var hashtagCount;
var hashtagList = document.getElementById("hashtag-list");
var monitorButton = document.getElementById("monitor-button");
var statusLabel = document.getElementById("status-label");

document.addEventListener("DOMContentLoaded", () => {

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/twitter")
        .build();

    connection.on("StartMonitoring", () => {
        monitorButton.setAttribute("data-state", "on");
        monitorButton.innerHTML = "Stop Monitoring";
        var startDate = new Date();
        statusLabel.innerHTML = `Started at ${startDate.toLocaleTimeString()} on ${startDate.toLocaleDateString()}`;
    });

    connection.on("StopMonitoring", () => {
        monitorButton.setAttribute("data-state", "off");
        monitorButton.innerHTML = "Start Monitoring";
        var stopDate = new Date();
        statusLabel.innerHTML = `Stopped at ${stopDate.toLocaleTimeString()} on ${stopDate.toLocaleDateString()}`;
    });

    connection.on("Update", (count, hashtags) => {
        countLabel.innerHTML = count.toLocaleString('en', { useGrouping: true });
        hashtagList.innerHTML = "";
        hashtagCount = 0;
        hashtags.forEach(addHashtag);
    });

    monitorButton.addEventListener("click", async (event) => {
        try {
            if (event.target.getAttribute('data-state') == "off") {
                countLabel.innerHTML = "";
                hashtagList.innerHTML = "";
                await connection.invoke("StartMonitoring");
            }
            else
                await connection.invoke("StopMonitoring");
        } catch (err) {
            console.error(err);
        }
    });

    function addHashtag(hashtag) {
        hashtag.link = `https://twitter.com/search?q=%23${hashtag.key}`
        const li = document.createElement("li");
        li.innerHTML = `<a href='${hashtag.link}' target='_blank'>${hashtag.key} <span class='count'>${hashtag.value} ${hashtag.value == 1 ? 'tweet' : 'tweets'}</span></a>`;
        hashtagList.appendChild(li);
    }

    async function start() {
        try {
            await connection.start();
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });

    // Start the connection.
    start();
});