using System;
using Xunit;

namespace Fx.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void Maybe_SomeInt_CanBeCreated()
        {
            var sut = Maybe<int>.Some(12);
            var result = sut.Match(x => x, () => throw new Exception());
            
            Assert.Equal(12, result);
        }

        [Fact]
        public void Maybe_NoneInt_CanBeCreated()
        {
            var sut = Maybe<int>.None();
            var result = sut.Match(x => false, () => true);
            
            Assert.True(result);
        }
        
        [Fact]
        public void MaybeInt_MappedToString_IsValid()
        {
            
            var sut = Maybe<int>.Some(12);
            var result = sut
                .Select(x => x.ToString())
                .Match(x => x, () => throw new Exception());
            
            Assert.Equal("12", result);
        }

        [Fact]
        public void MaybeInt_FlatMappedToNone_IsValid()
        {
            Maybe<string> F(int _) => Maybe<string>.None();

            var sut = Maybe<int>.Some(12);
            var result = sut
                .SelectMany(F)
                .Match(x => false, () => true);
            
            Assert.True(result);
        }
        
        
        [Fact]
        public void MaybeInt_FlatMappedToString_IsValid()
        {
            Maybe<string> F(int x) => Maybe<string>.Some(x.ToString());

            var sut = Maybe<int>.Some(12);
            var result = sut
                .SelectMany(F)
                .Match(x => x, () => throw new Exception());
            
            Assert.Equal("12", result);
        }
        
        
        [Fact]
        public void MaybeInt_CanBeUsedInLinq()
        {
            var sut = Maybe<int>.Some(12);
            var transform = 
                from x in sut
                select x * 2;
            
            var result = transform.Match(x => x, () => throw new Exception());

            
            Assert.Equal(24, result);
        }
    }
}