tinyMCE.init(
    {

        selector: "textarea",
        language: 'hr',
        branding: false,
        elementpath: false,
        height: 350,
        menubar: false,
        statusbar: false,
        resize: true,
        plugins: "textcolor lists link",
        toolbar: ' undo redo | styleselect | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | fontselect fontsizeselect | copy paste | forecolor backcolor | numlist bullist | link | '
        


    }
);



// varijable

var span = document.getElementsByClassName("closee")[0];
var modal = document.getElementById('myModal');
var img = document.getElementById("image");
var modalImg = document.getElementById("img01");




// kada vrsimo upload slike uzima sliku iz httppostedfilebase i ucitava u div za prikaz
document.getElementById("Slika").onchange = function () {
    var reader = new FileReader();

    reader.onload = function (e) {
        img.src = e.target.result;

        img.style.height = "250px";
        img.style.width = "auto";
    };
    reader.readAsDataURL(this.files[0]);
};

// otvara sliku za prikaz u modalnom prozoru
img.onclick = function () {
    modal.style.display = "block";
    modalImg.src = this.src;
};


// zatvara modalni prozor
span.onclick = function () {
    modal.style.display = "none";
};