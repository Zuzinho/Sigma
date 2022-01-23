function sendProjects() {
    let projects_array = document.querySelectorAll(".project__item-active");
    let ids_array = [];
    projects_array.forEach(project => ids_array.push(String(project.id)));
    console.log(ids_array);
    jQuery.ajax({
        method: "POST",
        url: "/Home/GetProjects",
        data: "ids_array=" + ids_array
    });
}