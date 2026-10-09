namespace SimpleDB;

using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;

public record Cheep (string Author, string Observation, long Timestamp);
