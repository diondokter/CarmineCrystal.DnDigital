using CarmineCrystal.DnDigital.Core.Data;
using CarmineCrystal.Networking;
using ProtoBuf;
using System.Collections.Generic;

[ProtoContract]
public class GetMapRequest : Request
{

}

[ProtoContract]
public class GetMapResponse : Response
{
	[ProtoMember(1)]
	public Dictionary<uint, BoardObject> Map;
}