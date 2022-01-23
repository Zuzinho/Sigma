function deleteicon(elem_id) {
    if (confirm("Are you sure thet you want to delete this link?")) {
        let id = Number(elem_id);
        let id_block = document.getElementById(elem_id);
        id_block.remove();
        jQuery.ajax({
            type: "POST",
            url: "/Home/DeleteLink",
            data: "id=" + id
        });
    }
}