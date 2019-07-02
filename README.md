
## MirrorNetworkDiscovery

Network discovery for [Mirror](https://github.com/vis2k/Mirror) based on [in0finite's version](https://github.com/in0finite/MirrorNetworkDiscovery).

## Features

- Simple. Still < 600 lines of code.

- Uses C#'s UDP sockets for broadcasting and sending responses.

- Independent of current transport. Although as-is works only with Telepathy (to source port data), trivial to extend.

- Single-threaded.

- Tested on: Windows.

- Has a separate NetworkDiscoveryHUD for easy testing.

- Has support for custom response data, with extensible broadcast packet.

- Same style of behaviour as in0finite's setup (client broadcasts, server listens) so efficiency should remain high.

## What's different in this fork?

- Prevent detection of multiple localhost servers (by assigning GUID to each packet).

- Binary payload being sent handled in a central message type outside of the NetworkDiscovery code (and not re-serialized on every NetworkDiscovery::Update)

- Signature agnostic of the application version (so you can show lobbys you cant join due to a version mismatch), simplified/optimised handshake

- Cached the NIC data

- Made client send messages aysnc (not changed any other area to minimise complexity), this has the benefit that a local server captures and processes the messages on varying NIC's so tightly that my visuals dont flicker between NIC IPs (although this is quite a lazy solution!)

- Shattered the code up into more modular scripts, reduced number of variables and static elements focusing more on singleton design, rewrote method and variable names to hopefully make them more self descriptive.

- Internalised the client side logic used to repeatedly poll the network to discover clients

- Prevent co-routine of server running on client

- Minimised the public API for NetworkDiscovery to just 4 methods required to manage discovery, 1. start server, 2. start client, 3. stop all, 4. update server message

## Usage

Attach NetworkDiscovery script (and for testing NetworkDiscoveryHUD script!) to NetworkManager's game object.

For more details on how to use it, check out NetworkDiscoveryHUD script.

NetworkDiscovery script contains four public methods that drive everything:
        
	    // I call this from my NetworkManage::OnStartServer
        public bool ServerPassiveBroadcastGame(byte[] serverBroadcastPacket)
		
	    // I call this when the Lobby screen is loaded in my game, it causes clients to periodically broadcast a message for any listening servers to respond to
        public bool ClientRunActiveDiscovery()
		
	    // I call this when I leave the lobby menu and in my override of NetworkManager::OnStopServer (not done consistently via the simple HUD as wanted to avoid adding NetworkManager override to sample)
        public void StopDiscovery() {

	    // I call this when my network manager acquires new players or other game state changes occur that I want to display in the lobby screen
        public void UpdateServerBroadcastPacket(byte[] serverBroadcastPacket)

## Optional TODO's that were beyond my scope of interest

- Targetted broadcast feature removed for simplicity (available on master), I have a separate direct connect UI in my game so I didn't want to keep this

- Measure ping - requires that all socket operations are done in a separate thread, or using async methods

## Thanks

I wrote this for my own usage and wanted to share it as thanks to in0finite for his excellent work.
