function ChangeActive(project_id) {
    if (document.querySelectorAll(".project__item-active").length < 6) {
        project_id.classList.toggle("project__item-active");
    }
    else if (project_id.classList.contains("project__item-active")) {
        project_id.classList.toggle("project__item-active");
    }
    console.log(document.querySelectorAll(".project__item-active").length);
}   