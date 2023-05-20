using System;

namespace BctSP.Infrastructure.BaseModels
{
    // ReSharper disable once UnusedTypeParameter
    /// <summary>
    /// BctSpCoreRequest.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BctSpCoreRequest<TResponse> where TResponse : BctSpBaseResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected BctSpCoreRequest()
        {
            if (GetType()!.BaseType!.Name != "BctSpBaseRequest`1")
            {
                throw new Exception("BctSpBaseRequest should be inherited");
            }
        }
    }

    /// <summary>
    /// BctSpCoreRequest
    /// </summary>
    public abstract class BctSpCoreRequest : BctSpCoreRequest<BctSpBaseResponse>
    {
        /// <summary>
        /// BctSpCoreRequest
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected BctSpCoreRequest()
        {
            if (GetType()!.BaseType!.Name != "BctSpBaseRequest`1")
            {
                throw new Exception("BctSpBaseRequest should be inherited");
            }
        }
    }
}