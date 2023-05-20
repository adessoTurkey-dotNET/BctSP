using System;

namespace BctSP.Infrastructure.BaseModels
{
    // ReSharper disable once UnusedTypeParameter
    public abstract class BctSpCoreRequest<TResponse> where TResponse : BctSpBaseResponse
    {
        protected BctSpCoreRequest()
        {
            if (GetType()!.BaseType!.Name != "BctSpBaseRequest`1")
            {
                throw new Exception("BctSpBaseRequest should be inherited");
            }
        }
    }

    public abstract class BctSpCoreRequest : BctSpCoreRequest<BctSpBaseResponse>
    {
        protected BctSpCoreRequest()
        {
            if (GetType()!.BaseType!.Name != "BctSpBaseRequest`1")
            {
                throw new Exception("BctSpBaseRequest should be inherited");
            }
        }
    }
}