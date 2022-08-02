const plus = document.getElementById("LinkAddButton");

const ids = [];
document.querySelectorAll(".id").forEach(id => ids.push(id.textContent));

const providers = ["vk.com",
    "github.com",
    "linkedin.com",
    "telegram.org",
    "facebook.com",
    "twitter.com",
    "youtube.com"
];

function CreateLink(icon, num) {
    let id = num;
    let Parent_block = document.querySelector('.user_links_value');
    let user_link = document.createElement('div');
    user_link.id = id;
    user_link.className = 'user_link_border-active';
    let majorChildImage = document.createElement('img');
    majorChildImage.className = 'user_link_image';
    majorChildImage.src = icon;
    majorChildImage.alt = "";
    let blockChild = document.createElement('div');
    blockChild.className = 'trash_border';
    let imageChild = document.createElement('img');
    imageChild.className = 'trash_link';
    imageChild.src = '/Content/img/icons/Trash.png';
    imageChild.alt = "";
    blockChild.appendChild(imageChild);
    user_link.appendChild(majorChildImage);
    user_link.appendChild(blockChild);
    Parent_block.appendChild(user_link);
    blockChild.onclick = () => deleteicon(user_link);
}

const providers_icons = new Map([
    ["vk.com", "/Content/img/links_icons/vk.com.png"],
    ["github.com", "/Content/img/links_icons/github.com.png"],
    ["linkedin.com", "/Content/img/links_icons/linkedin.com.png"],
    ["telegram.org", "/Content/img/links_icons/telegram.org.png"],
    ["facebook.com", "/Content/img/links_icons/facebook.com.png"],
    ["twitter.com", "/Content/img/links_icons/twitter.com.png"],
    ["youtube.com", "/Content/img/links_icons/youtube.com.png"]
]);

plus.addEventListener("click", () => {
    let Url = document.getElementById("LinkAdd").value;
    let is_exist = false;
    let provider;
    providers.forEach(provider_ => {
        if (Url.indexOf(provider_) != -1) {
            is_exist = true;
            provider = provider_;
        }
    });
    if (is_exist) {
        var icon = providers_icons.get(provider);
    }
    let all_icons = document.querySelectorAll(".user_link_image");
    let is_already_exist = false;
    all_icons.forEach(icon_ => {
        if (icon_.src.replace("%20-%20", ' - ').indexOf(icon) != -1) is_already_exist = true;
    });
    if (!is_exist) {
        alert("You can`t insert link from this site ;(");
    }
    else if (is_already_exist) {
        alert("You already using link from this site,for adding of new link you need to delete used one ")
    }
    else {
        let id = ids.splice(0, 1);
        console.log(provider, icon);
        let formdata = new FormData();
        formdata.append("Url", Url);
        formdata.append("id", id);
        axios.post("/Home/AddLink", formdata);
        CreateLink(icon, id);
    }
    document.getElementById("LinkAdd").value = "";
})

function deleteicon(elem) {
    if (!confirm("Are you sure that you want to delete this link?")) return;
    var id = elem.id;
    ids.push(id);
    elem.remove();
    console.log(elem);
    let formdata = new FormData();
    console.log(id);
    formdata.append('idStr', id);
    axios.post('/Home/DeleteLink', formdata);
}