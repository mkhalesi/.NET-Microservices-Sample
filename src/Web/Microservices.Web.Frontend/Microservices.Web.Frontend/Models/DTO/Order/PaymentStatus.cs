namespace Microservices.Web.Frontend.Models.DTO.Order
{
    public enum PaymentStatus
    {
        /// <summary>
        /// پرداخت نشده
        /// </summary>
        unPaid = 0,

        /// <summary>
        /// درخواست پرداخت ثبت شده
        /// </summary>
        RequestPayment = 1,

        /// <summary>
        /// پرداخت شده است
        /// </summary>
        isPaid = 2,
    }
}