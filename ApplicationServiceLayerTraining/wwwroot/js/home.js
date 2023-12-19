var time = document.querySelector("#time");

function ShowTime() {
    time.innerHTML = new Date().toLocaleTimeString();
}

setInterval(ShowTime, 1000);