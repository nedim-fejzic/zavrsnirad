


// varijable

var span = document.getElementsByClassName("closee")[0];
var modal = document.getElementById('myModal');
var img = document.getElementById("image");
var modalImg = document.getElementById("img01");




// otvara sliku za prikaz u modalnom prozoru
img.onclick = function () {
    modal.style.display = "block";
    modalImg.src = this.src;
};


// zatvara modalni prozor
span.onclick = function () {
    modal.style.display = "none";
};