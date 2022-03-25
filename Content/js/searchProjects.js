function searchProjects() {
    let projects_input = document.getElementById("ProjectSearch");
    let project_order = projects_input.value;
    console.log(project_order);
    let projects_list = document.querySelectorAll(".project__item");
    if (project_order != "") {
        projects_list.forEach(project => {
            if (project.querySelector(".item__name").innerText.toLowerCase().indexOf(project_order.toLowerCase()) == -1) project.classList.add("project__item-not_selected");
            else if (project.classList.contains("project__item-not_selected")) project.classList.remove("project__item-not_selected");
        })
    }
    else {
        projects_list.forEach(project => { if (project.classList.contains("project__item-not_selected")) project.classList.remove("project__item-not_selected") });
    }
    projects_list.forEach(project => console.log(project.classList));
}