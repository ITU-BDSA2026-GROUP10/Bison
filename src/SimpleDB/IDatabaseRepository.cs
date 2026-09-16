using System.Reflection.Metadata;

namespace SimpleDB;
interface IDatabaseRepository<T>
{
    public IEnumerable<T> Read(string path, int? limit = null);

    public void Store(T record, string path);

    public void StoreComment(T record, string path, string observePath, long ObservationID);
}