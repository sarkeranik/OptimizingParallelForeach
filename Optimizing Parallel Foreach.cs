
public async Task<List<int>> AsyncParallelVersion100() => await AsyncParallelVersion(100);

public async Task<List<int>> AsyncParallelVersion(int batches) //batches = threads numbers
{
    var list = new List<int> ();
    var allTasks = Enumerable.Range(0, TaskÇount)
        .Select ( -> new Func<Task<int>> () => TheMethodYouWantToRunParaller(Httpclient))).ToList();
    await ParallelForEachAsync(allTasks, batches, async func =>
    {
        list.Add(await func());
    });
    return list;
}

public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
{
    async Task AwaitPartition(IEnumerator<T> partition)
    {
        using (partition)
        {
            while (partition.MoveNext())
            { await body(partition.Current); }
        }
    }
    return Task.WhenAll(
        Partitioner
            .Create(source)
            .GetPartitions(dop)
            .AsParallel()
            .Select(p => AwaitPartition(p)));
}
