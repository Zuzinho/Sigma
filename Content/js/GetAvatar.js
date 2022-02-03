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
    //var formData = new FormData();
    //formData.append("UserAvatar", file);
    //for (var [key, value] of formData.entries()) {
    //    console.log(key, value);
    //}
//    axios.post("/Home/AddAvatar", formData);
}