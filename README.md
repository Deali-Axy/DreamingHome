# DreamingHome
Dreaming Home 梦想家，家装设计分享 & 寻找设计师一站式平台。

## 项目依赖
- .Net Core SDK 2.2
- Asp.Net Core 2.2
- Asp.Net Core Razor 2.2
- EntityFramework 2.2.4
- Swagger 5.0 rc2

## 数据库ER图

![](https://raw.githubusercontent.com/Deali-Axy/BlogImageHosting/master/img/ER.png)

## EF Core概念
- `Model`: 数据模型，一个普通的C#类
- `DbContext`: 与数据库沟通的桥梁，一个数据库对应一个`DbContext`

## 注册`EF Core`服务
```c#
services.AddEntityFrameworkSqlite()
    .AddDbContext<MainContext>(options => options.UseSqlite(Configuration["database:connection"]));
```

## 数据库上下文 `DbContext`
```c#
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

## 引入Swagger支持
### 安装依赖
```nuget
Install-Package Swashbuckle.AspNetCore
```

### 配置中间件
```c#
using Swashbuckle.AspNetCore.Swagger;

// 在Startup.ConfigureServices中配置服务
services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My Api", Version = "v1"}); });

// 在Startup.Configure中配置中间件
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
```

把应用的根路径设置为`Swagger UI`，如下：

```C#
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});
```

### 启动测试

启动应用，并导航到`http://localhost:<port>/swagger/v1/swagger.json`，生成的描述终结点的文档显示如下json格式。

可在 `http://localhost:<port>/swagger` 找到 Swagger UI。 通过 Swagger UI 浏览 API文档。

### 注意

- 每一个`Controller`的方法都需要标注出具体的`Http`方法，不然会报错
- `Swagger`会自动读取每个接口函数的C#文档，但是前提是要生成xml文档才可以

### 自定义以及扩展

修改之前配置的Swagger服务：

```C#
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        info: new OpenApiInfo
        {
            Version = "v1",
            Title = "Dreaming Home 智能家装平台",
            Description = "智能家装平台 Api 文档",
            TermsOfService = new Uri("http://blog.deali.cn"),
            Contact = new OpenApiContact
            {
                Name = "DealiAxy",
                Email = "dealiaxy@gmail.com",
                Url = new Uri("https://zhuanlan.zhihu.com/deali"),
            },
            License = new OpenApiLicense
            {
                Name = "GNU GENERAL PUBLIC LICENSE Version 2",
                Url = new Uri("https://www.gnu.org/licenses/old-licenses/gpl-2.0.html"),
            }
         });
});
```

显示接口的xml文档要先生成，然后在`services.AddSwaggerGen`里面设置才行。

```C#
// 为 Swagger JSON and UI设置xml文档注释路径
//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
var xmlPath = Path.Combine(basePath, "Doc", "DreamingHome.xml");
c.IncludeXmlComments(xmlPath);
```

### 小结

通过上面的操作可以总结出，Swagger UI 显示上述注释代码的 `<summary>` 元素的内部文本作为api大的注释！

当然你还可以将 remarks 元素添加到 `Get` 操作方法文档。 它可以补充 `<summary>` 元素中指定的信息，并提供更可靠的 Swagger UI。 `<remarks>` 元素内容可包含文本、JSON 或 XML。 代码如下：

```C#
 /// <summary>
 /// 这是一个带参数的get请求
 /// </summary>
 /// <remarks>
 /// 例子:
 /// Get api/Values/1
 /// </remarks>
 /// <param name="id">主键</param>
 /// <returns>测试字符串</returns>          
 [HttpGet("{id}")]
 public ActionResult<string> Get(int id)
 {
       return $"你请求的 id 是 {id}";
 }
```

### 描述响应类型

> 摘录自：<https://www.cnblogs.com/yanbigfeg/p/9232844.html>

接口使用者最关心的就是接口的返回内容和响应类型啦。下面展示一下201和400状态码的一个简单例子：

我们需要在我们的方法上添加：
```C#
[ProducesResponseType(201)]
[ProducesResponseType(400)]
```

然后添加相应的状态说明：返回value字符串如果id为空

最终代码应该是这个样子：

```C#
 /// <summary>
 /// 这是一个带参数的get请求
 /// </summary>
 /// <remarks>
 /// 例子:
 /// Get api/Values/1
 /// </remarks>
 /// <param name="id">主键</param>
 /// <returns>测试字符串</returns> 
 /// <response code="201">返回value字符串</response>
/// <response code="400">如果id为空</response>  
 // GET api/values/2
[HttpGet("{id}")]
[ProducesResponseType(201)]
[ProducesResponseType(400)]
public ActionResult<string> Get(int id)
{
     return $"你请求的 id 是 {id}";
}
```

