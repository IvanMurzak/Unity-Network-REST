# Unity Network REST

![npm](https://img.shields.io/npm/v/extensions.unity.network) [![openupm](https://img.shields.io/npm/v/extensions.unity.network?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/extensions.unity.network/) ![License](https://img.shields.io/github/license/IvanMurzak/Unity-Network-REST) [![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/badges/StandWithUkraine.svg)](https://stand-with-ukraine.pp.ua)

REST plugin for client app/game to communicate with single or multiple remote servers using SOLID principles and clean code. Only JSON format is supported for data sending/receiving.

### Features
- :white_check_mark: Supported REST requests
  - ✔️ GET
  - ✔️ POST
  - ✔️ PUT
  - ✔️ DELETE
- :white_check_mark: JSON serialization/deserialization
- :white_check_mark: Headers control
- :white_check_mark: Requests in a dedicated background thread

# How to install - Option 1 (RECOMMENDED)

- Install [ODIN Inspector](https://odininspector.com/)
- Install [OpenUPM-CLI](https://github.com/openupm/openupm-cli#installation)
- Open the command line in Unity project folder
- `openupm add extensions.unity.network`

# How to install - Option 2

- Install [ODIN Inspector](https://odininspector.com/)
- Add this code to <code>/Packages/manifest.json</code>
```json
{
  "dependencies": {
    "extensions.unity.network": "1.4.3",
  },
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "extensions.unity",
        "com.cysharp",
        "com.neuecc"
      ]
    }
  ]
}
```

# How to use
## STEP 1: Create Server representation as ScriptableObject instance
Create a class for representing the server, let's call it ``RemoteServerSO``. Extend the class from ``NetworkSO``. Press the right mouse click on the project in Unity Editor, and create a new instance of server representation using the menu: ``Tools/Remote Server``. Select the instance and put it into the server endpoint. We will use the instance for sending requests to the server.
```C#
[CreateAssetMenu(fileName = "RemoteServer", menuName = "Tools/Remote Server", order = 0)]
public class RemoteServerSO : NetworkSO
{

}
```

## STEP 2: Create request
Let's imagine the server by the Endpoint ``api/data`` returns the JSON.

<details>
    <summary>JSON body - response from a server</summary>
<pre><code lang="json">{
    "title": "My Title",
    "description": "Some description is here"
}</pre></code>
</details>

<details>
    <summary>C# representation of the server data response</summary>
<pre><code lang="CSharp">{
    [OdinSerialize] public string title { get; set; }
    [OdinSerialize] public string description { get; set; }
}</pre></code>
</details>

Each unique request in REST API should be represented as a C# class. Let's create one GET request as an example. Need to override ``Endpoint`` for this specific request.
```C#
public class GetDataRequest : RequestGet<Data>
{
    protected override string Endpoint => $"api/data";

    public GetDataRequest(RemoteServerSO remote) : base(remote) { }
}
``` 
### Optional data processing
If needed to process received data from the request, need to override ``OnDataReceived``
```C#
public class GetDataRequest : RequestGet<Data>
{
    protected override string Endpoint => $"api/data";

    public GetDataRequest(RemoteServerSO remote) : base(remote) { }
    
    protected override UniTask OnDataReceived(Data data)
    {
        // doing something with data
        return base.OnDataReceived(data);
    }
}
``` 

## STEP 3: Send request
Creating request instance and providing server instance
```C#
var request = new GetRemoteConfigs(remoteServer);
```

### Option 1 - just send a request
```C#
request.SendRequest().Forget();
```

### Option 2 - send request and wait for a response with valid data deserialization
```C#
var data = (await request.SendRequest()).ResponseData;
```

### Option 3 - send and subscribe on callback
```C#
request.SubscribeOnSuccess(data =>
{
    // doing something with data
}, this).SendRequest().Forget();
```

# Request callbacks
Subscription should be done before calling ``SendRequest()``
```C#
// Response received, data successfully serialized from JSON to C# object
request.SubscribeOnSuccess(data =>
{
    // doing something with data
}, this);

// Response received, raw JSON data provided
request.SubscribeOnSuccessRaw(rawJson =>
{
    // doing something
}, this);

// Response received, JSON can't be deserialized for any reason
request.SubscribeOnSerializationError(rawJson =>
{
    // doing something
}, this);

// HTTP error
request.SubscribeOnHttpError(httpError =>
{
    // doing something
}, this);

// Network error, related to an internet connection, can't reach the server at all
request.SubscribeOnNetworkError(networkError =>
{
    // doing something
}, this);

// Progress is a float number in the range from 0.0f to 1.0f
request.SubscribeOnProgress(progress =>
{
    // doing something with data
}, this);

// Request completed with boolean "success" status (true or false)
request.SubscribeOnComplete(success =>
{
    // doing something with data
}, this);
```
