# Exception-less Function Result

This repo compares basic implementation of an exception-less control flow in C#.
Ths means that an function returns a data structure expressing success or failure of its executing without throwing an exception in case on an error.

All implementation provide the same features:

1. Any function call returns a data structure which can be inspected if it was successful or not (`HasValue' )
2. A successful function call returns teh result value in property `Value`
3. In case of failure `HasValue` is false and there is a property `Reason` holding an exception describing the reasin of the error.
4 In case of failure property `Value` throws an exception having the error reason as an inner exception

The implementation differ in their technical means:

1. Project 'RefStruct': Result<T> is a `ref struct`
2. Project 'Struct': Result<T> is an interface with default implementations which has two record structs  `Ok<T>` and `Error<T>` deriving from the interface
3. Project 'Class': Result<T> is an interface with default implementations which has two record classes  `Ok<T>` and `Error<T>` deriving from the interface

These implementations are pretty basic and have the goal to compare the different solutions performance with a non-exception-less implementation returning just an integer value directly.  

On my machine the following result have been collected (dotnet 8.0.202):

| Method                     | Mean      | Error     | StdDev    | Median    | Comment |
|--------------------------- |----------:|----------:|----------:|----------:|-|
| Measure_ReturnOne          | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |this is the base line: directly return 1|
| Measure_ReturnRefStructOne | 0.0007 ns | 0.0013 ns | 0.0011 ns | 0.0001 ns |Returns ref struct holding 1|
| Measure_ReturnStructOne    | 1.4424 ns | 0.0202 ns | 0.0189 ns | 1.4357 ns |Returns a record struct holding 1|
| Measure_ReturnClassOne     | 1.4541 ns | 0.0281 ns | 0.0249 ns | 1.4495 ns |Returns a record class holding 1|

Most lightweight implementation is the `ref struct` as expected.
Using a `record class` or an `record struct` is more costly.
This is expected because they have to be allocated at the heap.
The cost the roughly the same, using a struct here isn't an advantage.