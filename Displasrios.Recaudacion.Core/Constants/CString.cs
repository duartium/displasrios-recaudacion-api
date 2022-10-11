﻿namespace Displasrios.Recaudacion.Core.Constants
{
    public static class CString
    {
        public const string EMAIL_TEMPLATE = "<div style='background-color:#F6EABE;color:#555;width:80%; height:430px; margin:0 auto; border-bottom:5px solid #000;font-family:Arial, Helvetica, sans-serif;'><div style='width:100%;height:170px;background-color:#000;text-align:center'><img src='https://i.imgur.com/jSADthV.png'width='170' height='170'/></div><div style='background-color:#000;height:70px;padding:3px;border-top:1px #fff solid'><h2 style='color:#fff;text-align:center'>Verificación de correo electrónico<h2></div><section style='padding:20px;'><p style='font-size:20px; line-height:1.9rem; text-align:center'>Código de verificación:</p><div style='font-size:22px;border-radius:25px;background-color:#fff;font-weight:bold;text-align:center;width:80%;margin:0 auto;'><p style='padding:5px 5px 15px'>@code</p></div></section> </div>";

        public const string RECIBO_TEMPLATE = @"<div style='margin: 0;  box-sizing: border-box;  font-family: \'arial\';  font-size: 13px;  color: rgba(0, 0, 0, 0.7)'>  <div style='width: 1024px;  height: 100%;  margin: 20px auto;  border: 1px solid rgba(0, 0, 0, 0.2);  background-color: #f1f1f1;  position: relative;'>  <div style='width: 85%;  height: 100%;  margin: 0 auto;  background-color: #7538b9;'></div>  <div class='receipt' style='position: absolute;  z-index: 999;  width: 50%;  top: 50px;  left: 25%;  box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.2);'>    <header class='receipt_header' style='background:#fff'>      <header class='receipt_header_top' style='background:#fff'>        <h4 class='dark' >DISPLASRIOS</h4>      </header>    </header>    <div class='receipt_receipt' style=' height: 499px;  background-color: #f4f4f4;  display: flex;  flex-direction: column;  padding: 20px 40px;'>      <h1 class='red'>RECIBO DE PAGO<br/></h1>	  <p class='dark'>DISTRIBUIDORA DE PLÁSTICOS LOS RIOS</p>	  <p class='dark'>RUC: 1202085872001</p>	  <p class='dark'>DIR: 9 DE OCTUBRE Y FLORES 504</p>            <div class='receipt_receipt_shipping' style='display: flex;  justify-content: space-around;  font-size: 0.7rem;  padding-bottom: 15px;  border-bottom: 1px solid rgba(0, 0, 0, 0.2);  margin-top: 20px;'>        <div class='receipt_receipt_shipping_left' style='flex: 1;'>          <h4 class='dark'>Datos del Cliente:</h4>          <div class='receipt_receipt_shipping_left_to' style='display: flex;  flex-direction: column;  font-size: 0.6rem;'>            <span>@nombreCliente</span>            <span>@direccion</span>            <span>@email</span>          </div>                  </div>        <div class='receipt_receipt_shipping_right' style='flex: 1;'>          <h4 class='dark'>Nro. Orden: @orderNumber</h4>          <p>COMPROBANTE NRO: @numInvoice</p>		  <p>@fechaEmision</p>        </div>      </div>      <div class='receipt_receipt_items' style='  font-size: 0.7rem;  width: 100%;  margin-top: 10px;'>		@details      </div>      <div class='receipt_receipt_total dark' style='font-size: 0.7rem;  font-weight: 900;  width: 200px;  align-self: flex-end;'>        <div class='receipt_receipt_total_separator' style='width: 190px;  height: 1px;  background-color: rgba(0, 0, 0, 0.2);  margin: auto;'></div>        <div class='receipt_receipt_total_sub' style='padding-top: 12px;'>          <p>Subtotal:</p><p>$@subtotal</p>        </div>        <div class='receipt_receipt_total_tax'>          <p>Iva:</p><p>$@iva</p>        </div>        <div class='receipt_receipt_total_fee'>          <p>Descuento:</p><p>$@descuento</p>        </div>        <div class='receipt_receipt_total_total'>          <p>TOTAL:</p><p>$@total</p>        </div>      </div> </div>  </div></div></div>";
    }
}