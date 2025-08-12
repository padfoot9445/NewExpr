using System.Reflection;

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
    ) => Cases.Select
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
    private static IEnumerable
    <(
        TPredicate Predicate,
        Func<TObject, bool> Result
    )>
    NoReturnToReturnCases<TObject, TAttribute, TPredicate>
    (
        IEnumerable
        <(
            TPredicate Predicate,
            Action<TObject> Result
        )> Cases
    ) => Cases.Select
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
    ) => NoReturnToReturnCases<TObject, TAttribute, Func<TAttribute, bool>>(Cases);
    static Func<TObject, bool> NoReturnToReturnFunc<TObject>(Action<TObject> Default) => x =>
    {
        Default(x); return default;
    };
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
        foreach ((var Predicate, var Result) in Cases)
        {
            if (Predicate(Accessor(Self))) return Result(Self);
        }
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
    ) => Self.Dispatch(Accessor, x => throw new MatchNotFoundException(), Cases);
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
    ) => Self.Dispatch(Accessor, NoReturnToReturnFunc(Default), NoReturnToReturnCases<TObject, TAttribute>(Cases));
    public static void Dispatch<TObject, TAttribute, TReturn>
    (
        this TObject Self,
        Func<TObject, TAttribute> Accessor,
        params IEnumerable
        <(
            Func<TAttribute, bool> Predicate,
            Action<TObject> Result
        )> Cases
    ) => Self.Dispatch(Accessor, NoReturnToReturnCases(Cases));
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
    ) => Self.Dispatch(Accessor, Default, SwitchToDispatch(Comparer, Cases));
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
    ) => Self.Switch(Accessor, Comparer, Cases);
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
    ) => Self.Switch(Accessor, NoReturnToReturnFunc(Default), Comparer, NoReturnToReturnCases<TObject, TAttribute, TAttribute>(Cases));
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
    ) => Self.Switch(Accessor, Comparer, Cases);
}