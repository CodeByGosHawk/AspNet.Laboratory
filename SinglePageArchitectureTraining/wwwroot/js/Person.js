var selectAllCheckBox = document.getElementById("selectAll");
window.onload = LoadData();

var selectedRowsList = [];
var allRowsCount;

document.getElementById("form").addEventListener("submit", function (e) {
  e.preventDefault();
  Insert();
});

document.getElementById("btnRefresh").addEventListener("click", LoadData);
document.getElementById("btnDeleteAll").addEventListener("click", DeleteAll);
document.getElementById("btnEdit").addEventListener("click", Update);

function Update() {
  let id = document.getElementById("id");
  let firstName = document.getElementById("firstName");
  let lastName = document.getElementById("lastName");
  let nationalCode = document.getElementById("nationalCode");
  firstName.classList.remove("is-invalid");
  lastName.classList.remove("is-invalid");
  nationalCode.classList.remove("is-invalid");
  let isValidateData = true;

  if (firstName.value == "") {
    let validationMessage = document.getElementById(
      "firstNameValidationMessage"
    );
    validationMessage.innerText = "Enter First Name";
    firstName.classList.add("is-invalid");
    isValidateData = false;
  }

  if (lastName.value == "") {
    let validationMessage = document.getElementById(
      "lastNameValidationMessage"
    );
    validationMessage.innerText = "Enter Last Name";
    lastName.classList.add("is-invalid");
    isValidateData = false;
  }

  if (!/^\d{10}$/.test(nationalCode.value)) {
    let validationMessage = document.getElementById(
      "nationalCodeValidationMessage"
    );
    validationMessage.innerText =
      "Wrong National Code (National Code should be 10 digits)";
    nationalCode.classList.add("is-invalid");
    isValidateData = false;
  }

  if (isValidateData) {
    let updateDto = new Object();
    updateDto.Id = id.value;
    updateDto.FirstName = firstName.value;
    updateDto.LastName = lastName.value;
    updateDto.NationalCode = nationalCode.value;

    fetch("http://Localhost:5100/Person/Update", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "*/*",
      },
      body: JSON.stringify(updateDto),
    })
      .then((res) => res.json())
      .then((json) => {
        console.log(updateDto);
        console.log(json);
        if (json == "NCError") {
          document.getElementById(
            "nationalCodeValidationMessage"
          ).innerText = `Person with NationalCode : ${nationalCode.value} already exist`;
          nationalCode.classList.add("is-invalid");
        } else LoadData();
      });
  }
}

function DeleteAll() {
  if (
    confirm(
      `Your are deleting ${selectedRowsList.length} records , Are you sure ? `
    )
  ) {
    let index = 0;
    selectedRowsList.forEach((personId) => {
      fetch("http://Localhost:5100/Person/Delete", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Accept: "*/*",
        },
        body: JSON.stringify({ Id: personId }),
      }).then(() => {
        if (index == selectedRowsList.length - 1) LoadData();
        index++;
      });
    });
  }
}

selectAllCheckBox.addEventListener("click", SelectAllCheckBoxClicked)

function SelectAllCheckBoxClicked() {
  document.querySelectorAll("#selectRow").forEach((checkBox) => {
    if (selectAllCheckBox.checked == true) {
      if(!selectedRowsList.includes(checkBox.name)){
        checkBox.checked = true;
        selectedRowsList.push(checkBox.name);
      }
    } else {
      checkBox.checked = false;
      selectedRowsList.splice(selectedRowsList.indexOf(checkBox.name));
    }
  });
  if (selectAllCheckBox.checked) {
    document.getElementById("btnInsert").disabled = true;
    document.getElementById("btnEdit").disabled = true;
    document.getElementById("btnDeleteAll").disabled = false;
    document.getElementById("firstName").value = "";
    document.getElementById("lastName").value = "";
    document.getElementById("nationalCode").value = "";
  } else {
    document.getElementById("btnInsert").disabled = false;
    document.getElementById("btnEdit").disabled = true;
    document.getElementById("btnDeleteAll").disabled = true;
    document.getElementById("firstName").value = "";
    document.getElementById("lastName").value = "";
    document.getElementById("nationalCode").value = "";
  }
  console.log(selectedRowsList);
}

