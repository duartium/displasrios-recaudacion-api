namespace Displasrios.Recaudacion.Core.Enums
{
    public enum OrderStage
    {
        PENDIENTE_PAGO = 1,
        PAGADO = 2
    }

    public enum MetodoPago
    {
        CONTADO = 1,
        CREDITO = 1019
    }

    public enum TipoIdentificacion { 
        C = 1, //Cédula
        R = 2, //Ruc
        P = 3 //Pasaporte
    }
}
