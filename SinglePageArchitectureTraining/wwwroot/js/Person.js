//#region [Elements]
// Form
var form = document.getElementById("form");

// Buttons
var btnInsert = document.getElementById("btnInsert");
var btnEdit = document.getElementById("btnEdit");
var btnDeleteSelected = document.getElementById("btnDeleteSelected");
var btnRefresh = document.getElementById("btnRefresh");
var btnDeleteConfirm = document.getElementById("btnDeleteConfirm");

// Inputs
var iptId = document.getElementById("id");
var iptFirstName = document.getElementById("firstName");
var iptLastName = document.getElementById("lastName");
var iptNationalCode = document.getElementById("nationalCode");

// Validation Messages
var vmFirstName = document.getElementById("firstNameValidationMessage");
var vmLastName = document.getElementById("lastNameValidationMessage");
var vmNationalCode = document.getElementById("nationalCodeValidationMessage");

// Delete Modal
var mdDeleteConfirm = document.getElementById("deleteConfirmModal");
var idInDeleteConfirmModal = document.getElementById("inDeleteConfirmModalId");
var deleteConfirmModalBody = document.getElementById("deleteConfirmModalBody");

// Details Modal
var mdDetails = document.getElementById("detailsModal");

// Others
var cbxSelectAll = document.getElementById("selectAll");
var tbody = document.querySelector("tbody");
var resultMessage = document.getElementById("resultMessage");
//#endregion

//#region [Event Listeners]
form.addEventListener("submit", Insert);
btnRefresh.addEventListener("click", LoadData);
btnEdit.addEventListener("click", Update);
cbxSelectAll.addEventListener("click", SelectOrDeselectAll);
mdDeleteConfirm.addEventListener("show.bs.modal", DeleteConfirm);
mdDetails.addEventListener("show.bs.modal", Details);
//#endregion

//#region [CodeBody]
var selectedRowsList = [];
var allRowsCount;

window.onload = LoadData();
//#endregion

//#region [Functions]
function LoadData() {
  RefreshForm(true);

  cbxSelectAll.checked = false;
  SelectOrDeselectAll();

  tbody.innerHTML = "";

  fetch("http://Localhost:5100/Person/GetPeople")
    .then((res) => res.json())
    .then((json) => {
      let html = "";
      allRowsCount = json.selectPersonDtosList.length;
      json.selectPersonDtosList.forEach(function (selectDto) {
        html += `<tr id="${selectDto.id}">
                  <td><input id="selectRow" class="form-check-input" type="checkbox" name="${selectDto.id}" onClick="SelectRow(this);"</td>
                  <td id="FNTD">${selectDto.firstName}</td>
                  <td id="LNTD">${selectDto.lastName}</td>
                  <td id="NCTD">${selectDto.nationalCode}</td>
                  <td>
                    <input class="btn btn-outline-primary btn-sm" type="button" value="Details" data-bs-toggle="modal" data-bs-target="#detailsModal">
                    <input class="btn btn-outline-danger btn-sm" type="button" value="Delete" data-bs-toggle="modal" data-bs-target="#deleteConfirmModal">                   
                  </td>
                </tr>`;
      });
      tbody.innerHTML = html;
    });
}

function Insert(e) {
  e.preventDefault();
  RefreshForm(false);
  let isValidData = ValidateFormData();

  if (isValidData) {
    let createDto = {
      Id: "",
      FirstName: iptFirstName.value,
      LastName: iptLastName.value,
      NationalCode: iptNationalCode.value,
    };

    fetch("http://Localhost:5100/Person/Create", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "*/*",
      },
      body: JSON.stringify(createDto),
    }).then((res) => {
      if (res.status == 409) {
        vmNationalCode.innerText = `Person with NationalCode : ${iptNationalCode.value} already exist`;
        iptNationalCode.classList.add("is-invalid");
      } else if (res.status == 200) {
        TriggerResultMessage("Operation Successful");
        LoadData();
      } else {
        TriggerResultMessage("Operation Failed");
      }
    });
  }
}

