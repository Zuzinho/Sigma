function deleteicon(elem_id) {
    if (confirm("Are you sure that you want to delete this link?")) {
        elem_id.remove();
        jQuery.ajax({
            type: "POST",
            url: "/Home/DeleteLink",
            data: "id_string=" + elem_id.id
        });
    }
}