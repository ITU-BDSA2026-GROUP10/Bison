using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
public record Observations (string Author, string Observation, long Timestamp, long ID) : Cheep (Author, Observation, Timestamp);