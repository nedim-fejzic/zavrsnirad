

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