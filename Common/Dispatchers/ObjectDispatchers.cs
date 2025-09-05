namespace Common.Dispatchers;

public static class ObjectDispatchers
{
    private static IEnumerable
    <(
        Func<TAttribute, bool> Predicate,
        Func<TObject, TReturn> Result
        )> SwitchToDispatch<TObject, TAttribute, TReturn>
    (
        Func<TAttribute, TAttribute, bool> Comparer,
        IEnumerable
        <(
            TAttribute Value,
            Func<TObject, TReturn> Result
            )> Cases
    )
    {
        return Cases.Select
            <
                (
                TAttribute Value,
                Func<TObject, TReturn> Result
                ),
                (
                Func<TAttribute, bool> Predicate,
                Func<TObject, TReturn> Result
                )
            >
            (x => (y => Comparer(y, x.Value), x.Result));
    }

    private static IEnumerable
        <(
            TPredicate Predicate,
            Func<TObject, bool> Result
            )>
        NoReturnToReturnCases<TObject, TPredicate>
        (
            IEnumerable
            <(
                TPredicate Predicate,
                Action<TObject> Result
                )> Cases
        )
    {
        return Cases.Select
        <
            (
            TPredicate Predicate,
            Action<TObject> Result
            ),
            (
            TPredicate Predicate,
            Func<TObject, bool> Result
            )
        >
        (x => (x.Predicate, y =>
                {
                    x.Result(y);
                    return default;
                }
        ));
    }

    private static IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            Func<TObject, bool> Result
            )>
        NoReturnToReturnCases<TObject, TAttribute>
        (
            IEnumerable
            <(
                Func<TAttribute, bool> Predicate,
                Action<TObject> Result
                )> Cases
        )
    {
        return NoReturnToReturnCases<TObject, Func<TAttribute, bool>>(Cases);
    }

    private static Func<TObject, bool> NoReturnToReturnFunc<TObject>(Action<TObject> Default)
    {
        return x =>
        {
            Default(x);
            return false;
        };
    }

    private static IEnumerable
    <(
        TPredicate Predicate,
        Func<TObject, TReturn> Result
        )> ValToFunc<TObject, TPredicate, TReturn>(
        IEnumerable
        <(
            TPredicate Predicate,
            TReturn Result
            )> Cases
    )
    {
        return Cases.Select
            <
                (
                TPredicate Predicate,
                TReturn Result
                ),
                (
                TPredicate Predicate,
                Func<TObject, TReturn> Result
                )
            >
            (x => (x.Predicate, y => x.Result));
    }


    public static Func<TObject, TReturn> GetDefault<TObject, TReturn>()
    {
        return _ => throw new MatchNotFoundException();
    }
    public static Action<TObject> GetDefault<TObject>()
    {
        return x =>
        {
            GetDefault<TObject, bool>()(x);
        };
    }


    public static TReturn Dispatch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TObject, TReturn> Default,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            Func<TObject, TReturn> Result
            )> Cases
    )
    {
        foreach (var (Predicate, Result) in Cases)
            if (Predicate(Accessor(Self)))
                return Result(Self);
        return Default(Self);
    }

    public static TReturn Dispatch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            Func<TObject, TReturn> Result
            )> Cases
    )
    {
        return Self.Dispatch(Accessor, GetDefault<TObject, TReturn>(), Cases);
    }

    public static void Dispatch<TObject, TAttribute>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Action<TObject> Default,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            Action<TObject> Result
            )> Cases
    )
    {
        Self.Dispatch(Accessor, NoReturnToReturnFunc(Default), NoReturnToReturnCases(Cases));
    }

    public static void Dispatch<TObject, TAttribute>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            Action<TObject> Result
            )> Cases
    )
    {
        Self.Dispatch(Accessor, NoReturnToReturnCases(Cases));
    }

    public static TReturn Switch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TObject, TReturn> Default,
        Func<TAttribute, TAttribute, bool> Comparer,
        params IEnumerable
        <(
            TAttribute Value,
            Func<TObject, TReturn> Result
            )> Cases
    )
    {
        return Self.Dispatch(Accessor, Default, SwitchToDispatch(Comparer, Cases));
    }

    public static TReturn Switch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TAttribute, TAttribute, bool> Comparer,
        params IEnumerable
        <(
            TAttribute Value,
            Func<TObject, TReturn> Result
            )> Cases
    )
    {
        return Self.Switch(Accessor, GetDefault<TObject, TReturn>(), Comparer, Cases);
    }

    public static void Switch<TObject, TAttribute>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Action<TObject> Default,
        Func<TAttribute, TAttribute, bool> Comparer,
        params IEnumerable
        <(
            TAttribute Value,
            Action<TObject> Result
            )> Cases
    )
    {
        Self.Switch(Accessor, NoReturnToReturnFunc(Default), Comparer,
            NoReturnToReturnCases(Cases));
    }

    public static void Switch<TObject, TAttribute>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TAttribute, TAttribute, bool> Comparer,
        params IEnumerable
        <(
            TAttribute Value,
            Action<TObject> Result
            )> Cases
    )
    {
        Self.Switch(Accessor, GetDefault<TObject>(), Comparer, Cases);
    }

    public static TReturn Switch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TObject, TReturn> Default,
        Func<TAttribute, TAttribute, bool> Comparer,
        params IEnumerable
        <(
            TAttribute Value,
            TReturn Result
            )> Cases
    )
    {
        return Self.Switch(Accessor, Default, Comparer, ValToFunc<TObject, TAttribute, TReturn>(Cases));
    }

    public static TReturn Switch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TAttribute, TAttribute, bool> Comparer,
        params IEnumerable
        <(
            TAttribute Value,
            TReturn Result
            )> Cases
    )
    {
        return Self.Switch(Accessor, GetDefault<TObject, TReturn>(), Comparer, Cases);
    }

    public static TReturn Dispatch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            TReturn Result
            )> Cases
    )
    {
        return Self.Dispatch(Accessor, GetDefault<TObject, TReturn>(), Cases);
    }

    public static TReturn Dispatch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        Func<TObject, TReturn> Default,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            TReturn Result
            )> Cases
    )
    {
        return Self.Dispatch(Accessor, Default, ValToFunc<TObject, Func<TAttribute, bool>, TReturn>(Cases));
    }

    public static TReturn Map<TObject, TReturn>
    (
        this TObject Self,
        params IEnumerable<(TObject, TReturn)> Cases
    )
    {
        return Self.Dispatch(x => x,
            Cases.Select<(TObject, TReturn), (Func<TObject, bool>, Func<TObject, TReturn>)>(x =>
                (y => (x.Item1 is null && y is null) || (y is not null && y.Equals(x.Item1)), _ => x.Item2)));
    }
}