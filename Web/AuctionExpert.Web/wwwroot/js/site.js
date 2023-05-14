// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    setInterval(countDown, 1000);
    
    $('.like i').click(likeAuctionHandler);
    $('#loadSubCategories').change(getSubCategories);
    $('#loadSubCategoryId').change(getSubCategoryId);
    $('#adminCountriesPerPage').change(adminCountriesPerPage);
});

function adminCountriesPerPage(e) {
    e.preventDefault();

    let itemsPerPage = Number($(e.currentTarget).children('.nice-select').children('.list').children('.selected').attr('data-value'));
    
    let postData = {
        itemsPerPage,
        currentPage: 1
    };

    loadCountries(postData);
}

function loadCountries(postData) {
    if (postData == null) {
        postData = {
            itemsPerPage: 10,
            currentPage: 1
        };
    }

    $.ajax({
        type: 'GET',
        url: '/Administration/Countries/GetAll',
        data: postData,
        success: function (data) {
            $('#countries-table tbody').empty();
            $('.pagination').empty();

            updateTable(data.countries);
            pagination(data);
        }
    });
};

function pagination(data) {
    let ul = $('#adminCountriesPagination');
    let fragment = $(document.createDocumentFragment());

    //prev element
    let prevElement = $(document.createElement('li'));
    $(prevElement).addClass('page-item');
    let prevLink = $(document.createElement('a'));
    $(prevLink).addClass('page-link');
    $(prevLink).text('Prev');

    if (!data.hasPreviousPage) {
        $(prevElement).addClass('disabled');
    }
    else {
        $(prevLink).attr('data-pageNumber', data.currentPage - 1);
    }
    $(prevElement).append(prevLink);
    $(fragment).append(prevElement);

    //previous pages
    for (let i = data.currentPage - 3; i < data.currentPage; i++) {
        if (i > 0) {
            let prevPageElement = $(document.createElement('li'));
            $(prevPageElement).addClass('page-item');
            let prevPageLink = $(document.createElement('a'));
            $(prevPageLink).addClass('page-link');
            $(prevPageLink).text(i);
            $(prevPageElement).append(prevPageLink);

            $(fragment).append(prevPageElement);
        }
    }

    //current page
    let currentPageElement = $(document.createElement('li'));
    $(currentPageElement).addClass('page-item', 'active');
    $(currentPageElement).attr('aria-current', 'page');

    let currentPageLink = $(document.createElement('a'));
    $(currentPageLink).addClass('page-link');
    $(currentPageLink).text(data.currentPage);
    $(currentPageElement).append(currentPageLink);

    $(fragment).append(currentPageElement);

    //next pages
    for (let i = data.currentPage + 1; i <= data.currentPage + 2; i++) {
        if (i <= data.pagesCount) {
            let nextPageElement = $(document.createElement('li'));
            $(nextPageElement).addClass('page-item');

            let nextPageLink = $(document.createElement('a'));
            $(nextPageLink).addClass('page-link');
            $(nextPageLink).text(i);
            $(nextPageElement).append(nextPageLink);

            $(fragment).append(nextPageElement);
        }
    }

    //next element
    let nextElement = $(document.createElement('li'));
    $(nextElement).addClass('page-item');

    let nextLink = $(document.createElement('a'));
    $(nextLink).addClass('page-link');
    $(nextLink).text('Next');

    if (!data.hasNextPage) {
        $(nextElement).addClass('disabled');
    }
    else {
        $(nextLink).attr('data-pageNumber', data.currentPage + 1);
    }
    $(nextElement).append(nextLink);

    $(fragment).append(nextElement);
    $(ul).append(fragment);

    //events for page numbers
    let pageElements = $('.page-item');

    $(pageElements).each(function (index, element) {
        $(element).click(function (e) {
            e.preventDefault();

            if (!$(e.currentTarget).hasClass('disabled')) {
                let pageNumber = Number($(e.currentTarget).children().text())

                if (isNaN(pageNumber)) {
                    pageNumber = Number($(e.currentTarget).children().first().attr('data-pagenumber'));
                }

                let itemsPerPage = Number($('#adminCountriesPerPage').children('.nice-select').children('.list').children('.selected').attr('data-value'));
                let postData = {
                    currentPage: pageNumber,
                    itemsPerPage
                };

                loadCountries(postData);
            }
        });
    });
};

