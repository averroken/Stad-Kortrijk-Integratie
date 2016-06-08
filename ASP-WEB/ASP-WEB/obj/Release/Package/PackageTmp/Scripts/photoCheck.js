window.URL = window.URL || window.webkitURL;
var elBrowse = document.getElementById("browse"),
        elPreview = document.getElementById("preview"),
        enabledisable = document.getElementById("enabledisable"),
        useBlob = false && window.URL; // `true` to use Blob instead of Data-URL

function readImage(file) {
    elPreview.innerHTML = "";
    var reader = new FileReader();
    reader.addEventListener("load", function () {
        var image = new Image();
        image.addEventListener("load", function () {
            if (file.size > 2048576) {
                elPreview.innerHTML += "Foto mag niet groter zijn dan 2MB </br>";
                enabledisable.disabled = true;
            }
            if (image.width !== 100 || image.height !== 100) {
                elPreview.innerHTML += "De foto moet 100 x 100 pixels groot zijn </br>";
                enabledisable.disabled = true;
            }
            if (elPreview.innerHTML == "") {
                enabledisable.disabled = false;
            }
        });
        image.src = useBlob ? window.URL.createObjectURL(file) : reader.result;
        if (useBlob) {
            window.URL.revokeObjectURL(file);
        }
    });
    reader.readAsDataURL(file);
}

elBrowse.addEventListener("change", function () {
    var files = this.files;
    var errors = "";
    if (!files) {
        elPreview.innerHTML += "File upload not supported by your browser.";
    }
    if (files && files[0]) {
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            if ((/\.(png|jpeg|jpg|gif)$/i).test(file.name)) {
                readImage(file);
            } else {
                elPreview.innerHTML += file.name + " Unsupported Image extension\n";
            }
        }
    }
    if (errors) {
        alert(errors);
    }
});