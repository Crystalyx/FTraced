using System;
using GlLib.Client;
using GlLib.Server;
using GlLib.Utils;

namespace GlLib.Common.Packets
{
    public class IntegratedConnectionRequestPacket : Packet
    {
        public IntegratedConnectionRequestPacket()
        {
        }
        public ClientService _client;

        public IntegratedConnectionRequestPacket(ClientService client)
        {
            _client = client;
        }

        public override void WriteToNbt(NbtTag tag)
        {
        }

        public override void ReadFromNbt(NbtTag tag)
        {
        }

        public override void OnServerReceive(SideService server)
        {
            ((ServerInstance)server).ConnectClient(_client);

            ConnectionEstablishedPacket connectionPacket =
                new ConnectionEstablishedPacket(server._serverId);

            Proxy.SendPacketToPlayer(_client._nickName, connectionPacket);
        }
    }
}