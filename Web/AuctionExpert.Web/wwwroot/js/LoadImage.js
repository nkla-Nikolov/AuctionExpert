function LoadBigImage(id) {
    let imageId = 'image+' + id;
    let bigImageElement = document.querySelector('#bigImage');
    let getImageSrc = document.getElementById(imageId).children[0].getAttribute('src');

    bigImageElement.setAttribute('src', getImageSrc);
}