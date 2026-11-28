using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BB RID: 6587
	public class CompRelicContainer : CompThingContainer
	{
		// Token: 0x0600A562 RID: 42338 RVA: 0x003AFB95 File Offset: 0x003ADD95
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Reliqary"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600A563 RID: 42339 RVA: 0x003AFBB7 File Offset: 0x003ADDB7
		public static bool IsRelic(Thing thing)
		{
			return thing.IsRelic();
		}

		// Token: 0x0600A564 RID: 42340 RVA: 0x003AFBBF File Offset: 0x003ADDBF
		public override bool Accepts(Thing thing)
		{
			return CompRelicContainer.IsRelic(thing);
		}

		// Token: 0x0600A565 RID: 42341 RVA: 0x003AFBC7 File Offset: 0x003ADDC7
		public override bool Accepts(ThingDef thingDef)
		{
			return false;
		}

		// Token: 0x0600A566 RID: 42342 RVA: 0x003AFBCC File Offset: 0x003ADDCC
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!base.Empty)
			{
				text = text + ("\n" + "StatsReport_RelicAtRitualMoodBonus".Translate() + ": ") + ThoughtDefOf.RelicAtRitual.stages[0].baseMoodEffect.ToString();
			}
			return text;
		}
	}
}
