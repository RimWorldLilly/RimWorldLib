using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002107 RID: 8455
	public class SymbolResolver_OpenDoorAction : SymbolResolver
	{
		// Token: 0x0600D456 RID: 54358 RVA: 0x004E7C5C File Offset: 0x004E5E5C
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_OpenDoor signalAction_OpenDoor = (SignalAction_OpenDoor)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenDoor, null);
			signalAction_OpenDoor.signalTag = rp.openDoorActionSignalTag;
			signalAction_OpenDoor.door = rp.openDoorActionDoor;
			GenSpawn.Spawn(signalAction_OpenDoor, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
