namespace SimpleDB;
interface IDatabaseRepository<T>
{
<<<<<<< HEAD
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
=======
public IEnumerable<T> Read(int? limit = null);
public void Store(T record);
>>>>>>> main
}