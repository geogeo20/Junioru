using System;
using System.Threading.Tasks;

public interface IManager
{
    public abstract string Name { get; }
    public abstract Action OnInitialized { get; set; }
    public abstract Task Init();
}
