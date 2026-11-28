using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002113 RID: 8467
	public class SymbolResolver_TriggerContainerEmptied : SymbolResolver
	{
		// Token: 0x0600D481 RID: 54401 RVA: 0x004E91DD File Offset: 0x004E73DD
		public override bool CanResolve(ResolveParams rp)
		{
			return rp.triggerContainerEmptiedThing != null;
		}

		// Token: 0x0600D482 RID: 54402 RVA: 0x004E91E8 File Offset: 0x004E73E8
		public override void Resolve(ResolveParams rp)
		{
			TriggerContainerEmptied triggerContainerEmptied = (TriggerContainerEmptied)ThingMaker.MakeThing(ThingDefOf.TriggerContainerEmptied, null);
			triggerContainerEmptied.signalTag = rp.triggerContainerEmptiedSignalTag;
			triggerContainerEmptied.container = rp.triggerContainerEmptiedThing;
			GenSpawn.Spawn(triggerContainerEmptied, rp.triggerContainerEmptiedThing.Position, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
