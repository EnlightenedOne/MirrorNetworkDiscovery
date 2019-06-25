using Assets.Scripts.NetworkMessages;
using Assets.Scripts.Utility.Serialisation;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror {

    public class NetworkDiscoveryHUD : MonoBehaviour {

        Dictionary<string, DiscoveryInfo> m_discoveredServers = new Dictionary<string, DiscoveryInfo>();
        string[] m_headerNames = new string[] { "IP", "Host" };
        Vector2 m_scrollViewPos = Vector2.zero;

        GUIStyle m_centeredLabelStyle;

        public int offsetX = 5;
        public int offsetY = 150;
        public int width = 500, height = 400;

        void OnEnable() {
            NetworkDiscovery.onReceivedServerResponse += OnDiscoveredServer;
        }

        void OnDisable() {
            NetworkDiscovery.onReceivedServerResponse -= OnDiscoveredServer;
        }

        void OnGUI() {
            if (NetworkManager.singleton == null) {
                return;
            }
            if (NetworkServer.active || NetworkClient.active) {
                return;
            }
            if (!NetworkDiscovery.SupportedOnThisPlatform) {
                return;
            }
            if (m_centeredLabelStyle == null) {
                m_centeredLabelStyle = new GUIStyle(GUI.skin.label);
                m_centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
            }

            int elemWidth = width / m_headerNames.Length - 5;

            GUILayout.BeginArea(new Rect(offsetX, offsetY, width, height));

            // In my own game I ripped this out, this is just as an example (wanted to avoid adding a NetworkManager to the sample)
            if (!NetworkClient.isConnected && !NetworkServer.active) {
                if (!NetworkClient.active) {
                    // LAN Host
                    if (GUILayout.Button("Passive Host", GUILayout.Height(25), GUILayout.ExpandWidth(false))) {
                        m_discoveredServers.Clear();
                        NetworkManager.singleton.StartHost();

                        // Wire in broadcaster pipeline here
                        GameBroadcastPacket gameBroadcastPacket = new GameBroadcastPacket();

                        gameBroadcastPacket.serverAddress = NetworkManager.singleton.networkAddress;
                        gameBroadcastPacket.port = ((TelepathyTransport)Transport.activeTransport).port;
                        gameBroadcastPacket.hostName = "MyDistinctDummyPlayerName";
                        gameBroadcastPacket.serverGUID = NetworkDiscovery.instance.serverId;

                        byte[] broadcastData = ByteStreamer.StreamToBytes(gameBroadcastPacket);
                        NetworkDiscovery.instance.ServerPassiveBroadcastGame(broadcastData);
                    }
                }
            }

            if (GUILayout.Button("Active Discovery", GUILayout.Height(25), GUILayout.ExpandWidth(false))) {
                m_discoveredServers.Clear();
                NetworkDiscovery.instance.ClientRunActiveDiscovery();
            }

            GUILayout.Label(string.Format("Servers [{0}]:", m_discoveredServers.Count));

            // header
            GUILayout.BeginHorizontal();
            foreach (string str in m_headerNames) {
                GUILayout.Button(str, GUILayout.Width(elemWidth));
            }
            GUILayout.EndHorizontal();

            // servers

            m_scrollViewPos = GUILayout.BeginScrollView(m_scrollViewPos);

            foreach (var info in m_discoveredServers.Values) {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button(info.EndPoint.Address.ToString(), GUILayout.Width(elemWidth))) {
                    Connect(info);
                }

                for (int i = 0; i < m_headerNames.Length; i++) {
                    if (i == 0) {
                        GUILayout.Label(info.unpackedData.serverAddress, m_centeredLabelStyle, GUILayout.Width(elemWidth));
                    } else {
                        GUILayout.Label(info.unpackedData.hostName, m_centeredLabelStyle, GUILayout.Width(elemWidth));
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        void Connect(DiscoveryInfo info) {
            if (NetworkManager.singleton == null ||
                Transport.activeTransport == null) {
                return;
            }
            if (!(Transport.activeTransport is TelepathyTransport)) {
                Debug.LogErrorFormat("Only {0} is supported", typeof(TelepathyTransport));
                return;
            }

            // assign address and port
            NetworkManager.singleton.networkAddress = info.EndPoint.Address.ToString();
            ((TelepathyTransport)Transport.activeTransport).port = (ushort)info.unpackedData.port;

            NetworkManager.singleton.StartClient();
        }

        void OnDiscoveredServer(DiscoveryInfo info) {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            m_discoveredServers[info.unpackedData.serverGUID] = info;
        }

    }

}
