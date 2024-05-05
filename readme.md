# Exception-less Function Result

This repo compares basic implementation of an exception-less control flow in C#.
Ths means that an function returns a data structure expressing success or failure of its executing without throwing an exception in case on an error.

## Similarities in Functionality

All implementation provide the same features:

1. Any function call returns a data structure which can be inspected if it was successful or not (`HasValue' )
2. A successful function call returns teh result value in property `Value`
3. In case of failure `HasValue` is false and there is a property `Reason` holding an exception describing the reason of the error.
4 In case of failure property `Value` throws an exception having the error reason as an inner exception

## Differences in Implementation

Using an interface as a generalization of the `Ok<T>` and `Error<T>` results allows to specialize the actual implementations of `OK` and `Error` to hold only a minimal required amount of state while the `ref struct` implementation has to carry the data for both cases (a field for T and a reference to an optional `exception`of the error reason) in addition to a boolean separating the good and the bad case.

The implementation differ in their technical means:

1. Project 'RefStruct': `Result<T>` is a `ref struct`
2. Project 'Struct': `Result<T>` is an interface with default implementations which has two `record` structs  `Ok<T>` and `Error<T>` deriving from the interface
3. Project 'Class': `Result<T>` is an interface with default implementations which has two `record` classes  `Ok<T>` and `Error<T>` deriving from the interface

These implementations are pretty basic and have the goal to compare the different solutions performance with a non-exception-less implementation returning just an integer value directly.  

On my machine the following result have been collected (dotnet 8.0.202):

| Method                     | Mean      | Error     | StdDev    | Comment |
|--------------------------- |----------:|----------:|----------:|-|
| Measure_ReturnOne          | 0.0000 ns | 0.0000 ns | 0.0000 ns |this is the base line: directly return 1|
| Measure_ReturnRefStructOne | 0.0009 ns | 0.0011 ns | 0.0009 ns |Returns ref struct holding 1|
| Measure_ReturnStructOne    | 1.3110 ns | 0.0369 ns | 0.0410 ns |Returns a record struct holding 1|
| Measure_ReturnClassOne     | 1.4998 ns | 0.0340 ns | 0.0318 ns |Returns a record class holding 1|

Most lightweight implementation is the `ref struct` as expected.
Using a `record class` or an `record struct` is more costly.
This is expected because they have to be allocated at the heap.
The cost the roughly the same, using a struct instead of a class isn't an advantage.

Also the additional amount of memory required to store the `ref struct` (sizeof(T))+sizeof(bool)+sizeof(reference-to-exception) doesn'T increase the amount of time used because all allocation happens on the stack.
The heap os used only in the error case which isn't necessarily an execution path which has to perform well.

Accessing the `Value` after checking `HasValue` shows an even clearer picture:

| Method                     | Mean      | Error     | StdDev    | Median    | Comment |
|--------------------------- |----------:|----------:|----------:|----------:|-|
| Measure_ReturnOne          | 0.0006 ns | 0.0011 ns | 0.0010 ns | 0.0000 ns |this is the base line: directly return 1|
| Measure_ReturnRefStructOne | 0.0016 ns | 0.0026 ns | 0.0024 ns | 0.0000 ns |Returns ref struct holding 1|
| Measure_ReturnStructOne    | 3.1270 ns | 0.0648 ns | 0.0575 ns | 3.1376 ns |Returns a record struct holding 1|
| Measure_ReturnClassOne     | 4.0354 ns | 0.0313 ns | 0.0293 ns | 4.0369 ns |Returns a record class holding 1|

The interface-based implementations now require multiple milliseconds now to return the value.
Also the `class` -based implementation is now significantly slower than the `struct`-based.
Clear winner remains the `ref struct`, still almost as fast as the plain raw value access.
