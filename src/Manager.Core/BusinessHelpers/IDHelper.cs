namespace Manager.Core.BusinessHelpers
{
    public static class IDHelper
    {
        public static bool IsValidID(this long id)
            => id > 0;

        public static bool IsInvalidID(this long id)
            => id > 0;
    }
}
