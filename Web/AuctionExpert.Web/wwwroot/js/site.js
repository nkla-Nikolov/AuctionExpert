// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getTimers() {
    let index = 0;
    let timerElements = [];
    let timer = document.querySelector(`#timer${index}`)

    while (timer != null) {
        timerElements.push(timer)
        index++;
        timer = document.querySelector(`#timer${index}`);
    }

    return timerElements;
}

function countDown() {

    let timers = getTimers();

    if (timers.length == 0) {
        return;
    }

    for (let i = 0; i < timers.length; i++) {

        let date = document.querySelector(`#timer${i}`).getAttribute('data-enddate');
        let endDate = new Date(date).getTime();
        let now = new Date().getTime();

        let difference = endDate - now;
        let second = 1000;
        let minute = second * 60;
        let hour = minute * 60;
        let day = hour * 24;

        let days = Math.floor(difference / day);
        let hours = Math.floor((difference % day) / hour)
        let minutes = Math.floor((difference % hour) / minute)
        let seconds = Math.floor((difference % minute) / second)

        if (days < "10") {
            days = "0" + days;
        }
        if (hours < "10") {
            hours = "0" + hours;
        }
        if (minutes < "10") {
            minutes = "0" + minutes;
        }
        if (seconds < "10") {
            seconds = "0" + seconds;
        }

        document.querySelector(`#days${i}`).textContent = days;
        document.querySelector(`#hours${i}`).textContent = hours;
        document.querySelector(`#minutes${i}`).textContent = minutes;
        document.querySelector(`#seconds${i}`).textContent = seconds;
    }
}

setInterval(countDown, 1000);