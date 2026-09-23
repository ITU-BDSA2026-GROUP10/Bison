namespace SimpleDB;

using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
public record Comment (string Author, string Observation, long Timestamp, long ObservationId) : Cheep (Author, Observation, Timestamp);