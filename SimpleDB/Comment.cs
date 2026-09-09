
using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
public record Comment (string author, long timestamp, long observationId, string comment) : Cheep (author, timestamp);