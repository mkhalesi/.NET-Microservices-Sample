namespace BasketService.Extensions
{
    public static class ConstantExtension
    {
        public static string GetBasketKey(string userId)
        {
            return $"basket_{userId}";
        }

        public static string GetBasketItemListKey(string userId)
        {
            return $"basketItemList_{userId}";
        }
    }
}
