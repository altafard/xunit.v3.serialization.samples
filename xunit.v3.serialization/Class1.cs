using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Xunit;
using Xunit.Sdk;
using xunit.v3.serialization;

[assembly: RegisterXunitSerializer(typeof(CustomSerializer), typeof(MyClass))]

namespace xunit.v3.serialization;

public class CustomSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        return new MyClass { Number = int.Parse(serializedValue) };
    }

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        failureReason = "Type is not serializable.";
        return type == typeof(MyClass);
    }

    public string Serialize(object value)
    {
        return ((MyClass)value).Number.ToString();
    }
}

public class MyClassStub : MyClass, IXunitSerializable
{
    public void Deserialize(IXunitSerializationInfo info)
    {
        var json = info.GetValue<string>(nameof(MyClassStub)) ?? throw new Exception("No value.");
        var stub = JsonSerializer.Deserialize<MyClassStub>(json) ?? throw new Exception("Cannot deserialize.");
        Number = stub.Number;
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(MyClassStub), JsonSerializer.Serialize(this));
    }
}

public class UnitTests
{
    public static IEnumerable<TheoryDataRow<MyClass>> Data1 = new[]
    {
        new TheoryDataRow<MyClass>(new MyClass { Number = 1 }),
        new TheoryDataRow<MyClass>(new MyClass { Number = 2 }),
        new TheoryDataRow<MyClass>(new MyClass { Number = 3 }),
    };

    public static IEnumerable<TheoryDataRow<MyClassStub>> Data2 = new[]
    {
        new TheoryDataRow<MyClassStub>(new MyClassStub { Number = 1 }),
        new TheoryDataRow<MyClassStub>(new MyClassStub { Number = 2 }),
        new TheoryDataRow<MyClassStub>(new MyClassStub { Number = 3 }),
    };

    [Theory]
    [MemberData(nameof(Data1))]
    public void Test1(MyClass testCase)
    {
        Assert.True(testCase.Number > 0);
    }

    [Theory]
    [MemberData(nameof(Data2))]
    public void Test2(MyClassStub testCase)
    {
        Assert.True(testCase.Number > 0);
    }
}

public class MyClass
{
    public int Number { get; set; }
}