var uploadSource = document.getElementById('photo-upload');
var linkSource = document.getElementById('photo-link');   

var photoSelect = document.getElementById('photo-select');

var navbar = document.getElementById('navbar');

document.getElementById('navbar-button').addEventListener('click', () => {

    navbar.classList.toggle('hidden');
}
)

if (photoSelect != null) {
    function onOptionChange() {

        var options = photoSelect.getElementsByTagName('option')[photoSelect.selectedIndex];

        if (options.value == 0) {
            uploadSource.classList.remove('hidden');
            linkSource.classList.add('hidden');
        }
        else if (options.value == 1) {
            uploadSource.classList.add('hidden');
            linkSource.classList.remove('hidden');
        }
        else {
            linkSource.classList.add('hidden');
            uploadSource.classList.add('hidden');
        }

    }
    onOptionChange()
    photoSelect.addEventListener('change', () => {
        onOptionChange()
    })
}

var output = document.getElementById('output');
var loadFile = function (event) {
    output.src = URL.createObjectURL(event.target.files[0]);
    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
};

var loadPhoto = (event) => {
    output.src = event.target.value;
}

/*var dropButton = document.getElementById("drop-button");
var dpList = document.getElementById("dropdown-list");
var adminDrop = document.getElementById("admindrop-list");


const dropdownHandle = () => {
    dpList.classList.toggle("hidden");
}


const adminDropHandle = () => {
    adminDrop.classList.toggle("hidden");
}
*/