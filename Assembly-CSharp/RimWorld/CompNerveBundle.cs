using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188E RID: 6286
	public class CompNerveBundle : CompFleshmassHeartChild
	{
		// Token: 0x06009D5F RID: 40287 RVA: 0x003896CC File Offset: 0x003878CC
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.FleshmassNeuralLump, null);
			thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = this.heart.Biosignature;
			GenSpawn.Spawn(thing, this.parent.Position, prevMap, WipeMode.Vanish);
			Messages.Message("NeuralLumpDroppedMessage".Translate(), thing, MessageTypeDefOf.NeutralEvent, true);
			base.Notify_Killed(prevMap, dinfo);
		}
	}
}
