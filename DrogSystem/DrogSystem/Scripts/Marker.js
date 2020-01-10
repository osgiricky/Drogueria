$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Markers/List",
        //data: '1',
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombreFabricante + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.MarkerId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.MarkerId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getbyID(MarkerId) {
    document.title = 'Modificar Fabricante';
    $('#NombreFabricante').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Markers/getbyID/" + MarkerId,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#MarkerId').val(result.MarkerId);
            $('#NombreFabricante').val(result.NombreFabricante);

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

function Delete(ID) {
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
                url: "/Markers/Borrar/" + ID,
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
        MarkerId: $('#MarkerId').val(),
        NombreFabricante: $('#NombreFabricante').val(),
    };
    $.ajax({
        url: "/Markers/Editar",
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
            else
            {
                loadData();
                $('#myModal').modal('hide');
                $('#MarkerId').val("");
                $('#NombreFabricante').val("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#NombreFabricante').val().trim() == "") {
        $('#NombreFabricante').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NombreFabricante').css('border-color', 'lightgrey');
    }    
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        MarkerId: $('#MarkerId').val(),
        NombreFabricante: $('#NombreFabricante').val(),
    };
    $.ajax({
        url: "/Markers/Crear",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
                 $('#myModal').modal('hide')
                 $('#MarkerId').val("");
                 $('#NombreFabricante').val("");                 
                 //$('#cerrar').click(); //Esto simula un click sobre el botón close de la modal, por lo que no se debe preocupar por qué clases agregar o qué clases sacar.
                 $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    $('#MarkerId').val("");
    $('#NombreFabricante').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#NombreFabricante').css('border-color', 'lightgrey');
}