function CheckBoxClicked(checkBox) {
  if (checkBox.checked == true) {
    selectedRowsList.push(checkBox.name);
  } else selectedRowsList.splice(selectedRowsList.indexOf(checkBox.name),1);

  if (selectedRowsList.length != allRowsCount)
    selectAllCheckBox.checked = false;
  else selectAllCheckBox.checked = true;

  console.log(selectedRowsList);

  if (selectedRowsList.length >= 1) {
    document.getElementById("btnDeleteAll").disabled = false;
    document.getElementById("btnInsert").disabled = true;
  } else {
    document.getElementById("btnDeleteAll").disabled = true;
    document.getElementById("btnInsert").disabled = false;
  }
  if (selectedRowsList.length == 1) {
    document.getElementById("btnEdit").disabled = false;

    document.getElementById("id").value = selectedRowsList[0];
    document.getElementById("firstName").value = document.getElementById(
      "FN" + selectedRowsList[0]
    ).innerText;
    document.getElementById("lastName").value = document.getElementById(
      "LN" + selectedRowsList[0]
    ).innerText;
    document.getElementById("nationalCode").value = document.getElementById(
      "NC" + selectedRowsList[0]
    ).innerText;
  } else {
    document.getElementById("btnEdit").disabled = true;

    document.getElementById("id").value = "";
    document.getElementById("firstName").value = "";
    document.getElementById("lastName").value = "";
    document.getElementById("nationalCode").value = "";
  }
}

var tbody = document.querySelector("tbody");

function LoadData() {
  let firstName = document.getElementById("firstName");
  let lastName = document.getElementById("lastName");
  let nationalCode = document.getElementById("nationalCode");
  firstName.classList.remove("is-invalid");
  lastName.classList.remove("is-invalid");
  nationalCode.classList.remove("is-invalid");
  firstName.value = "";
  lastName.value = "";
  nationalCode.value = "";
  selectAllCheckBox.checked = false;
  SelectAllCheckBoxClicked();

  document.querySelector("tbody").innerHTML = "";

  fetch("http://Localhost:5100/Person/GetPeople")
    .then((res) => res.json())
    .then((json) => {
      let html = "";
      allRowsCount = json.selectPersonDtosList.length;
      json.selectPersonDtosList.forEach(function (person) {
        html += `<tr>
                        <td><input id="selectRow" class="form-check-input" type="checkbox" name="${person.id}" onClick="CheckBoxClicked(this);"></td>
                        <td id="FN${person.id}">${person.firstName}</td>
                        <td id="LN${person.id}">${person.lastName}</td>
                        <td id="NC${person.id}">${person.nationalCode}</td>
                        <td>
                            <input name="${person.id}" class="btn btn-outline-primary btn-sm" type="button" value="Details" onClick="Details(this.name);">
                            <input name="${person.id}" class="btn btn-outline-danger btn-sm" type="button"  value="Delete" onClick="Delete(this.name);">
                        </td>

                        </tr>`;
      });
      tbody.innerHTML = html;
    });
}

function Details(id) {
  fetch(`http://Localhost:5100/Person/Details?id=${id}`)
  .then((res) => res.json())
  .then((json) => {
    console.log(json);
    alert(`***Person Details***\nFirst Name : ${json.firstName}\nLast Name : ${json.lastName}\nNational Code : ${json.nationalCode}`);
  })
}


function Delete(id) {
  if (confirm("Are you sure ? ")) {
    fetch("http://Localhost:5100/Person/Delete", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "*/*",
      },
      body: JSON.stringify({ Id: id }),
    }).then((res) => LoadData());
  }
}

function Insert() {
  let firstName = document.getElementById("firstName");
  let lastName = document.getElementById("lastName");
  let nationalCode = document.getElementById("nationalCode");
  firstName.classList.remove("is-invalid");
  lastName.classList.remove("is-invalid");
  nationalCode.classList.remove("is-invalid");
  let isValidateData = true;

  if (firstName.value == "") {
    let validationMessage = document.getElementById(
      "firstNameValidationMessage"
    );
    validationMessage.innerText = "Enter First Name";
    firstName.classList.add("is-invalid");
    isValidateData = false;
  }

  if (lastName.value == "") {
    let validationMessage = document.getElementById(
      "lastNameValidationMessage"
    );
    validationMessage.innerText = "Enter Last Name";
    lastName.classList.add("is-invalid");
    isValidateData = false;
  }

  if (!/^\d{10}$/.test(nationalCode.value)) {
    let validationMessage = document.getElementById(
      "nationalCodeValidationMessage"
    );
    validationMessage.innerText =
      "Wrong National Code (National Code should be 10 digits)";
    nationalCode.classList.add("is-invalid");
    isValidateData = false;
  }

  if (isValidateData) {
    let createDto = new Object();
    createDto.Id = "";
    createDto.FirstName = firstName.value;
    createDto.LastName = lastName.value;
    createDto.NationalCode = nationalCode.value;

    fetch("http://Localhost:5100/Person/Create", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "*/*",
      },
      body: JSON.stringify(createDto),
    })
      .then((res) => res.json())
      .then((json) => {
        if (json == "NCError") {
          document.getElementById(
            "nationalCodeValidationMessage"
          ).innerText = `Person with NationalCode : ${nationalCode.value} already exist`;
          nationalCode.classList.add("is-invalid");
        } else LoadData();
      });
  }
}
