// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Home Page timer
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

function deleteAuction(id) {
    fetch(`/api/ApiAuction/?id=${id}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Something went wrong')
            }
        })
        .then(() => {
            if (window.location.href.endsWith(id)) {
                window.location.href = 'https://localhost:44319/';
            }
            else {
                window.location.reload()
            }
        });
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

        //TODO: send email when time gets in last 5 min
        //TODO: add 3 minutes to auction end time if a bid is placed last 5 mins
        if (days < 1 && hours < 1 && minutes < 6) {
            let id = document.querySelector(`#timer${i}`).getAttribute('data-auctionId');
            console.log(id);
            deleteAuction(id);
        }

        document.querySelector(`#days${i}`).textContent = days;
        document.querySelector(`#hours${i}`).textContent = hours;
        document.querySelector(`#minutes${i}`).textContent = minutes;
        document.querySelector(`#seconds${i}`).textContent = seconds;
    }
}

setInterval(countDown, 1000);


//Loading Subcategories
function getSubCategories() {
    let selected = document.querySelectorAll("div .list")[0];
    let categoryId = Number.parseInt(selected.querySelector('.selected').getAttribute("data-value"));
    let ul = document.querySelectorAll("div .list")[1];

    if (!Number.isInteger(categoryId) || categoryId == 0) {
        ul.parentElement.parentElement.parentElement.style.display = 'none';
        return;
    }

    fetch(`/api/SubCategory/?id=${categoryId}`, {
        method: 'get'
    })
        .then(response => response.json())
        .then(subCategories => {

            ul.parentElement.parentElement.parentElement.style.display = 'block';

            for (let i = 0; i < subCategories.length; i++) {

                let liElement = document.createElement("li");
                liElement.classList.add("option");
                liElement.setAttribute("data-value", subCategories[i].id)
                liElement.textContent = subCategories[i].name
                ul.appendChild(liElement)
            }
        })

    ul.innerHTML = '';
    document.querySelectorAll('.current')[1].textContent = '';
}

function getSubCategoryId() {
    let selected = document.querySelectorAll("div .list")[1];
    let subCategoryId = Number.parseInt(selected.querySelector('.selected').getAttribute("data-value"));

    let optionElement = selected.parentElement.parentElement.getElementsByTagName('option')[0];
    optionElement.setAttribute('value', subCategoryId);
    optionElement.classList.add('selected');
}