function Update() {
  RefreshForm(false);
  let isValidData = ValidateFormData();

  if (isValidData) {
    let updateDto = {
      Id: iptId.value,
      FirstName: iptFirstName.value,
      LastName: iptLastName.value,
      NationalCode: iptNationalCode.value,
    };

    fetch("http://Localhost:5100/Person/Update", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Accept: "*/*",
      },
      body: JSON.stringify(updateDto),
    }).then((res) => {
      if (res.status == 409) {
        vmNationalCode.innerText = `Person with NationalCode : ${iptNationalCode.value} already exist`;
        iptNationalCode.classList.add("is-invalid");
      } else if (res.status == 200) {
        TriggerResultMessage("Operation Successful");
        LoadData();
      } else {
        TriggerResultMessage("Operation Failed");
      }
    });
  }
}

function Delete() {
  let id = idInDeleteConfirmModal.value;
  fetch("http://Localhost:5100/Person/Delete", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "*/*",
    },
    body: JSON.stringify({ Id: id }),
  }).then((res) => {
    if (res.status == 200) {
      TriggerResultMessage("Operation Successful");
      LoadData();
    } else {
      TriggerResultMessage("Operation Failed");
    }
  });
  idInDeleteConfirmModal.value = "";
}

function DeleteSelected() {
  let deleteSelectedDto = { DeletePersonDtosList: [] };
  selectedRowsList.forEach((personId) => {
    deleteSelectedDto.DeletePersonDtosList.push({ Id: personId });
  });
  fetch("http://Localhost:5100/Person/DeleteSelected", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Accept: "*/*",
    },
    body: JSON.stringify(deleteSelectedDto),
  }).then((res) => {
    if (res.status == 200) {
      TriggerResultMessage("Operation Successful");
      LoadData();
    } else {
      TriggerResultMessage("Operation Failed");
    }
  });
}

function SelectOrDeselectAll() {
  document.querySelectorAll("#selectRow").forEach((checkBox) => {
    if (cbxSelectAll.checked == true) {
      if (!selectedRowsList.includes(checkBox.name)) {
        checkBox.checked = true;
        selectedRowsList.push(checkBox.name);
      }
    } else {
      checkBox.checked = false;
      selectedRowsList = [];
    }
  });
  if (cbxSelectAll.checked) {
    btnInsert.disabled = true;
    btnEdit.disabled = true;
    btnDeleteSelected.disabled = false;
    RefreshForm(true);
  } else {
    btnInsert.disabled = false;
    btnEdit.disabled = true;
    btnDeleteSelected.disabled = true;
    RefreshForm(true);
  }
}

function SelectRow(checkBox) {
  if (checkBox.checked == true) {
    selectedRowsList.push(checkBox.name);
  } else selectedRowsList.splice(selectedRowsList.indexOf(checkBox.name), 1);

  if (selectedRowsList.length != allRowsCount) cbxSelectAll.checked = false;
  else cbxSelectAll.checked = true;

  if (selectedRowsList.length >= 1) {
    btnDeleteSelected.disabled = false;
    btnInsert.disabled = true;
  } else {
    btnDeleteSelected.disabled = true;
    btnInsert.disabled = false;
  }

  if (selectedRowsList.length == 1) {
    btnEdit.disabled = false;
    RefreshForm();
    iptId.value = selectedRowsList[0];
    iptFirstName.value = document.querySelector(
      `tr[id="${selectedRowsList[0]}"] td[id="FNTD"]`
    ).innerText;
    iptLastName.value = document.querySelector(
      `tr[id="${selectedRowsList[0]}"] td[id="LNTD"]`
    ).innerText;
    iptNationalCode.value = document.querySelector(
      `tr[id="${selectedRowsList[0]}"] td[id="NCTD"]`
    ).innerText;
  } else {
    btnEdit.disabled = true;
    RefreshForm(true);
  }
}

