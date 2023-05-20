# BctSp

This project has been developed with the .NET Standart 2.1 and allows you to call stored procedure from the database
by simply creating only interface and methods signature without needing any concrete class to call any stored procedure.
**It can work async and sync. It provides stored procedure support for MsSQL and MySQL, function support for PostgreSQL.**


# Installation

BctSP is available in Nuget. Anybody can install it via Nuget Package Manager.

https://www.nuget.org/packages/BctSP/

# Usage


- Add BctSp service to your application builder in Program.cs and set BctSpConnectionStringOrConfigurationPath and DatabaseType for database connection which can be MsSQL or MySQL or PostgreSQL.

```cs
builder.Services.AddBctSp((x) =>
{
    // x.BctSpConnectionStringOrConfigurationPath = "Server=...";
    x.BctSpConnectionStringOrConfigurationPath = "Databases:ConnectionString";
    x.DatabaseType = BctSpDatabaseType.PostgreSql;
});
```

- Create a request that inherits from the BctSpBaseRequest. Generic argument of the BctSpBaseRequest should be the response type of the request that inherits BctSpBaseResponse.

```cs
    public class GetProductsByPriceRequest : BctSpBaseRequest<GetProductsByPriceResponse>
    {
        public GetProductsByPriceRequest(string bctSpName) : base(bctSpName)
        {
        }

        public decimal ProductPrice { get; set; }

    }
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

- Create an interface which implements IBctSp and has method signatures.

```cs
	public interface ISampleSP : IBctSp
	{
	 
		List<GetProductsByPriceResponse> GetProductsByPrice(GetProductsByPriceRequest request);
	 
		Task<List<GetProductsByPriceResponse>> GetProductsByPriceAsync(GetProductsByPriceRequest request);

	}
```


- Create an interface which implements IBctSp and has method signatures.

```cs    
public class WeatherForecastController
{
    private readonly ISampleSP _sampleSp;

    public WeatherForecastController(ISampleSP sampleSp)
    {
        _sampleSp = sampleSp;
    }
```

- Sample Usage

```cs
        public List<GetProductsByPriceResponse> GetProductsByPrice()
        {
            var response = _sampleSp.GetProductsByPrice(new GetProductsByPriceRequest("yourSPname") { ProductPrice = 100 });

            return response;
        }

        public async Task<List<GetProductsByPriceResponse>> GetProductsByPriceAsync()
        {
            var response = await _sampleSp.GetProductsByPriceAsync(new GetProductsByPriceRequest("yourSPname") { ProductPrice = 100 });

            return response;
        }
```


- Customized Usage -- the database configuration can be overridden and customized for each request if it is needed.

```cs
    public List<GetProductsByPriceResponse> GetProductsByPrice()
    {
        var response = _sampleSp.GetProductsByPrice(
            new GetProductsByPriceRequest("yourSPname",
                "CustomDatabases:ConnectionString",
                BctSpDatabaseType.MsSql) { ProductPrice = 100 });

        // var response = _sampleSp.GetProductsByPrice(
        //     new GetProductsByPriceRequest("yourSPname",
        //         "Server=...",
        //         BctSpDatabaseType.MsSql) { ProductPrice = 100 });

        return response;
    }
    
    public async Task<List<GetProductsByPriceResponse>> GetProductsByPriceAsync()
    {
        var response = await _sampleSp.GetProductsByPriceAsync(
            new GetProductsByPriceRequest("yourSPname",
                "CustomDatabases:ConnectionString",
                BctSpDatabaseType.MsSql) { ProductPrice = 100 });

        // var response = await _sampleSp.GetProductsByPriceAsync(
        //     new GetProductsByPriceRequest("yourSPname",
        //         "Server=...",
        //         BctSpDatabaseType.MsSql) { ProductPrice = 100 });

        return response;
    }
```
------------

![MsSql](https://cdn.iconscout.com/icon/free/png-256/free-sql-4-190807.png?f=webp&w=256)
![PostgreSql](https://cdn.iconscout.com/icon/free/png-256/free-postgresql-11-1175122.png)
![MySql](https://cdn.iconscout.com/icon/free/png-256/free-mysql-21-1174941.png)

