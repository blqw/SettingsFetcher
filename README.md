# SettingsFetcher
用于快速操作设置

## nuget
> Install-Package blqw.SettingsFetcher

## 用法


### 1. 实例对象
`app.config` or `web.config`
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="datetime" value="2017-1-1 0:0:0"/>
    <add key="url" value="//baidu.com"/>
  </appSettings>
</configuration>
```

```cs
class Settings
{
    public Settings() => SettingsFetcher.Fill(this);
    public DateTime DateTime { get; private set; }
    public Uri Url { get; private set; }
}

class Program
{
    static void Main(string[] args)
    {
        var settings = new Settings();
        Console.WriteLine(settings.DateTime); // 2017-1-1 0:0:0
        Console.WriteLine(settings.Url); // http://baidu.com
    }
}
```

### 2. 静态对象
`app.config` or `web.config`
```xml
<add key="datetime" value="2017-1-1 0:0:0"/>
<add key="url" value="//baidu.com"/>
```

```cs
static class Settings
{
    static Settings() => SettingsFetcher.Fill(typeof(Settings));
    public static DateTime DateTime { get; private set; }
    public static Uri Url { get; private set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Settings.DateTime); // 2017-1-1 0:0:0
        Console.WriteLine(Settings.Url); // http://baidu.com
    }
}
```

### 3.带前缀
`app.config` or `web.config`
```xml
<add key="mydemo.datetime" value="2017-1-1 0:0:0"/>
<add key="mydemo.url" value="//baidu.com"/>
```

```cs
[SettingsGroupName("mydemo")]
static class Settings
{
    static Settings() => SettingsFetcher.Fill(typeof(Settings));
    public static DateTime DateTime { get; private set; }
    public static Uri Url { get; private set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Settings.DateTime); // 2017-1-1 0:0:0
        Console.WriteLine(Settings.Url); // http://baidu.com
    }
}
```

### 3.自定义前缀组合
`app.config` or `web.config`
```xml
<add key="mydemo@datetime" value="2017-1-1 0:0:0"/>
<add key="mydemo@url" value="//baidu.com"/>
```

```cs
[SettingsGroupName("mydemo")]
static class Settings
{
    static Settings() => SettingsFetcher.Fill(new SettingsFetcherArgs
    {
        JoinName = (pre, name) => pre == null ? name : $"{pre}@{name}"
    }, this);
    public static DateTime DateTime { get; private set; }
    public static Uri Url { get; private set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Settings.DateTime); // 2017-1-1 0:0:0
        Console.WriteLine(Settings.Url); // http://baidu.com
    }
}
```

### 4. 自定义类型转换
`app.config` or `web.config`
```xml
<add key="user" value="{&quot;name&quot;:&quot;blqw&quot;}"/>
```

```cs
static class Settings
{
    static Settings() => SettingsFetcher.Fill(new SettingsFetcherArgs
    {
        Converter = (value, type) => Json.ToObject(value, type)
    }, this);
    public static User User { get; private set; }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Settings.User.Name); // blqw
    }
}
```

### 5. 自定义设置来源
```cs
static class Settings
{
    static Settings() => SettingsFetcher.Fill(new SettingsFetcherArgs
    {
        Getter = name =>
        {
            return db.Sql($"select value from settings where name = '{name}'").ExecuteScalar<string>();
        }
    }, this);
}
```