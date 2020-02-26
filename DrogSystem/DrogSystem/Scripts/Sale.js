$(document).ready(function () {
    loadData();
});


//Load Data function  
function loadData() {
    var fecha = new Date();
    var d = fecha.getDate();
    var m = fecha.getMonth() + 1;
    var y = fecha.getFullYear();
    var dateString = (d <= 9 ? '0' + d : d) + '/' + (m <= 9 ? '0' + m : m) + '/' + y;
    $("#FechaFactura").val(dateString);
    $.ajax({
        url: "/Sales/NroFactura",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if(result == 0)
                document.getElementById("NroFactura").disabled = false; 
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
    var PresentacionID = $('#PresentationId').val();
    var DescripPresentacion = $('#PresentationId')[0].selectedOptions[0].text;
    var Cantidad = $('#Cantidad').val();
    var Precio = Number($('#Precio').val());
    var PrecioTotal = Number($('#PrecioTotal').val());
    var res = ValidarExistencias(ProductDetailId, PresentacionID, Cantidad);
    if (res == false) {
        return false;
    }
    var html = '';
    html = $('#detalle').html();
    html += '<tr ProductDetailId="' + ProductDetailId + '" PresentacionID = "' + PresentacionID + '">';
    html += '<td>' + NombreProducto + '</td>';
    html += '<td>' + NombreFabricante + '</td>';
    html += '<td>' + DescripPresentacion + '</td>';
    html += '<td>' + Cantidad + '</td>';
    html += '<td>' + currencyFormat(Precio) + '</td>';
    html += '<td>' + currencyFormat(PrecioTotal) + '</td>';
    html += '<td><center><a href="#" onclick="editarFila(this)">Editar</a>   |   <a href="#" onclick="eliminarFila(this)">Eliminar</a></center></td>';
    html += '</tr>';
    $('#detalle').html(html);
    $('#myModal').modal('hide');
    $('.modal-backdrop').remove();
    CalcularTotal();
}

function eliminarFila(i) {
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
        var table = document.getElementById("tabledetail");
        var rowCount = i.parentNode.parentNode.parentNode.rowIndex;
        var IdBorrar = i.parentNode.parentNode.parentNode.attributes[1].value;
        var ArrayIdBorrar = [];
        if (sessionStorage.getItem("IdsBorrar")) {
            var array = sessionStorage.getItem("IdsBorrar");
            ArrayIdBorrar = JSON.parse(array);
        }
        ArrayIdBorrar.push(IdBorrar);
        sessionStorage.setItem("IdsBorrar", JSON.stringify(ArrayIdBorrar));
        if (rowCount < 1)
            alert('No se puede eliminar el encabezado');
        else
            table.deleteRow(rowCount);
        CalcularTotal();
    })    
}

