# BctSP

This project has been developed with the .NET Standard 2.1.
It allows you to call stored procedures and functions from the database by simply creating only the interface and methods signature without needing any concrete class to call any stored procedure.
**It can work async and sync. It provides stored procedure and function support for MsSQL, MySQL, PostgreSQL and OracleSQL**

# Installation

BctSP is now available on NuGet. 
If interested, you can easily install it using the NuGet Package Manager. It can be accessed through the link https://www.nuget.org/packages/BctSP.

# Usage

_To utilize the BctSP NuGet package, you can follow these phases:_

- Add BctSP service to your application builder in Program.cs and set BctSpConnectionStringOrConfigurationPath and DatabaseType for database connection, which can be MsSQL, MySQL, PostgreSQL, OracleSQL.

```cs
builder.Services.AddBctSp((x) =>
{
    // x.BctSpConnectionStringOrConfigurationPath = "Server=...";
    x.BctSpConnectionStringOrConfigurationPath = "Databases:ConnectionString";
    x.DatabaseType = BctSpDatabaseType.PostgreSql;
});
```

- Create a request that inherits from the BctSpBaseRequest. The generic argument of the BctSpBaseRequest should be the response type of the request that inherits BctSpBaseResponse.
- The request should have a class attributes whichs arguments should be sp/function name and command type.

```cs
    [BctSpAttribute("GetProductsByPrice", BctSP.Enums.BctSpCommandType.StoredProcedure)]
    public class GetProductsByPriceRequest : BctSpBaseRequest<GetProductsByPriceResponse>
    {
        public decimal ProductPrice { get; set; }
    }
```

_Customized Usage_  
The database configuration can be overridden and customized for each request if needed.

```cs
    [BctSpAttribute("GetProductsByPrice", BctSP.Enums.BctSpCommandType.StoredProcedure,"CustomDatabases:ConnectionString",BctSP.Enums.BctSpDatabaseType.MsSql))]
    public class GetProductsByPriceRequest : BctSpBaseRequest<GetProductsByPriceResponse>
    {
        public decimal ProductPrice { get; set; }
    }

    //[BctSpAttribute("GetProductsByPrice", BctSP.Enums.BctSpCommandType.StoredProcedure,"Server=...",,BctSP.Enums.BctSpDatabaseType.MsSql))]
    //public class GetProductsByPriceRequest : BctSpBaseRequest<GetProductsByPriceResponse>
    //{
    //    public decimal ProductPrice { get; set; }
    //}

```

- Create a response that inherits from the BctSpBaseResponse.

```cs
	public class GetProductsByPriceResponse : BctSpBaseResponse
	{

		public string ProductName { get; set; }

		public decimal ProductPrice { get; set; }

		public string ProductDescription { get; set; }
	}
```

- Create an interface that implements IBctSp and has method signatures.

```cs
	public interface ISampleSP : IBctSp
	{
	 
		List<GetProductsByPriceResponse> GetProductsByPrice(GetProductsByPriceRequest request);
	 
		Task<List<GetProductsByPriceResponse>> GetProductsByPriceAsync(GetProductsByPriceRequest request);

	}
```


- Get the interface with the Dependency Injection pattern.


```cs    
public class WeatherForecastController
{
    private readonly ISampleSP _sampleSp;

    public WeatherForecastController(ISampleSP sampleSp)
    {
        _sampleSp = sampleSp;
    }
```

_Sample Usage_

```cs
        public List<GetProductsByPriceResponse> GetProductsByPrice()
        {
            var response = _sampleSp.GetProductsByPrice(new GetProductsByPriceRequest{ ProductPrice = 100 });

            return response;
        }

        public async Task<List<GetProductsByPriceResponse>> GetProductsByPriceAsync()
        {
            var response = await _sampleSp.GetProductsByPriceAsync(new GetProductsByPriceRequest{ ProductPrice = 100 });

            return response;
        }
```

------------

***Contributors***

<a href="https://github.com/adessoTurkey-dotNET/Bctsp/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=adessoTurkey-dotNET/Bctsp" />
</a>

------------

