using NRedisStack;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;
using StackExchange.Redis;

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,password=pass.123");
IDatabase db = redis.GetDatabase();

SearchCommands ft = db.FT();
JsonCommands json = db.JSON();


var yowko = new User
{
    Username = "yowko",
    Email = "yowko@yowko.com",
    Rank = 99,
    Birthday = new DateTime(1983, 07, 29)
};


var key = yowko.Username;
var insertResult=json.Set(key, "$", yowko);

Console.WriteLine(insertResult);

// 在 json 資料上以 Username 與 Email 建立 primaryIndex index
ft.Create("primaryIndex", new FTCreateParams().On(IndexDataType.JSON)
    ,
    new Schema().AddTextField(new FieldName("$.Username", "Username"))
        .AddTextField("$.Email"));
                    
// 在 primaryIndex 中指定搜尋 Username 欄位值為 `yowko`
var usernameResult=ft.Search("primaryIndex", new Query("@Username:yowko"));

foreach (var result in usernameResult.ToJson())
{
    Console.WriteLine(result);
}
// 在 primaryIndex 中搜尋 `.com` (包含 Username 與 Email)
var emailResult=ft.Search("primaryIndex", new Query(".com"));

foreach (var result in emailResult.ToJson())
{
    Console.WriteLine(result);
}


public class User
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Rank { get; set; }
    public DateTime Birthday { get; set; }
}