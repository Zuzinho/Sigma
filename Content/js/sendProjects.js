function sendProjects() {
    let projects_array = document.querySelectorAll(".project__item-active");
    let ids_array = "";
    projects_array.forEach(project => ids_array += project.id + ',');
    console.log(ids_array);
    let formdata = new FormData();
    formdata.append('idsArray_str', ids_array);
    console.log(ids_array);
    axios.post('/Home/GetProjects', formdata);
}