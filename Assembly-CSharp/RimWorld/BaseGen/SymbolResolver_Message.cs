using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200212F RID: 8495
	public class SymbolResolver_Message : SymbolResolver
	{
		// Token: 0x0600D4D3 RID: 54483 RVA: 0x004ECD68 File Offset: 0x004EAF68
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
			signalAction_Message.signalTag = rp.messageSignalTag;
			signalAction_Message.message = rp.message;
			signalAction_Message.messageType = rp.messageType;
			signalAction_Message.lookTargets = rp.lookTargets;
			GenSpawn.Spawn(signalAction_Message, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
