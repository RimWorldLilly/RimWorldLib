using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002114 RID: 8468
	public class SymbolResolver_TriggerRect : SymbolResolver
	{
		// Token: 0x0600D484 RID: 54404 RVA: 0x004E9244 File Offset: 0x004E7444
		public override void Resolve(ResolveParams rp)
		{
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = rp.rectTriggerSignalTag;
			rectTrigger.Rect = rp.rect;
			rectTrigger.destroyIfUnfogged = rp.destroyIfUnfogged.GetValueOrDefault();
			GenSpawn.Spawn(rectTrigger, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}
	}
}
