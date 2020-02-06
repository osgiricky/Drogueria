$(document).ready(function () {
    clearTextBox();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Entries/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            var tercero;
            $.each(result, function (key, item) {
                tercero = '';
                if (item.TipoTercero == "P") {
                    tercero = 'Proveedor';
                }
                else if (item.TipoTercero == "O") {
                    tercero = 'Otro';;
                }
                html += '<tr>';
                html += '<td>' + item.NombreTercero + '</td>';
                html += '<td>' + item.Codtercero + '</td>';
                html += '<td>' + tercero + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.TerceroId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.TerceroId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function AddFila() {
    var NombreProducto = $('#NombreProducto').val();
    var NombreFabricante = $('#NombreFabricante').val();
    var Cantidad = $('#Cantidad').val();
    var Lote = $('#Lote').val();
    var FechaVence = $('#FechaVence').val();
    var NroFila = document.getElementById("tabledetail").rows.length;
    var html = '';
    html = $('.tbody').html();
    html += '<tr class="fila' + NroFila + '">';
    html += '<td>' + NombreProducto + '</td>';
    html += '<td>' + NombreFabricante + '</td>';
    html += '<td>' + Cantidad + '</td>';
    html += '<td>' + Lote + '</td>';
    html += '<td>' + FechaVence + '</td>';
    html += '<td><center><a href="#" onclick="return getbyID(fila' + NroFila + ')">Editar</a>    |    <a href="#" onclick="Delete(fila' + NroFila + ')">Eliminar</a></center></td>';
    html += '</tr>';
    $('.tbody').html(html);
    $('#myModal').modal('hide');
    $('.modal-backdrop').remove();
}

function getbyID(EntryDetailId) {
    document.getElementById('myModalLabel').innerHTML = "Editar Tercero";
    $('#NombreTercero').css('border-color', 'lightgrey');
    $('#Codtercero').css('border-color', 'lightgrey');
    $('#ProviderTypeId').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Entries/getbyID/" + EntryDetailId,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#TerceroId').val(result.TerceroId);
            $('#NombreTercero').val(result.NombreTercero);
            $('#Codtercero').val(result.Codtercero);
            if (result.TipoTercero == "P") {
                $('#O').attr('checked', false);
                $('#P').attr('checked', true);
            }
            else if (result.TipoTercero == "O") {
                $('#P').attr('checked', false);
                $('#O').attr('checked', true);
            }

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

function Delete(EntryDetailId) {
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
                url: "/Entries/Borrar/" + EntryDetailId,
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
    var tipotercero = '';
    if (document.getElementsByName('TipoTercero')[0].checked) {
        tipotercero = 'P';
    }
    else if (document.getElementsByName('TipoTercero')[1].checked) {
        tipotercero = 'O';
    }
    var userObj = {
        TerceroId: $('#TerceroId').val(),
        NombreTercero: $('#NombreTercero').val(),
        Codtercero: $('#Codtercero').val(),
        TipoTercero: tipotercero,
    };
    $.ajax({
        url: "/Entries/Editar",
        data: JSON.stringify(userObj),
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
                $('#TerceroId').val("");
                $('#NombreTercero').val("");
                $('#Codtercero').val("");
                $('#ProviderTypeId').html("");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function validate() {
    var isValid = true;
    if ($('#CodBarras').val().trim() == "") {
        $('#CodBarras').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#CodBarras').css('border-color', 'lightgrey');
    }
    if ($('#Cantidad').val().trim() == "") {
        $('#Cantidad').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Cantidad').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var tipotercero = '';
    if (document.getElementsByName('TipoTercero')[0].checked) {
        tipotercero = 'P';
    }
    else if (document.getElementsByName('TipoTercero')[1].checked) {
        tipotercero = 'O';
    }
    var userObj = {
        TerceroId: $('#TerceroId').val(),
        NombreTercero: $('#NombreTercero').val(),
        Codtercero: $('#Codtercero').val(),
        TipoTercero: tipotercero,
    };
    $.ajax({
        url: "/Entries/Crear",
        data: JSON.stringify(userObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            loadData();
            $('#myModal').modal('hide');
            $('#TerceroId').val("");
            $('#NombreTercero').val("");
            $('#Codtercero').val("");
            $('.modal-backdrop').remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearTextBox() {
    $('#EntryId').val("");
    $('#NombreTercero').html("");
    $('#NombreTercero').css('border-color', 'lightgrey');
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#S').attr('checked', false);
    $('#N').attr('checked', true);
    $.ajax({
        url: "/Entries/listaProveedores/",
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var html = html += '<option value="" selected> Selecione un Proveedor</option>';;
            $.each(result, function (key, item) {
                html += '<option value="' + item.TerceroId + '" >' + item.NombreTercero + '</option>';
            });
            $('#NombreTercero').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    var fecha = new Date();
    var d = fecha.getDate();
    var m = fecha.getMonth() + 1;
    var y = fecha.getFullYear();
    var dateString = (d <= 9 ? '0' + d : d) + '/' + (m <= 9 ? '0' + m : m) + '/' + y;
    $("#FechaIngreso").val(dateString);
}

function clearModal() {
    $('#EntryDetailId').val("");
    $('#CodBarras').val("");
    $('#NombreProducto').val("");
    $('#NombreFabricante').val("");
    $('#RegInvima').val("");
    $('#Cantidad').val("");
    $('#FechaVence').val("");
    $('#Lote').val("");
}

function buscarProduct() {
    var CodBarra = $('#CodBarras').val();
    if (CodBarra.length == 13) {
        $.ajax({
            url: "/ProductPresentationPrices/BuscarProducto",
            data: '{CodBarras: "' + CodBarra + '" }',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#ProductDetailId').val(result.ProductDetailId);
                $('#NombreProducto').val(result.NombreProducto);
                $('#NombreFabricante').val(result.NombreFabricante);
                $('#RegInvima').val(result.RegInvima);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    else {
        swal.fire({
            title: "Estimado Usuario",
            text: "El codigo de barras debe tener una longitud de 13 caracteres.",
            icon: 'info',
            showCancelButton: false,
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Aceptar",
        });
        return false;

    }
}