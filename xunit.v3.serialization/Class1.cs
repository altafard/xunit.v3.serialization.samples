using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;
using xunit.v3.serialization;

[assembly: RegisterXunitSerializer(typeof(CustomSerializer), typeof(MyClass))]

namespace xunit.v3.serialization;

public class CustomSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        throw new NotImplementedException();
    }

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        throw new NotImplementedException();
    }

    public string Serialize(object value)
    {
        throw new NotImplementedException();
    }
}

public class UnitTests
{
    public static IEnumerable<TheoryDataRow<MyClass>> Data = new[]
    {
        new TheoryDataRow<MyClass>(new MyClass { Number = 1 }),
        new TheoryDataRow<MyClass>(new MyClass { Number = 2 }),
        new TheoryDataRow<MyClass>(new MyClass { Number = 3 }),
    };

    [Theory]
    [MemberData(nameof(Data))]
    public void Test1(MyClass testCase)
    {
        Assert.True(testCase.Number > 0);
    }
}

public class MyClass
{
    public int Number { get; set; }
}