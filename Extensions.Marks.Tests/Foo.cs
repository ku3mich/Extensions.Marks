using Marks.Annotations;

namespace Extensions.Marks.Tests;

[Mark(By.Register, Injects.Singleton, As.Type, typeof(IFoo))]
public class Foo : IFoo
{
}
