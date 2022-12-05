using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeededRNG : System.IDisposable
{
    private Random.State state;

    private bool disposedValue;

    public SeededRNG(int seed)
    {
        state = Random.state;
        Random.InitState(seed);
    }

    public static SeededRNG Block(int seed) => new SeededRNG(seed);


    protected void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Random.state = state;
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        System.GC.SuppressFinalize(this);
    }
}
