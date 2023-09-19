// See https://aka.ms/new-console-template for more information
using BookFinder;

var result = new Finder().FindByIsbn("9789643638047");
result.Wait();
if(result.IsCompletedSuccessfully)
Console.WriteLine(result.Result[0].Title);
else
    Console.WriteLine("fail");
