# FX

Functional extensions for C# shamelessly inspired (/ripped off) of ideas in ploeh's [asynchronous-injection](https://github.com/ploeh/asynchronous-injection)

## Maybe

```csharp
var mInt = Maybe<int>.Some(12); //Some of 12
var mNone = Maybe<int>.None(); //None of Int
```

## Either

```csharp
var eitherIntOrString = Either<int, string>.Left(12); //Left of 12
var eitherIntOrString = Either<int,string>.Right("string"); //Right of "hello"
```
