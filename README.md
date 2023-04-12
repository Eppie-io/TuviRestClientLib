# TuviRestClientLib: .NET REST Client

`TuviRestClientLib` is a .NET Standard 2.0 based C# Library that can interact with REST APIs.

## List of the features

- Supports default HTTP Methods (GET, POST, PUT, HEAD, DELETE,  PATCH).
- Transform request and response data (System.Text.Json serialization).
- Dependency Injection Support.

## Example Usage

```C#
using Tuvi.RestClient;

// Defining a client to interact with the API
class Client : Tuvi.RestClient.Client
{

    public Client(HttpClient client)
        : base(client, new Uri("https://host"))
    {}

    public async Task<User> CreateUserAsync(User user)
    {
        var message = new CreateUserMessage();
        await SendAsync(message).ConfigureAwait(false);

        return message.Response.Content;
    }
}
```

```C#
using Tuvi.RestClient;

// API request definition.
class CreateUserMessage 
    : Message<JsonResponse<CreateUserMessage.ResponseContent>,      
              JsonRequest<CreateUserMessage.User>>
{
    public override Uri Endpoint => new Uri("/user", uriKind: UriKind.Relative);
    public override HttpMethod Method => HttpMethod.Post;

    public struct User
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public struct ResponseContent
    {
        public bool Success { get; set; }
    }
}
```
