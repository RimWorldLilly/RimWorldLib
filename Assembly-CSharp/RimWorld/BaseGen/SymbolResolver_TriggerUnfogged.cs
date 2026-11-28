using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002115 RID: 8469
	public class SymbolResolver_TriggerUnfogged : SymbolResolver
	{
		// Token: 0x0600D486 RID: 54406 RVA: 0x004E92B0 File Offset: 0x004E74B0
		public override void Resolve(ResolveParams rp)
		{
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = rp.unfoggedSignalTag;
			GenSpawn.Spawn(triggerUnfogged, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
