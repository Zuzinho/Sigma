function ShowProjects() {
    document.querySelectorAll(".project__item-not_active").forEach(project => project.classList.toggle("project__item-active"));
}