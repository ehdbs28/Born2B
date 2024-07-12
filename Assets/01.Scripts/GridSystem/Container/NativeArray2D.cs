using System;
using Unity.Collections;
using Unity.Mathematics;

public struct NativeArray2D<T> : IDisposable where T : struct, IEquatable<T>
{

    private NativeArray<T> _array;
    public int width { get; private set; }
    public int height { get; private set; }
    public int size => width * height;
    public NativeArray<T> array => _array;

    public NativeArray2D(int size, Allocator allocator)
    {

        _array = new NativeArray<T>(size * size, allocator);
        width = size;
        height = size;

    }

    public NativeArray2D(int width, int height, Allocator allocator)
    {

        _array = new NativeArray<T>(width * height, allocator);
        this.width = width;
        this.height = height;

    }

    public T this[int x, int y]
    {
        get
        {

            return _array[y * width + x];

        }
        set
        {

            _array[y * width + x] = value;

        }

    }

    public T this[int2 idx]
    {
        get
        {

            return _array[idx.y * width + idx.x];

        }
        set
        {

            _array[idx.y * width + idx.x] = value;

        }

    }

    public T this[int idx]
    {

        get
        {

            return _array[idx];

        }
        set
        {

            _array[idx] = value;

        }

    }

    public bool Constain(T value)
    {

        foreach(var item in _array)
        {

            if(item.Equals(value)) return true;

        }

        return false;

    }

    public bool CheckOutBounce(int2 idx)
    {

        return idx.x >= width || idx.x < 0 || idx.y >= height || idx.y < 0;

    }

    public void Dispose()
    {

        _array.Dispose();

    }

}
