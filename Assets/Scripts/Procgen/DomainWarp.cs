using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DomainWarp
{
    public static void Warp(FastNoiseLite noise, ref float x, ref float y, WarpMode mode)
    {
        switch (mode)
        {
            case WarpMode.Basic:
                SimpleWarp(noise, ref x, ref y);
                break;
            case WarpMode.Double:
                DoubleWarp(noise, ref x, ref y);
                break;
            case WarpMode.Triple:
                TripleWarp(noise, ref x, ref y);
                break;
        }
    }

    private static void SimpleWarp(FastNoiseLite noise, ref float x, ref float y)
    {
        float qX = x;
        float qY = y;

        noise.DomainWarp(ref qX, ref qY);
    }

    private static void DoubleWarp(FastNoiseLite noise, ref float x, ref float y)
    {
        float qX = x;
        float qY = y;

        noise.DomainWarp(ref qX, ref qY);


        float rX = x + 4 * qX + 1.7f;
        float rY = y + 4 * qY + 9.2f;

        noise.DomainWarp(ref rX, ref rY);

        x += 4 * rX;
        y += 4 * rY;
    }

    private static void TripleWarp(FastNoiseLite noise, ref float x, ref float y)
    {
        float qX = x;
        float qY = y;

        noise.DomainWarp(ref qX, ref qY);


        float rX = x + 4 * qX + 1.7f;
        float rY = y + 4 * qY + 9.2f;

        noise.DomainWarp(ref rX, ref rY);


        float zX = x + 2 * qX + 3 * rX + 7.6f;
        float zY = y + 2 * qY + 3 * rY + 4.9f;

        noise.DomainWarp(ref zX, ref zY);


        x += 2.5f * rX * zX;
        y += 2.5f * rY * zY;
    }

    public enum WarpMode
    {
        Basic,
        Double,
        Triple
    }
}
