using System;

public abstract class Optional<T>
{
    public abstract bool IsPresent { get; }
    public abstract bool IsEmpty { get; }

    public static Optional<T> OfNullable(T value) {
        return value == null ? new Empty<T>() : new Present<T>(value);
    }

    public static Optional<T> Of(T value) {
        _ = value ?? throw new ArgumentNullException();
        return new Present<T>(value);
    }

    public static Optional<T> OfEmpty() {
        return new Empty<T>();
    }

    public abstract void IfPresent(Action<T> consume);
    public abstract Optional<O> Map<O>(Func<T, O> map);

    public abstract T Get();
    public abstract T GetOrElse(T alternative);
}


public class Present<T> : Optional<T>
{
    public override bool IsPresent => true;
    public override bool IsEmpty => false;

    private readonly T value;

    public Present(T value)
    {
        this.value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override void IfPresent(Action<T> consume)
    {
        consume.Invoke(value);
    }

    public override Optional<O> Map<O>(Func<T, O> map)
    {
        return new Present<O>(map(value));
    }

    public override T Get()
    {
        return value;
    }

    public override T GetOrElse(T alternative)
    {
        return value;
    }
}


public class Empty<T> : Optional<T>
{
    public override bool IsPresent => false;
    public override bool IsEmpty => true;

    public Empty() {}

    public override void IfPresent(Action<T> consume)
    {
        // Do nothing
    }

    public override Optional<O> Map<O>(Func<T, O> map)
    {
        return new Empty<O>();
    }

    public override T Get()
    {
        throw new ArgumentNullException();
    }

    public override T GetOrElse(T alternative)
    {
        return alternative;
    }
}