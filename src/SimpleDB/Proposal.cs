namespace SimpleDB;

using System;
using System.Collections.Specialized;
using System.Net;
using System.ComponentModel.DataAnnotations.Schema;
public record Proposal (string Author, string TaxonId, long Timestamp, long ObservationId);