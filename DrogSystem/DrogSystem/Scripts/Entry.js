$(document).ready(function () {
    loadData();
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
                html += '<tr>';
                html += '<td>' + item.FechaIngreso + '</td>';
                html += '<td>' + item.NombreTercero + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.EntryId + ')">Editar</a>    |    <a href="#" onclick="Delete(' + item.EntryId + ')">Eliminar</a></center></td>';
                html += '</tr>';
            });
            $('#maestro').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function AddFila() {
    var ProductDetailId = $('#ProductDetailId').val();
    var NombreProducto = $('#NombreProducto').val();
    var NombreFabricante = $('#NombreFabricante').val();
    var Cantidad = $('#Cantidad').val();
    var Lote = $('#Lote').val();
    var FechaVence = $('#FechaVence').val();
    var NroFila = document.getElementById("tabledetail").rows.length;
    var html = '';
    html = $('#detalle').html();
    html += '<tr ProductDetailId="' + ProductDetailId + '" EntryDetailId = "">';
    html += '<td>' + NombreProducto + '</td>';
    html += '<td>' + NombreFabricante + '</td>';
    html += '<td>' + Cantidad + '</td>';
    html += '<td>' + Lote + '</td>';
    html += '<td>' + FechaVence + '</td>';
    html += '<td><center><a href="#" onclick="editarFila(this)">Editar</a>   |   <a href="#" onclick="eliminarFila(this)">Eliminar</a></center></td>';
    html += '</tr>';
    $('#detalle').html(html);
    $('#myModal1').modal('hide');
    $('.modal-backdrop').remove();
}
 
function eliminarFila(i) {
    var table = document.getElementById("tabledetail");
    var rowCount = i.parentNode.parentNode.parentNode.rowIndex;
    if (rowCount < 1)
        alert('No se puede eliminar el encabezado');
    else
        table.deleteRow(rowCount);
}

function editarFila(nodo) {
    var nodoTd = nodo.parentNode.parentNode; //Nodo TD
    var nodoTr = nodoTd.parentNode; //Nodo TR
    var nodosEnTr = nodoTr.getElementsByTagName('td');
    var Htmledit = nodoTr.innerHTML;
    var diaact = nodosEnTr[4].textContent.substr(0, 2);
    var mesact = nodosEnTr[4].textContent.substr(3, 2);
    var anioact = nodosEnTr[4].textContent.substr(6, 4);
    var userObj = {
        producto: nodosEnTr[0].textContent,
        fabricante: nodosEnTr[1].textContent,
        cantidad: nodosEnTr[2].textContent,
        lote: nodosEnTr[3].textContent,
        fechaVence: anioact + '-' + mesact + '-' + diaact,
    };
    var nuevoCodigoHtml = '';
    nuevoCodigoHtml += '<td>' + userObj.producto + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.fabricante + '</td>';
    nuevoCodigoHtml += '<td><input type="text" name="cantidad" id="cantidadedit" value="' + userObj.cantidad + '" size="10"></td>';
    nuevoCodigoHtml += '<td><input type="text" name="lote" id="loteedit" value="' + userObj.lote + '" size="10"></td>';
    nuevoCodigoHtml += '<td><input type="date" name="fechaVenceedit" id="fechaVenceedit" value="' + userObj.fechaVence + '" size="10"></td>';
    nuevoCodigoHtml += '<td><center><a href="#" onclick="actualizar(this)">Actualizar</a></center></td>';
    nodoTr.innerHTML = nuevoCodigoHtml;
    
}

function actualizar(nodo) {
    var nodoTd = nodo.parentNode.parentNode; //Nodo TD
    var nodoTr = nodoTd.parentNode; //Nodo TR
    var nodosEnTr = nodoTr.getElementsByTagName('td');
    var nuevoCodigoHtml = '';
    var diaact = $('#fechaVenceedit').val().substr(8, 2);
    var mesact = $('#fechaVenceedit').val().substr(5, 2);
    var anioact = $('#fechaVenceedit').val().substr(0, 4);
    var userObj = {
        producto: nodosEnTr[0].textContent,
        fabricante: nodosEnTr[1].textContent,
        cantidad: $('#cantidadedit').val(),
        lote: $('#loteedit').val(),
        fechaVence: diaact + '/' + mesact + '/' + anioact,
    };
    nuevoCodigoHtml += '<td>' + userObj.producto + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.fabricante + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.cantidad + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.lote + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.fechaVence + '</td>';
    nuevoCodigoHtml += '<td><center><a href="#" onclick="editarFila(this)">Editar</a>   |   <a href="#" onclick="eliminarFila(this)">Eliminar</a></center></td>';
    nodoTr.innerHTML = nuevoCodigoHtml;
}


function validate() {
    var isValid = true;
    if ($('#NombreTercero').val().trim() == "") {
        $('#NombreTercero').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NombreTercero').css('border-color', 'lightgrey');
    }
    return isValid;
}

function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var Aprobado = '';
    if (document.getElementsByName('Aprobado')[0].checked) {
        Aprobado = 'S';
    }
    else if (document.getElementsByName('Aprobado')[1].checked) {
        Aprobado = 'N';
    }
    var nodo = document.getElementsByTagName('tbody');
    var entryObj = {
        TerceroId: $('#NombreTercero').val(),
        FechaIngreso: $('#FechaIngreso').val(),
        Aprobado: $('input:radio[name="Aprobado"]:checked').val(),
    };
    var detailObj = {
        ProductDetailId: '',
        NombreProducto: '',
        NombreFabricante: '',
        Cantidad: '',
        Lote: '',
        FechaVence: '',
    };
    var detailObj = {
        ProductDetailId: '',
        NombreProducto: '',
        NombreFabricante: '',
        Cantidad: '',
        Lote: '',
        FechaVence: '',
    };
    var arraydetail = [];
    for (var i = 0; i < nodo[0].rows.length; i++) {
        arraydetail.push({ ProductDetailId: nodo[0].rows[i].attributes[0].value,
            NombreProducto: nodo[0].rows[i].cells[0].innerText,
            NombreFabricante : nodo[0].rows[i].cells[1].innerText,
            Cantidad : nodo[0].rows[i].cells[2].innerText,
            Lote: nodo[0].rows[i].cells[3].innerText,
            FechaVence : nodo[0].rows[i].cells[4].innerText
        });
    }
    $.ajax({
        url: "/Entries/Crear",
        data: '{DetalleEntrada: ' + JSON.stringify(arraydetail) + ', Entradas: ' + JSON.stringify(entryObj) + '}',
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.Probar == true) {
                swal.fire({
                    title: "Estimado Usuario",
                    text: response.Mensaje,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    icon: "success",
                    closeOnConfirm: false
                }).then((result) => {
                    if (result.value) {
                        window.location.href = './Home/Index';
                    }
                });
            }           
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