namespace WvsBeta.Common.Interfaces
{
    public interface IInitialize
    {
        void OnInitialize();
    }
    public static class InitializeExtensions
    {
        public static void Initialize<T>(this T obj) where T : IInitialize
        {
            if (obj.GetType() == typeof(T))
                obj.OnInitialize();
        }
    }
}
