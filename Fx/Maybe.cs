using System;
using System.Collections.Generic;

namespace Fx
{
    /// <summary>
    /// Maybe Monad of Type T
    /// </summary>
    /// <typeparam name="T">Type to enclose is a maybe</typeparam>
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

        /// <summary>
        /// Create a Maybe of Type T, that represents the None.
        /// </summary>
        /// <returns>A None Maybe of Type T</returns>
        public static Maybe<T> None()
        {
            return new Maybe<T>();
        }

        /// <summary>
        /// Create a Maybe Monad with a value
        /// </summary>
        /// <param name="value">Value to enclose</param>
        /// <returns>Maybe of Type T with value</returns>
        /// <exception cref="ArgumentNullException">thrown if null passed in as value</exception>
        public static Maybe<T> Some(T value)
        {
            if (value == null)
                throw new ArgumentNullException();
                
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
        
        public Maybe<TResult> SelectMany<TCollection, TResult>(Func<T, Maybe<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector)
        {
            return !HasItem
                ? Maybe<TResult>.None()
                : collectionSelector(Value).Match(x => Maybe<TResult>.Some(resultSelector(Value, x)), Maybe<TResult>.None);
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