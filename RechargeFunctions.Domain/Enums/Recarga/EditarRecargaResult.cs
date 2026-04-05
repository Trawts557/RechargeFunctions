namespace RechargeFunctions.Domain.Enums.Recarga
{
    public enum EditarRecargaResult
    {
        Success,
        RechargeNotFound,
        InvalidAmount,
        InvalidId,
        InvalidClientId,
        InvalidCardId,
        ClientNotFound,
        CardNotFound,
        CardInactive,
    }
}
