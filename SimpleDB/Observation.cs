using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
public record Observation (string Author, long Timestamp, string observation) : Cheep (Author, Timestamp);