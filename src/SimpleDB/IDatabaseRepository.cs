using System.Reflection.Metadata;

namespace SimpleDB;

interface IDatabaseRepository<T>
{
    public IEnumerable<T> ReadObservation(string path, int? limit = null);

    public IEnumerable<T> ReadDiscussion(string path, long observationId, int? limit = null);

    public void Store(T record, string path, string location);

    public void StoreComment(T record, string observePath, string commentPath, long ObservationID);
}