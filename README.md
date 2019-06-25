
## MirrorNetworkDiscovery

Network discovery for [Mirror](https://github.com/vis2k/Mirror) based on [in0finite's version](https://github.com/in0finite/MirrorNetworkDiscovery).

## Features

- Simple. Still < 600 lines of code.

- Shattered the code up into more modular scripts, reduced number of variables and static elements focusing more in singleton design, rewrote method and variable names to hopefully make them more self descriptive.

- Uses C#'s UDP sockets for broadcasting and sending responses.

- Independent of current transport. Although as-is works only with Telepathy (to source port data), trivial to extend.

- Single-threaded.

- Tested on: Windows.

- Has a separate NetworkDiscoveryHUD for easy testing.

- Has support for custom response data, with extensible broadcast packet.

- Same style of behaviour as in0finite's setup (client broadcasts, server listens) so efficiency should remain high.

- Prevent detection of multiple localhost servers (by assigning GUID to each packet).

## Usage

Attach NetworkDiscovery script (and for testing NetworkDiscoveryHUD script!) to NetworkManager's game object.

For more details on how to use it, check out NetworkDiscoveryHUD script.

NetworkDiscovery script contains three public methods that drive everything:
        
	    // I call this from my NetworkManage::OnStartServer
        public bool ServerPassiveBroadcastGame(byte[] serverBroadcastPacket)
		
	    // I call this when the Lobby screen is loaded in my game, it causes clients to periodically broadcast a message for any listening servers to respond to
        public bool ClientRunActiveDiscovery()
		
	    // I call this when I leave the lobby menu and in my override of NetworkManager::OnStopServer (not done consistently via the simple HUD as wanted to avoid adding NetworkManager override to sample)
        public void StopDiscovery() {

## Optional TODO's that were beyond my scope of interest

- Targetted broadcast feature removed for simplicity (available on master), I have a separate direct connect UI in my game so I didn't want to keep this

- Update server broadcast packet when game changes

- Measure ping - requires that all socket operations are done in a separate thread, or using async methods

## Thanks

I wrote this for my own usage and wanted to share it as thanks to in0finite for his excellent work.
