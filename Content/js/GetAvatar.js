let image = document.querySelector(".image");
function GetAvatar(e) {
    console.log(e);
    var file = e.target.files[0];
    console.log(file);
    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        image.src = reader.result;
    }
}