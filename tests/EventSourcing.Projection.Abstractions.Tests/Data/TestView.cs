﻿using EventSourcing.Projection;
using System;
using System.IO;
using System.Text.Json.Serialization;

namespace EventSourcing.Projection.Tests.Data
{
    public class TestView : MaterialisedView
    {
        [JsonPropertyName("field1")]
        public string Field1 { get; set; }

        [JsonPropertyName("field2")]
        public int Field2 { get; set; }

        [JsonPropertyName("field3")]
        public string Field3 { get; set; }

        [JsonPropertyName("field4")]
        public DateTime Field4 { get; set; }
    }
}