function Details(event) {
  let clickedButton = event.relatedTarget;
  let id = clickedButton.parentNode.parentNode.id;

  fetch(`http://Localhost:5100/Person/Details?id=${id}`)
    .then((res) => res.json())
    .then((json) => {
      let inModalUl = document.querySelectorAll(".card li");
      inModalUl[0].innerText = `First Name : ${json.firstName}`;
      inModalUl[1].innerText = `Last Name : ${json.lastName}`;
      inModalUl[2].innerText = `National Code : ${json.nationalCode}`;
    });
}

function RefreshForm(removeInputValues) {
  if (removeInputValues) {
    iptFirstName.classList.remove("is-invalid", "is-valid");
    iptLastName.classList.remove("is-invalid", "is-valid");
    iptNationalCode.classList.remove("is-invalid", "is-valid");

    iptFirstName.id = "";
    iptFirstName.value = "";
    iptLastName.value = "";
    iptNationalCode.value = "";

    vmFirstName.innerText = "";
    vmLastName.innerText = "";
    vmNationalCode.innerText = "";
  } else {
    iptFirstName.classList.remove("is-invalid", "is-valid");
    iptLastName.classList.remove("is-invalid", "is-valid");
    iptNationalCode.classList.remove("is-invalid", "is-valid");

    vmFirstName.innerText = "";
    vmLastName.innerText = "";
    vmNationalCode.innerText = "";
  }
}

function ValidateFormData() {
  let isValidData = true;

  if (iptFirstName.value == "") {
    vmFirstName.innerText = "First Name is required";
    iptFirstName.classList.add("is-invalid");
    isValidData = false;
  } else {
    iptFirstName.classList.add("is-valid");
  }

  if (iptLastName.value == "") {
    vmLastName.innerText = "Last Name is required";
    iptLastName.classList.add("is-invalid");
    isValidData = false;
  } else {
    iptLastName.classList.add("is-valid");
  }

  if (!/^\d{10}$/.test(iptNationalCode.value)) {
    vmNationalCode.innerText = "Wrong National Code";
    iptNationalCode.classList.add("is-invalid");
    isValidData = false;
  }

  return isValidData;
}

function DeleteConfirm(event) {
  let clickedButton = event.relatedTarget;

  if (clickedButton.value == "Delete") {
    let id = clickedButton.parentNode.parentNode.id;
    idInDeleteConfirmModal.value = id;

    PassDetailsToDeleteConfirm(id);

    btnDeleteConfirm.addEventListener("click", Delete);
  } else {
    if (selectedRowsList.length == 1)
      PassDetailsToDeleteConfirm(selectedRowsList[0]);
    else
      deleteConfirmModalBody.innerHTML = `Your are deleting <strong>${selectedRowsList.length} records</strong> , Are you sure ? `;

    btnDeleteConfirm.addEventListener("click", DeleteSelected);
  }
}

function PassDetailsToDeleteConfirm(id) {
  let firstName = document.querySelector(
    `tr[id="${id}"] td[id="FNTD"]`
  ).innerText;
  let lastName = document.querySelector(
    `tr[id="${id}"] td[id="LNTD"]`
  ).innerText;
  let nationalCode = document.querySelector(
    `tr[id="${id}"] td[id="NCTD"]`
  ).innerText;
  deleteConfirmModalBody.innerHTML = `You are deleting :<br><strong>First Name : ${firstName}<br>Last Name : ${lastName}<br>National Code : ${nationalCode}</strong><br>Are you sure ?`;
}

function TriggerResultMessage(message) {
  resultMessage.innerText = message;
  resultMessage.style.opacity = "1";
  setTimeout(function () {
    resultMessage.style.opacity = "0";
  }, 2000);
}
//#endregion
