using System;
using Xunit;

namespace Fx.Tests
{
    public class EitherTests
    {
        [Fact]
        public void EitherStringOrInt_WithString_CanBeCreated()
        {
            var sut = Either<int, string>.Right("hello");
            var result = sut.Match(x => throw new Exception(), x => x);
            
            Assert.Equal("hello", result);
        }
        
        [Fact]
        public void EitherStringOrInt_WithInt_CanBeCreated()
        {
            var sut = Either<int, string>.Left(12);
            var result = sut.Match(x => x, x => throw new Exception());
            
            Assert.Equal(12, result);
        }
        
        [Fact]
        public void EitherStringOrInt_Mapped_IsRightBiased()
        {
            
            var sut = Either<int, string>.Right("hello");
            var result = sut
                .Select(x => x.ToUpper())
                .Match( x => throw new Exception(), x => x);
            
            Assert.Equal("HELLO", result);
        }

        [Fact]
        public void EitherStringOrInt_MappedWhenLeft_RetainsLeft()
        {
            
            var sut = Either<int, string>.Left(12);
            var result = sut
                .Select(x => x.ToUpper())
                .Match( x => x, x => throw new Exception());
            
            Assert.Equal(12, result);
        }
        
        [Fact]
        public void EitherStringOrInt_FlatMapped_IsRightBiased()
        {
            
            var sut = Either<int, string>.Right("hello");
            var result = sut
                .SelectMany(x => Either<int,string>.Right(x.ToUpper()))
                .Match( x => throw new Exception(), x => x);

            Assert.Equal("HELLO", result);
        }

        [Fact]
        public void EitherStringOrInt_FlatMappedWhenLeft_RetainsLeft()
        {
            
            var sut = Either<int, string>.Left(12);
            var result = sut
                .SelectMany(x => Either<int,string>.Right(x.ToUpper()))
                .Match( x => x, x => throw new Exception());
            
            Assert.Equal(12, result);
        }

        [Fact]
        public void EitherIntString_CanBeUsedInLinq()
        {
            var sut = Either<int, string>.Right("12");
            var sut2 = Either<int, string>.Right("34");

            var transform =
                from x in sut
                from y in sut2
                select x + y;
            
            var result = transform.Match(x => throw new Exception(), x => x);

            Assert.Equal("1234", result);
        }

    }
    
}