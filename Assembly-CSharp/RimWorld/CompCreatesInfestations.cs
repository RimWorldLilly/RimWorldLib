using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001853 RID: 6227
	public class CompCreatesInfestations : ThingComp
	{
		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x06009BE3 RID: 39907 RVA: 0x00382ADC File Offset: 0x00380CDC
		public bool CanCreateInfestationNow
		{
			get
			{
				CompDeepDrill comp = this.parent.GetComp<CompDeepDrill>();
				return (comp == null || comp.UsedLastTick()) && !this.CantFireBecauseCreatedInfestationRecently && !this.CantFireBecauseSomethingElseCreatedInfestationRecently;
			}
		}

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x06009BE4 RID: 39908 RVA: 0x00382B17 File Offset: 0x00380D17
		public bool CantFireBecauseCreatedInfestationRecently
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastCreatedInfestationTick + 420000;
			}
		}

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x06009BE5 RID: 39909 RVA: 0x00382B34 File Offset: 0x00380D34
		public bool CantFireBecauseSomethingElseCreatedInfestationRecently
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this.parent && list[i].Position.InHorDistOf(this.parent.Position, 10f) && list[i].TryGetComp<CompCreatesInfestations>().CantFireBecauseCreatedInfestationRecently)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06009BE6 RID: 39910 RVA: 0x00382BC4 File Offset: 0x00380DC4
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastCreatedInfestationTick, "lastCreatedInfestationTick", -999999, false);
		}

		// Token: 0x06009BE7 RID: 39911 RVA: 0x00382BDC File Offset: 0x00380DDC
		public void Notify_CreatedInfestation()
		{
			this.lastCreatedInfestationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04005CAD RID: 23725
		private int lastCreatedInfestationTick = -999999;

		// Token: 0x04005CAE RID: 23726
		private const float MinRefireDays = 7f;

		// Token: 0x04005CAF RID: 23727
		private const float PreventInfestationsDist = 10f;
	}
}