function updateTable(countries) {
    let fragment = $(document.createDocumentFragment());

    $(countries).each(function (index, country) {

        let row = $(document.createElement('tr'));

        let tdId = $(document.createElement('td'));
        $(tdId).attr('data-label', 'Country ID');
        $(tdId).text(country.id);

        let tdName = $(document.createElement('td'));
        $(tdName).attr('data-label', 'Country name');
        $(tdName).text(country.name);

        let tdCitiesCount = $(document.createElement('td'));
        $(tdCitiesCount).attr('data-label', 'Cities count');
        $(tdCitiesCount).text(country.citiesCount);

        let tdDeleteButton = $(document.createElement('td'));
        $(tdDeleteButton).attr('data-label', 'Delete');
        let deleteButton = $(document.createElement('button'));
        $(deleteButton).text('Delete');
        $(deleteButton).addClass(['btn', 'btn-sm', 'btn-outline-danger']);
        $(deleteButton).prop('id', `delete-country_${country.id}`);

        $(deleteButton).click(deleteButtonHandler);
        $(tdDeleteButton).append(deleteButton);

        let tdEditButton = $(document.createElement('td'));
        $(tdEditButton).attr('data-label', 'Edit');
        let editButton = $(document.createElement('button'));
        $(editButton).text('Edit');
        $(editButton).addClass(['btn', 'btn-sm', 'btn-outline-warning']);
        $(editButton).prop('id', `edit-country_${country.id}`);

        $(editButton).click(editButtonHandler);
        $(tdEditButton).append(editButton);

        $(row).append(tdId);
        $(row).append(tdName);
        $(row).append(tdCitiesCount);
        $(row).append(tdDeleteButton);
        $(row).append(tdEditButton);

        $(fragment).append(row);
    });

    $('#countries-table tbody').append(fragment);
};

function deleteButtonHandler(e) {
    let country = $(e.target).attr('id');
    let indexOfId = country.indexOf('_')
    let countryId = Number(country.substring(indexOfId + 1));
    let token = $('#antiForgeryForm input').val();

    let postData = {
        countryId
    };

    $.ajax({
        type: 'POST',
        url: '/Administration/Countries/Delete',
        data: postData,
        headers: {
            'X-CSRF-TOKEN': token
        },
        success: function (response) {
            if (!response.result) {
                alert('Country does not exist!')
            }
            else {
                $(e.target).parent().parent().remove();
            }
        },
        error: function (response) {
            alert('Request failed. Please try again')
        }
    });
};