function editarFila(nodo) {
    var nodoTd = nodo.parentNode.parentNode; //Nodo TD
    var nodoTr = nodoTd.parentNode; //Nodo TR
    var nodosEnTr = nodoTr.getElementsByTagName('td');
    var ProductDetailId = nodoTd.parentElement.attributes[0].value;
    var userObj = {
        producto: nodosEnTr[0].textContent,
        fabricante: nodosEnTr[1].textContent,
        presentacion: nodosEnTr[2].textContent,
        cantidad: nodosEnTr[3].textContent,
        Precio: nodosEnTr[4].textContent,
        PrecioTotal: nodosEnTr[5].textContent,
        PresentationId: nodoTd.parentElement.attributes[1].value,
    };
    var nuevoCodigoHtml = '';
    $.ajax({
        url: "/Sales/ListaPresentacion",
        data: '{ProductDetailId: ' + JSON.stringify(ProductDetailId) + '}',
        type: "POST",
        async: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            nuevoCodigoHtml += '<td>' + userObj.producto + '</td>';
            nuevoCodigoHtml += '<td>' + userObj.fabricante + '</td>';
            nuevoCodigoHtml += '<td><select id="PresentationIdedit" >';
            $.each(result, function (key, item) {
                if (item.PresentationId == userObj.PresentationId)
                    nuevoCodigoHtml += '<option value="' + item.PresentationId + '" selected>' + item.NombrePresentacion + ' x ' + item.CantPresentacion + '</option>';
                else
                    nuevoCodigoHtml += '<option value="' + item.PresentationId + '" >' + item.NombrePresentacion + ' x ' + item.CantPresentacion + '</option>';
            });
            nuevoCodigoHtml += '</select> </td>';
            nuevoCodigoHtml += '<td><input type="text" name="cantidad" id="cantidadedit" value="' + userObj.cantidad + '" size="10"></td>';
            nuevoCodigoHtml += '<td>' + userObj.Precio + '</td>';
            nuevoCodigoHtml += '<td>' + userObj.PrecioTotal + '</td>';
            nuevoCodigoHtml += '<td><center><a href="#" onclick="actualizar(this)">Actualizar</a></center></td>';
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    nodoTr.innerHTML = nuevoCodigoHtml;
    CalcularTotal();
}

function actualizar(nodo) {
    var nodoTd = nodo.parentNode.parentNode; //Nodo TD
    var nodoTr = nodoTd.parentNode; //Nodo TR
    var nodosEnTr = nodoTr.getElementsByTagName('td');
    var IdPresentation = $('#PresentationIdedit').val();
    var ProductDetailId = nodoTr.attributes[0].value;
    var Cantidad = $('#cantidadedit').val();
    var Precio = 0;
    var res = ValidarExistencias(ProductDetailId, IdPresentation, Cantidad);
    if (res == false) {
        return false;
    }
    $.ajax({
        url: "/Sales/BuscarPrecio",
        data: '{ProductDetailId: "' + ProductDetailId + '", IdPresentacion: "' + IdPresentation + '" }',
        type: "POST",
        async: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            Precio = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    var PrecioTotal = Precio * Cantidad;
    nodoTr.attributes[1].value = $('#PresentationIdedit').val();
    var nuevoCodigoHtml = '';
    var userObj = {
        producto: nodosEnTr[0].textContent,
        fabricante: nodosEnTr[1].textContent,
        cantidad: $('#cantidadedit').val(),
        DescripPresentacion: $('#PresentationIdedit')[0].selectedOptions[0].text,
    };
    nuevoCodigoHtml += '<td>' + userObj.producto + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.fabricante + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.DescripPresentacion + '</td>';
    nuevoCodigoHtml += '<td>' + userObj.cantidad + '</td>';
    nuevoCodigoHtml += '<td>' + Precio + '</td>';
    nuevoCodigoHtml += '<td>' + PrecioTotal + '</td>';
    nuevoCodigoHtml += '<td><center><a href="#" onclick="editarFila(this)">Editar</a>   |   <a href="#" onclick="eliminarFila(this)">Eliminar</a></center></td>';
    nodoTr.innerHTML = nuevoCodigoHtml;
    CalcularTotal();
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
    var nodo = document.getElementById('detalle');
    var entryObj = {
        EntryId: $('#EntryId').val(),
        TerceroId: $('#NombreTercero').val(),
        FechaIngreso: $('#FechaIngreso').val(),
        Aprobado: $('input:radio[name="Aprobado"]:checked').val(),
    };
    var detailObj = {
        ProductDetailId: '',
        EntryDetailId: '',
        NombreProducto: '',
        NombreFabricante: '',
        Cantidad: '',
        Lote: '',
        FechaVence: '',
    };
    var arraydetail = [];
    for (var i = 0; i < nodo.rows.length; i++) {
        arraydetail.push({
            ProductDetailId: nodo.rows[i].attributes[0].value,
            EntryDetailId: nodo.rows[i].attributes[1].value,
            NombreProducto: nodo.rows[i].cells[0].innerText,
            NombreFabricante: nodo.rows[i].cells[1].innerText,
            Cantidad: nodo.rows[i].cells[2].innerText,
            Lote: nodo.rows[i].cells[3].innerText,
            FechaVence: nodo.rows[i].cells[4].innerText
        });
    }
    var array = sessionStorage.getItem("IdsBorrar");
    var IdBorrar = JSON.parse(array);
    $.ajax({
        url: "/Entries/Crear",
        data: '{DetalleEntrada: ' + JSON.stringify(arraydetail) + ', Entradas: ' + JSON.stringify(entryObj) + ', IdABorrar: ' + JSON.stringify(IdBorrar) + '}',
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
                        sessionStorage.clear();
                        loadData();
                        $('#myModal').modal('hide');
                        $('.modal-backdrop').remove();
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
    $('#ProductDetailId').val("");
    $('#CodBarras').val("");
    $('#NombreProducto').val("");
    $('#NombreFabricante').val("");
    $('#PresentationId').html("");
    $('#Precio').val("");
    $('#Cantidad').val("");
    $('#PrecioTotal').val("");
    $('#CodBarras').css('border-color', 'lightgrey');
    $('#Precio').css('border-color', 'lightgrey');
    $('#btnAdd').show();    
}

function buscarProduct() {
    var CodBarra = $('#CodBarras').val();
    if (CodBarra.length == 13) {
        $.ajax({
            url: "/Sales/BuscarProducto",
            data: '{CodBarras: "' + CodBarra + '" }',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                $('#ProductDetailId').val(result.ProductDetailId);
                $('#NombreProducto').val(result.NombreProducto);
                $('#NombreFabricante').val(result.NombreFabricante);
                var html = '';
                $.each(result.ListaPresentacion, function (key, item) {
                    html += '<option value="' + item.PresentationId + '" >' + item.NombrePresentacion + ' x ' + item.CantPresentacion + '</option>';
                });
                $('#PresentationId').html(html);
                $('#Precio').val(result.Precio);
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

function getbyID(ID) {
    $('#NombreTercero').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Entries/getbyID/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#EntryId').val(result.EntryId);
            $('#NombreProducto').val(result.NombreProducto);
            if (result.Aprobado == "S")
                $('#S').attr('checked', true);
            else
                $('#N').attr('checked', true);
            var html = '';
            $.each(result.ListaTerceros, function (key, item) {
                if (result.TerceroId == item.TerceroId) {
                    html += '<option value="' + item.TerceroId + '" selected>' + item.NombreTercero + '</option>';
                }
                else {
                    html += '<option value="' + item.TerceroId + '" >' + item.NombreTercero + '</option>';
                }
            });
            $('#NombreTercero').html(html);
            $("#FechaIngreso").val(result.FechaIngreso);
            var htmldetail = '';
            $.each(result.ListaEntradas, function (key, item) {
                htmldetail += '<tr ProductDetailId="' + item.ProductDetailId + '" EntryDetailId = "' + item.EntryDetailId + '">';
                htmldetail += '<td>' + item.NombreProducto + '</td>';
                htmldetail += '<td>' + item.NombreFabricante + '</td>';
                htmldetail += '<td>' + item.Cantidad + '</td>';
                htmldetail += '<td>' + item.Lote + '</td>';
                htmldetail += '<td>' + item.FechaVence + '</td>';
                htmldetail += '<td><center><a href="#" onclick="editarFila(this)">Editar</a>   |   <a href="#" onclick="eliminarFila(this)">Eliminar</a></center></td>';
                htmldetail += '</tr>';
            });
            $('#detalle').html(htmldetail);
            $('#myModal').modal('show');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;

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

function limpiarVar() {
    sessionStorage.clear();
}

function buscarPrecio() {
    var ProductDetailId = $('#ProductDetailId').val();
    var IdPresentacion = $('#PresentationId').val();
    $.ajax({
        url: "/Sales/BuscarPrecio",
        data: '{ProductDetailId: "' + ProductDetailId + '", IdPresentacion: "' + IdPresentacion + '" }',
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#Precio').val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    if ($('#Cantidad').val().trim() != "")
        $('#PrecioTotal').val() = $('#Cantidad').val() * $('#Precio').val();
}

function CalcValor() {
    var valor1 = $('#Cantidad').val();
    var valor2 = $('#Precio').val();
    total = valor1 * valor2;
    $('#PrecioTotal').val(total);
}

function Delete(Id) {
    Swal.fire({
        title: "Estimado Usuario",
        text: "Esta seguro(a) que desea eliminar registro?, recuerde que se elimina el detalle del ingreso.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: '#d33',
        confirmButtonText: "Si",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.value == true) {
            $.ajax({
                url: "/Entries/Borrar/" + Id,
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

function ValidarExistencias(ProductDetailId, PresentacionID, Cantidad) {
    var isValid = true;
    $.ajax({
        url: "/Sales/ValidarExitencias",
        data: '{ProductDetailId: "' + ProductDetailId + '", PresentacionID: "' + PresentacionID + '", Cantidad: "' + Cantidad + '" }',
        type: "POST",
        async: false,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.HayExistencias == false) {
                isValid = false;
                swal.fire({
                    title: "Estimado Usuario",
                    text: "No hay existencias necesarias para el producto en inventario, existen " + response.CantExistente + "unidades.",
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "OK",
                    icon: "error",
                    closeOnConfirm: false
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return isValid;
}

function CalcularTotal() {
    var nodo = document.getElementById('detalle');
    var Total = 0;
    for (var i = 0; i < nodo.rows.length; i++) {
        var valor = Number(nodo.rows[i].cells[5].innerText);
        Total = Total + valor;
    }
    Total = '$' + currencyFormat(Total);
    $('#ValorFactura').val(Total);
}

function currencyFormat(num) {
    return num.toFixed(0).replace(/(\p)(?=(\d{3})+(?!\d))/g, '$1,')
}
    