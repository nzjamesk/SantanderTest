Running instructions
====================
The REST service has been configured with Swagger, so just run the project and you will be able to test the GET endpoint by entering a value in the Swagger UI.

Tests
=====
I have included basic model conversion unit tests, as well as controller unit tests. As the provider uses the RestClient directly, it is not viable to unit test this so instead I have created integration tests to call the actual Hacker News API, and to test the behaviour of the provider this way.

Scalability
===========
As each call to this endpoint only calls the hacker news API twice, then a large number of calls will only call Hacker News 2xN times, which I don't consider a risk of overloading the endpoint, especially as the calls have been coded as asynchronous.
Any type of bottlenecking or batched execution would involve maintaining state between calls to share with other callers, which defeats the design of REST and is a larger scalability concern.

Further enhancements
====================
I would of course put configuration of endpoint URLs in the application config, rather than hard coded, and I would implement a full DI framework as default. 
I would also probably use a conversion factory for the conversion between Hackernews JSON and the native JSON objects we want to convert
I would store JSON offline to simulate the HackerNews API calls and use that in further local tests.
Lastly I would separate the business logic of HackerNewsProvider from the data retrieval mechanism, so that the provider logic could also be unit tested, without being reliant on the underlying implementation of the RestClient.

