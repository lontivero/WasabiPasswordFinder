# Install:

## Get The Requirements

1. Get Git: https://git-scm.com/downloads
2. Get .NET Core 2.2 SDK: https://www.microsoft.com/net/download
  
## Get and build this software from source code

```sh
git clone https://github.com/lontivero/WasabiPasswordFinder.git
```

## Usage

```
dotnet run {encryptedSecret} {password}
``` 
You can find your encryptedSecret in your `Wallet.json` file, that you have previously created with Wasabi.

## NOTE

This process is rather slow and CPU heavy. Even for a 10 chars length password it can take significant time to run and
finding the error is not warranted in any case. Please review the code before running it.