function editButtonHandler(e) {
    $(e.target).attr('disabled', 'disabled');
    let country = $(e.target).attr('id');
    let indexOfId = country.indexOf('_')
    let countryId = Number(country.substring(indexOfId + 1));
    let token = $('#antiForgeryForm input').val();

    let tdElement = $(this).parent().parent().children()[1];
    let currentCountryName = $(tdElement).text();
    $(tdElement).text('');
    let div = $(document.createElement('div'));
    $(div).addClass('d-flex');
    $(div).css('margin', 'auto');

    let input = $(document.createElement('input'));
    $(input).addClass('form-control');
    $(input).val(currentCountryName);

    let updateBtn = $(document.createElement('button'));
    $(updateBtn).addClass(['btn', 'btn-sm', 'btn-outline-success']);
    $(updateBtn).css('margin-left', '20px');
    $(updateBtn).text('Update');

    let cancelBtn = $(document.createElement('button'));
    $(cancelBtn).addClass(['btn', 'btn-sm', 'btn-outline-danger']);
    $(cancelBtn).css('margin-left', '20px');
    $(cancelBtn).text('Cancel');

    $(div).append(input);
    $(div).append(updateBtn);
    $(div).append(cancelBtn);
    $(tdElement).append(div);

    $(updateBtn).click(function (e) {
        e.preventDefault();

        let inputValue = $(this).parent().children()[0];
        let countryName = $(inputValue).val();

        let data = {
            countryId,
            countryName
        };

        $.ajax({
            type: 'POST',
            url: '/Administration/Countries/Update',
            data: data,
            headers: {
                'X-CSRF-TOKEN': token
            },
            success: function (response) {
                if (!response.result) {
                    alert(response.message);
                    $(cancelBtn).trigger('click');
                }
                else {
                    $(cancelBtn).trigger('click');
                    tdElement.textContent = countryName;
                }
            },
            error: function (response) {
                alert('Request failed. Please try again');
                $(cancelBtn).trigger('click');
            }
        });
    });

    $(cancelBtn).click(function () {
        $(tdElement).empty();
        $(tdElement).text(currentCountryName);
        $(e.target).removeAttr('disabled');
    });
};

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
    var token = $('#antiForgeryForm input').val();

    $.ajax({
        type: 'POST',
        data: { id },
        url: '/Auction/Delete',
        headers: {
            'X-CSRF-TOKEN': token
        },
        success: function (response) {
            if (response.success && location.href.endsWith(id)) {
                location.href = '/Home/Index';
            }
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

        if (days < 1 && hours < 1 && minutes < 6) {
            let id = document.querySelector(`#timer${i}`).getAttribute('data-auctionId');

            deleteAuction(id);
        }

        document.querySelector(`#days${i}`).textContent = days;
        document.querySelector(`#hours${i}`).textContent = hours;
        document.querySelector(`#minutes${i}`).textContent = minutes;
        document.querySelector(`#seconds${i}`).textContent = seconds;
    }
}

//Loading Subcategories
function getSubCategories(e) {
    e.preventDefault();

    let categoryId = Number($(e.currentTarget).children('.nice-select').children('.list').children('.selected').attr('data-value'));
    $('#loadSubCategoryId').children('.nice-select').children('.list').empty();

    if (!Number.isInteger(categoryId)) {

        $('#loadSubCategoryId').parent().css('display', 'none');
        return;
    }

    $.ajax({
        type: 'get',
        url: '/SubCategory/GetSubCategoriesByCategoryId',
        data: { categoryId },
        success: function (response) {
            if (response.success) {
                let fragment = $(document.createDocumentFragment());

                $(response.subCategories).each(function (index, subCategory) {
                    let liElement = $(document.createElement('li'));
                    $(liElement).addClass('option');
                    $(liElement).attr('data-value', subCategory.id);
                    $(liElement).text(subCategory.name);

                    fragment.append(liElement);
                });

                $('#loadSubCategoryId').children('.nice-select').children('.list').append(fragment);
                $('#loadSubCategoryId').parent().css('display', 'block');
            }
            else {
                alert(response.errorMessage);
            }
        },
        error: function () {
            alert('Request failed. Please try again later');
        }
    });
}

function getSubCategoryId(e) {
    e.preventDefault();

    let subCategoryId = Number($(e.currentTarget).children('.nice-select').children('.list').children('.selected').attr('data-value'));
    $(e.currentTarget).children('select').children().val(subCategoryId);
}

//Auction details modal 
function openModal() {
    document.getElementById("myModal").style.display = "block";
    document.querySelector('body').style.overflow = 'hidden';
}

function closeModal() {
    document.getElementById("myModal").style.display = "none";
    document.querySelector('body').style.overflow = 'visible';
}

var slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
    showSlides(slideIndex += n);
}

function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
    captionText.innerHTML = dots[slideIndex - 1].alt;
}function openModal() {
    document.getElementById("myModal").style.display = "block";
    document.querySelector('body').style.overflow = 'hidden';
}

function closeModal() {
    document.getElementById("myModal").style.display = "none";
    document.querySelector('body').style.overflow = 'visible';
}

var slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
    showSlides(slideIndex += n);
}

function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");

    if (n > slides.length) {
        slideIndex = 1
    }
    if (n < 1) {
        slideIndex = slides.length
    }
    for (let i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (let i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }

    if (slides[slideIndex - 1] != undefined) {
        slides[slideIndex - 1].style.display = "block";
    }
    if (dots[slideIndex - 1] != undefined) {
        dots[slideIndex - 1].className += " active";
        captionText.innerHTML = dots[slideIndex - 1].alt;
    }
}

function likeAuction(postData, token, e) {
    $.ajax({
        type: 'POST',
        url: '/Auction/LikeAuction',
        data: postData,
        headers: {
            'X-CSRF-TOKEN': token
        },
        success: function (response) {
            if (response.result == true) {
                e.target.classList.remove('bi-heart')
                e.target.classList.add('bi-heart-fill')
            }
        }
    });
}

function dislikeAuction(postData, token, e) {
    $.ajax({
        type: 'POST',
        url: '/Auction/DislikeAuction',
        data: postData,
        headers: {
            'X-CSRF-TOKEN': token
        },
        success: function (response) {
            if (response.result == true) {
                e.target.classList.remove('bi-heart-fill')
                e.target.classList.add('bi-heart')
            }
        }
    });
}

function likeAuctionHandler(e) {
    e.preventDefault();
    let auctionId = $(this).parent().attr('data-auctionid');
    let token = $('#antiForgeryForm input').val();

    var postData = {
        auctionId
    };

    $.ajax({
        type: 'GET',
        url: '/Auction/DoesUserLikeCurrentAuction',
        data: postData,
        headers: {
            'X-CSRF-TOKEN': token
        },
        success: function (response) {
            if (!response) {
                likeAuction(postData, token, e);
            }
            else {
                dislikeAuction(postData, token, e)
            }
        }
    });
}