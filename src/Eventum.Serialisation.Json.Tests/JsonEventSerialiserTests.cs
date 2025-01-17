﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Eventum.Serialisation.Json.TestData;

namespace Eventum.Serialisation.Json.Tests;

public partial class JsonEventSerialiserTests
{
    private readonly JsonEventSerialiser _jsonEventSerialiser;

    public JsonEventSerialiserTests()
    {
        _jsonEventSerialiser = new JsonEventSerialiser();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WhenDataIsNullOrEmpty_Expect_Deserialise_ThrowsArgumentNullException(string data)
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _jsonEventSerialiser.Deserialise<TestObject>(data));
    }

    [Fact]
    public void WhenDataIsValid_Expect_Deserialise_ReturnsCorrectObject()
    {
        // Arrange

        string data = "{\"Property1\": \"Value1\", \"Property2\": \"Value2\"}";
        var expected = new TestObject { Property1 = "Value1", Property2 = "Value2" };

        // Act

        var actual = _jsonEventSerialiser.Deserialise<TestObject>(data);

        // Assert

        Assert.Equal(expected.Property1, actual.Property1);
        Assert.Equal(expected.Property2, actual.Property2);
    }

    [Fact]
    public void WhenObjectIsNull_Expect_Serialise_ThrowsArgumentNullException()
    {
        // Arrange

        TestObject obj = null;

        // Act & Assert

        Assert.Throws<ArgumentNullException>(() => _jsonEventSerialiser.Serialise(obj));
    }

    [Fact]
    public void WhenObjectIsValid_Expect_Serialise_ReturnsCorrectJson()
    {
        // Arrange

        var obj = new TestObject { Property1 = "Value1", Property2 = "Value2", Property3 = "Value3" };
        var expectedJson = "{\"Property1\":\"Value1\",\"Property2\":\"Value2\"}";

        // Act

        var actualJson = _jsonEventSerialiser.Serialise(obj);

        // Assert

        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void WhenCustomOptionsArePassed_Expect_JsonSerializerOptionsToBeSetCorrectly()
    {
        // Arrange

        var customOptions = new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false
        };
        var serialiser = new JsonEventSerialiser(customOptions);

        // Act

        var options = serialiser.Options;
        
        // Assert
        
        Assert.Equal(JsonIgnoreCondition.WhenWritingNull, options.DefaultIgnoreCondition);
        Assert.False(options.WriteIndented);
    }

    [Fact]
    public void WhenJsonNamingPolicyIsCamelCase_Expect_SerialisedProperties_Are_CamelCase()
    {
        // Arrange

        var customOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var obj = new TestObject { Property1 = "Value1", Property2 = "Value2" };
        var serialiser = new JsonEventSerialiser(customOptions);

        // Act

        var json = serialiser.Serialise(obj);

        // Assert

        var expectedJson = "{\"property1\":\"Value1\",\"property2\":\"Value2\"}";
        Assert.Equal(expectedJson, json);
    }
}