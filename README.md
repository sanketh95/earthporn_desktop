# Earth Porn Desktop Wallpaper App
A multi-platform app which sets the top image on [/r/earthporn](http://www.reddit.com/r/earthporn) as your desktop wallpaper. Available on **Windows**, **MacOS** and **Linux**.

## Installation

1. Install [.NET Core](https://www.microsoft.com/net/core#windows).
2. Clone the repository.
3. Execute the following commands in the root of the repository:

```
dotnet restore
```

That's it. 

## Running the app
To run the app, execute the following command:

```
dotnet run
```

The output should look something like this:

```
App Started
Talking to Reddit...
Reddit data retrieved: [URL]
Downloading file...
File downloaded to: [Path]
Setting desktop wallpaper...
Done!
```

## Deployment
To deploy the app to an executable, you can follow [this guide](https://docs.microsoft.com/en-us/dotnet/articles/core/deploying/).

### Deployment Example
This is a brief example of how to deploy the app to an executable on **Windows**.

Modify the `project.json` file and remove the following line:

```
"type": "platform",
```

Now, add a `runtimes` property on the root JSON object with the Windows 10 platform:

```
"runtimes": {
    "win10-x64": {}
}
```

Execute the following commands in the command line to build and publish the app:

```
dotnet build -r win10-x64
dotnet publish -c release -r win10-x64
```

A **.exe** file, along with all of the required references will be have been created in the directory: `\bin\release\netcoreapp1.0\win10-x64`

## Contributing
All constributions are welcome. Please fork, make your changes and submit a pull request. 