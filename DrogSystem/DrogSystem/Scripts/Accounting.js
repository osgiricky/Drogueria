$(document).ready(function () {
    loadData();
});


//Load Data function  
function loadData() {
    $.ajax({
        url: "/Accountings/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.FechaCierre + '</td>';
                html += '<td>' + item.Ingresos + '</td>';
                html += '<td>' + item.Egresos + '</td>';
                html += '<td>' + item.BaseCaja + '</td>';
                html += '<td><center><a href="#" onclick="return getbyID(' + item.ContabilidadId + ')">Detalles</a></center></td>';
                html += '</tr>';
            });
            $('#maestro').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
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
    $.ajax({
        url: "/Accountings/DatosCierre",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#FechaCierre').val(result.FechaCierre);
            $('#Ingresos').val(currencyFormat(result.Ingresos.toString()));
            $('#Egresos').val(currencyFormat(result.Egresos.toString()));
            $('#BaseInicial').val(currencyFormat(result.BaseInicial.toString()));
            if (result.BaseInicial == 0)
                document.getElementById("BaseInicial").disabled = false;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
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
