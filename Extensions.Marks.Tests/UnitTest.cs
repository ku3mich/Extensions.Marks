using Extensions.Marks.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Marks.Tests;

public class UnitTest
{
    readonly IServiceProvider serviceProvider;

    public UnitTest()
    {
        var services = new ServiceCollection();
        services.AddMarkedFrom(typeof(UnitTest).Assembly);
        serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void Test1()
    {
        var t = serviceProvider.GetRequiredService<IFoo>();
        Assert.IsType<Foo>(t);
    }
}