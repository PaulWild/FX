using System;
using System.Collections.Generic;

namespace Fx
{
    public sealed class Maybe<T>
    {
        private bool HasItem { get; }
        
        private T Value { get; }
        
        private Maybe()
        {
            HasItem = false;
        }

        private Maybe(T value)
        {
            Value = value;
            HasItem = true;
        }

        public static Maybe<T> None()
        {
            return new Maybe<T>();
        }
        
        public static Maybe<T> Some(T value)
        {
            return new Maybe<T>(value);
        }

        public Maybe<TResult> Select<TResult>(Func<T, TResult> f)
        {
            return !HasItem 
                ? Maybe<TResult>.None() 
                : Maybe<TResult>.Some(f(Value));
        }
        
        public Maybe<TResult> SelectMany<TResult>(Func<T, Maybe<TResult>> f)
        {
            return !HasItem
                ? Maybe<TResult>.None()
                : f(Value);
        }

        public TResult Match<TResult>(Func<T, TResult> ok, Func<TResult> nothing)
        {
            return HasItem 
                ? ok(Value) 
                : nothing();
        }
   
        private bool Equals(Maybe<T> other)
        {
            return HasItem == other.HasItem && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Maybe<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (HasItem.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }
    }
}