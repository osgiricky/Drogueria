$(document).ready(function () {
});


function clearModal() {
    $('#FechaDesde').val("");
    $('#FechaHasta').val("");
    $('#myModal1').modal('show');
}

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
            if (result == 0)
                document.getElementById("NroFactura").disabled = false;
            else
                $("#NroFactura").val(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    CalcularTotal();
}

function IngresoProduct() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var FechaDesde = $('#FechaDesde').val();
    var FechaHasta = $('#FechaHasta').val();
    //$.ajax({
    //    url: "/Sales/RepEntries",
    //    data: '{FechaDesde: "' + FechaDesde + '", FechaHasta: "' + FechaHasta + '" }',
    //    type: "POST",
    //    async: false,
    //    success: function (result) {
    //        Precio = result;
    //    },
    //    error: function (errormessage) {
    //        alert(errormessage.responseText);
    //    }
    //});
    windows.open("/Sales/RepEntries", "_blank");
    $('#detalle').html(html);
    $('#myModal').modal('hide');
    $('.modal-backdrop').remove();
    CalcularTotal();
}

function validate() {
    var isValid = true;
    if ($('#FechaDesde').val().trim() == "") {
        $('#FechaDesde').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FechaDesde').css('border-color', 'lightgrey');
    }
    if ($('#FechaHasta').val().trim() == "") {
        $('#FechaHasta').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#FechaHasta').css('border-color', 'lightgrey');
    }
    return isValid;
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
    var nodoTd = nodo.parentNode.parentNode;
    var nodoTr = nodoTd.parentNode;
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
}

function actualizar(nodo) {
    var nodoTd = nodo.parentNode.parentNode;
    var nodoTr = nodoTd.parentNode;
    var nodosEnTr = nodoTr.getElementsByTagName('td');
    var IdPresentation = $('#PresentationIdedit').val();
    var ProductDetailId = nodoTr.attributes[0].value;
    var Cantidad = $('#cantidadedit').val();
    var Precio = 0;
    var res = ValidarExistencias(ProductDetailId, IdPresentation, Cantidad);
    if (res == false) {
        return false;
    };
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
    Precio = currencyFormat(Precio.toString());
    PrecioTotal = currencyFormat(PrecioTotal.toString());
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


function Add() {
    var nodo = document.getElementById('detalle');
    var entryObj = {
        NroFactura: $('#NroFactura').val(),
        FechaFactura: $('#FechaFactura').val(),
        ValorFactura: currencyANumero($('#ValorFactura').val()),
    };
    var arraydetail = [];
    for (var i = 0; i < nodo.rows.length; i++) {
        arraydetail.push({
            ProductDetailId: nodo.rows[i].attributes[0].value,
            PresentationId: nodo.rows[i].attributes[1].value,
            Quantity: nodo.rows[i].cells[3].innerText,
            PrecioTotal: currencyANumero(nodo.rows[i].cells[5].innerText),
        });
    }
    $.ajax({
        url: "/Sales/Crear",
        data: '{DetalleFactura: ' + JSON.stringify(arraydetail) + ', Factura: ' + JSON.stringify(entryObj) + '}',
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
                    location.href = '/Home';
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

function BuscarXId(ProductDetailId) {
    $.ajax({
        url: "/Sales/BuscarXId",
        data: '{ProductDetailId: "' + ProductDetailId + '" }',
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#CodBarras').val(result.CodBarras);
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
    $('#myModal2').modal('hide');
    $('.modal-backdrop').remove();
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
                url: "/Sales/Borrar/" + Id,
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
        var valor = nodo.rows[i].cells[5].innerText;
        valor = Number(currencyANumero(valor));
        Total = Total + valor;
    }
    Total = Total.toString();
    Total = '$' + currencyFormat(Total);
    $('#ValorFactura').val(Total);
}

function currencyFormat(num) {
    return num.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}

function currencyANumero(num) {
    num = num.replace("$", "");
    num = num.replace(",", "");
    return num;
}

function resume() {
    var nodo = document.getElementById('detalle');
    if (nodo.rows.length > 0) {
        $('#TotalFact').val();
        $('#Efectivo').val();
        $('#Cambio').val();
        Total = $('#ValorFactura').val();
        $('#TotalFact').val(Total);
        $('#myModal1').modal('show');
    }
    else {
        $('#myModal1').modal('hide');
        $('.modal-backdrop').remove();
        Swal.fire({
            title: "Estimado Usuario",
            text: "No existen productos en la factura.",
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "OK",
            icon: "error",
            closeOnConfirm: false
        })
    }

}

function ValidarProduct(ProductDetailId, PresentacionID) {
    var isValid = true;
    var nodo = document.getElementById('detalle');
    for (var i = 0; i < nodo.rows.length; i++) {
        if (ProductDetailId == nodo.rows[i].attributes[0].value) {
            var cantidad = Number(nodo.rows[i].cells[3].textContent);
            cantidad += Number($('#Cantidad').val());
            var res = ValidarExistencias(ProductDetailId, PresentacionID, cantidad);
            if (res == false) {
                isValid = false;
                break;
            }
            var valor = Number(currencyANumero(nodo.rows[i].cells[4].textContent));
            var valorTotal = cantidad * valor;
            nodo.rows[i].cells[3].innerHTML = cantidad.toString();
            nodo.rows[i].cells[5].innerHTML = currencyFormat(valorTotal.toString());
            CalcularTotal();
            $('#myModal').modal('hide');
            $('.modal-backdrop').remove();
            isValid = false;
            break;
        }
    }
    return isValid;
}

function CalcCambio() {
    var Total = $('#TotalFact').val();
    var Efectivo = $('#Efectivo').val();
    Total = Number(currencyANumero(Total));
    var Cambio = Efectivo - Total;
    Cambio = Cambio.toString();
    Cambio = '$' + currencyFormat(Cambio);
    $('#Cambio').val(Cambio);
}

function buscar() {
    var ProductName = {
        NombreProducto: $('#BuscarProducto').val(),
    };
    $.ajax({
        url: "/Sales/BuscarXNombre/",
        data: JSON.stringify(ProductName),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.NombreProducto + '</td>';
                html += '<td>' + item.NombreFabricante + '</td>';
                html += '<td><center><a href="#" onclick="return BuscarXId(' + item.ProductDetailId + ')">Seleccionar</a></center></td>';
                html += '</tr>';
            });
            $('#detalle2').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function clearModal2() {
    $('#BuscarProducto').val("");
    $('#detalle2').html("");
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
