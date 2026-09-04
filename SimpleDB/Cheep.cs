using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;

public record Cheep (string Author, string Observation, long Timestamp);
/*{
    [Name("Author")]
    public required string Author { get; set; }

    [Name("Observation")]
    public required string Observation {get; set;} 
    [Name("Timestamp")]
    public required long Timestamp { get; set;}
}*/