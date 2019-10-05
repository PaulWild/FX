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

    }
}