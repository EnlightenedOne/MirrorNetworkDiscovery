﻿using UnityEngine;
using UnityEditor;
using System.Net;
using System.Collections.Generic;
using Assets.Scripts.NetworkMessages;

namespace Mirror {
    public class DiscoveryInfo {
        public DiscoveryInfo(IPEndPoint endPoint, byte[] packetData) {
            this.EndPoint = endPoint;
            this.packetData = packetData;
        }
        public IPEndPoint EndPoint { get; }
        public byte[] packetData;

        public GameBroadcastPacket unpackedData = null;
    }
}