# DreamingHome
Dreaming Home 梦想家，家装平台

## EF Core概念
- `Model`: 数据模型，一个普通的C#类
- `DbContext`: 与数据库沟通的桥梁，一个数据库对应一个`DbContext`

## 注册`EF Core`服务
```c#
services.AddEntityFrameworkSqlite()
    .AddDbContext<MainContext>(options => options.UseSqlite(Configuration["database:connection"]));
```

## 数据库上下文 `DbContext`
```bash
public class MainContext : DbContext
{
    public MainContext() { }
    public MainContext(DbContextOptions<MainContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        var configuration = builder.Build();
        optionsBuilder.UseSqlite(configuration["database:connection"]);
    }
}
```
这里我遇到一个很奇怪的问题，单纯在`Startup.cs`里面注册EFCore根本不行，运行的时候老是提示我`No database provider`，只能在`DbContext`里面再重写这个`OnConfiguring`，重新配置一遍数据库= =...


## 数据库迁移
创建数据库迁移：
```bash
dotnet ef migrations add InitialCreate -v
```

查看状态：
```bash
dotnet ef migrations list
```

应用迁移来更新数据库：
```bash
dotnet ef database update -v
```