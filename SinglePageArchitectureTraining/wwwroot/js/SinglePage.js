window.onload = LoadData();
document.getElementById("btnSave").addEventListener("click", SaveData);

var tbody = document.querySelector("tbody");
function LoadData() {
    fetch("http://Localhost:5100/Person/GetPeople")
        .then((res) => res.json())
        .then((json) => {
            let html = "";
            json.selectPersonDtosList.forEach(function (person) {
                html += `<tr>
                    <td>${person.firstName} ${person.lastName}</td>
                    <td>${person.nationalCode}</td>
                    <td>
                        <input id="${person.id}" class="btn btn-outline-primary btn-sm" type="button" value="Details" onClick="Details(this.id);">
                        <input id="${person.id}" class="btn btn-outline-dark btn-sm" type="button" value="Edit" onClick="Edit(this.id);">
                        <input id="${person.id}" class="btn btn-outline-danger btn-sm" type="button" value="Delete" onClick="Delete(this.id);">
                    </td>

                    </tr>`
            })
            tbody.innerHTML = html;
        });
}

function Details(id) {
    window.location.href = `http://Localhost:5100/Person/Details?id=${id}`
}
function Delete(id) {
    window.location.href = `http://Localhost:5100/Person/Delete?id=${id}`
}

function SaveData() {
    let id = document.getElementById("Id").value;
    let firstName = document.getElementById("firstName").value;
    let lastName = document.getElementById("lastName").value;
    let nationalCode = document.getElementById("nationalCode").value;

    // if (Id == "") {
    let createPersonDto = new Object();
    createPersonDto.Id = "";
    createPersonDto.FirstName = firstName;
    createPersonDto.LastName = lastName;
    createPersonDto.NationalCode = nationalCode;

    console.log(createPersonDto);
    fetch("http://Localhost:5100/Person/Create", {
        method: 'POST',
        body: { createPersonDto }
    })
        .then((res) => console.log(res));

}