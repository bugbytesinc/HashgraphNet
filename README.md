# HashgraphNet
.NET Core Client Library for Hedera Hashgraph

## Build Status
[![Build Status](https://bugbytes.visualstudio.com/HashgrapNet/_apis/build/status/HashgrapNet%20Continuous%20Build?branchName=master)](https://bugbytes.visualstudio.com/HashgrapNet/_build/latest?definitionId=27&branchName=master)

## Cloning
This project references the [Hedera Protobuf](https://github.com/hashgraph/hedera-protobuf) 
project as a git submodule.  It is recommended to include ```--recurse-submodules``` options 
when cloning the repository so that the ```*.proto``` files from the submodule are present
when building the project:
```
$ git clone --recurse-submodules https://github.com/bugbytesinc/HashgraphNet.git
```

## Build Requirements
This project relies protobuf support found in .net core 3, 
previous versions of the .net core framework will not work.
(At the time of this writing we are in [preview4](https://dotnet.microsoft.com/download/dotnet-core/3.0))

Visual Studio is not required to build the library, however the project
references the [NSec.Cryptography](https://nsec.rocks/) library, which 
loads the libsodium.dll library which relies upon the VC++ runtime. In
order to execute tests, the [Microsoft Visual C++ Redistributable](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads)
must be installed on the build agent if Visual Studio is not.