using System;

namespace Store;
public class Jewelry
{
    public string Title { get; }

    public int Id { get; }

    public Jewelry(int id, string title)
    {
        Title = title;
        Id = id;
    }
}
