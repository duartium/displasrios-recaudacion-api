namespace Displasrios.Recaudacion.Core.Enums
{
    public enum OrderStage
    {
        PENDIENTE_PAGO = 1,
        PAGADO = 2,
        ANULADO = 3
    }

    public enum FormaPago
    {
        CONTADO = 1,
        CREDITO = 1019
    }

    public enum TipoIdentificacion { 
        C = 1, //Cédula
        R = 2, //Ruc
        P = 3 //Pasaporte
    }

    public enum CashMovement
    {
        APERTURA = 1,
        CIERRE = 2,
        ARQUEO = 3
    }

    public enum Perfil
    {
        Recaudador = 1,
        Administrador = 2,
        Gerente = 3
    }

}
