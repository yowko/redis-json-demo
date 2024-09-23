using System.Text.Json;
using Redis.OM;
using Redis.OM.Modeling;

var provider = new RedisConnectionProvider("redis://:pass.123@localhost:6379");
provider.Connection.CreateIndex(typeof(User));
var users = provider.RedisCollection<User>();

// Insert customer
var redisKey = users.Insert(new User
{
    Username = "yowko",
    Email = "yowko@yowko.com",
    Rank = 99,
    Birthday = new DateTime(1983,07,29)
});

Console.WriteLine(redisKey);

foreach (var yowkoUser in users.Where(a => a.Username == "yowko"))
{
    Console.WriteLine(JsonSerializer.Serialize(yowkoUser));
}


[Document(StorageType = StorageType.Json)]
public class User
{
    [RedisIdField]
    [Indexed]
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Rank { get; set; }
    public DateTime Birthday { get; set; }
}