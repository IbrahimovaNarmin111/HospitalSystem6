let deletebtnimages = document.querySelectorAll(".delete-image-button");

deletebtnimages.forEach(btn => btn.addEventListener("click", function (e) {
    btn.parentElement.remove()
}))
