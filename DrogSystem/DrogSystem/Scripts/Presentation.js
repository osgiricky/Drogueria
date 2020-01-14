$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Presentations/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombrePresentacion + '</td>';
                html += '<td>' + item.CantPresentacion + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.PresentationId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.PresentationId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(PresentationId) {
    document.title = 'Modificar Fabricante';
    $('#NombrePresentacion').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Presentations/getbyID/" + PresentationId,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#PresentationId').val(result.PresentationId);
            $('#NombrePresentacion').val(result.NombrePresentacion);
            $('#CantPresentacion').val(result.CantPresentacion);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Delete(PresentationId) {
    Swal.fire({
        title: "Estimado Usuario",
        text: "Esta seguro(a) que desea eliminar registro?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: '#d33',
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.value == true) {
            $.ajax({
                url: "/Presentations/Borrar/" + PresentationId,
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                //data: ID,
                success: function (response) {
                    if (response.Probar == false) {
                        swal.fire({
                            title: "Estimado Usuario",
                            text: response.Mensaje,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",
                            icon: "error",
                            closeOnConfirm: false
                        });
                    }
                    else {
                        Swal.fire({
                            title: "Estimado Usuario",
                            text: response.Mensaje,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",
                            icon: "success",
                            closeOnConfirm: false
                        });
                    }
                    loadData();
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        }
    })
}
function Update() {
    var res = validate();
    if (res == false) {
        swal.fire({
            title: "Estimado Usuario",
            text: "Los campos marcados en rojo son obligatorios.",
            icon: 'warning',
            showCancelButton: false,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Aceptar",
        });
        return false;
    }
    var empObj = {
        PresentationId: $('#PresentationId').val(),
        NombrePresentacion: $('#NombrePresentacion').val(),
        CantPresentacion: $('#CantPresentacion').val(),
    };
    $.ajax({
        url: "/Presentations/Editar",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Probar == false) {
                swal.fire({
                    title: "Estimado Usuario",
                    text: response.Mensaje,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    icon: "error",
                    closeOnConfirm: false
                });
            }
            else {
                loadData();
                $('#myModal').modal('hide');
                $('#PresentationId').val("");
                $('#NombrePresentacion').val("");
                $('#CantPresentacion').val("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#NombrePresentacion').val().trim() == "") {
        $('#NombrePresentacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NombrePresentacion').css('border-color', 'lightgrey');
    }
    if ($('#CantPresentacion').val().trim() == "") {
        $('#CantPresentacion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#CantPresentacion').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        PresentationId: $('#PresentationId').val(),
        NombrePresentacion: $('#NombrePresentacion').val(),
        CantPresentacion: $('#CantPresentacion').val(),
    };
    $.ajax({
        url: "/Presentations/Crear",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide')
            $('#PresentationId').val("");
            $('#NombrePresentacion').val("");
            $('#CantPresentacion').val("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    $('#PresentationId').val("");
    $('#NombrePresentacion').val("");
    $('#CantPresentacion').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#NombrePresentacion').css('border-color', 'lightgrey');
}