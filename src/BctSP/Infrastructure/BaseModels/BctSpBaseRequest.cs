namespace BctSP.Infrastructure.BaseModels
{
    /// <summary>
    /// Base request.
    /// </summary>
    /// <typeparam name="T">BctSpBaseResponse type.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public abstract class BctSpBaseRequest<T> : BctSpCoreRequest where T : BctSpBaseResponse
    {
       
    }
}