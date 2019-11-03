// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;

#pragma warning disable 693

namespace Fx
{
    public sealed class Either<L,R>
    {
        private bool IsRight { get; }
        private L LeftValue { get; }
        
        private R RightValue { get; }
        
        private Either(L left)
        {
            IsRight = false;
            LeftValue = left;
        }
        
        private Either(R right)
        {
            IsRight = true;
            RightValue = right;
        }

        public static Either<L,R> Left(L value)
        {
            return new Either<L, R>(value);
        }
        
        public static Either<L,R> Right(R value)
        {
            return new Either<L, R>(value);
        }
        
        public TResult Match<TResult>(Func<L, TResult> left, Func<R, TResult> right)
        {
            return IsRight
                ? right(RightValue)
                : left(LeftValue);
        }
        
        public Either<L, TResult> Select<TResult>(Func<R, TResult> f)
        {
            return IsRight
                ? Either<L, TResult>.Right(f(RightValue))
                : Either<L, TResult>.Left(LeftValue);
        }
        
        public Either<L, TResult> SelectMany<TResult>(Func<R, Either<L, TResult>> f)
        {
            return IsRight
                ? f(RightValue)
                : Either<L, TResult>.Left(LeftValue);
        }
        
        public Either<L, TResult> SelectMany<TCollection, TResult>(Func<R, Either<L,TCollection>> collectionSelector, Func<R, TCollection, TResult> resultSelector)
        {
            return !IsRight
                ? Either<L, TResult>.Left(LeftValue)
                : collectionSelector(RightValue).Match(Either<L, TResult>.Left, x => Either<L, TResult>.Right(resultSelector(RightValue, x)));
        }
        
        private bool Equals(Either<L, R> other)
        {
            return IsRight == other.IsRight && EqualityComparer<L>.Default.Equals(LeftValue, other.LeftValue) && EqualityComparer<R>.Default.Equals(RightValue, other.RightValue);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Either<L, R> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsRight.GetHashCode();
                hashCode = (hashCode * 397) ^ EqualityComparer<L>.Default.GetHashCode(LeftValue);
                hashCode = (hashCode * 397) ^ EqualityComparer<R>.Default.GetHashCode(RightValue);
                return hashCode;
            }
        }

    }
}