using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200184C RID: 6220
	public class CompCerebexCore_Destroyed : ThingComp
	{
		// Token: 0x06009BAC RID: 39852 RVA: 0x00381CAC File Offset: 0x0037FEAC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.hiveDestroyedTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06009BAD RID: 39853 RVA: 0x00381CC1 File Offset: 0x0037FEC1
		public override void CompTick()
		{
			if (this.hiveDestroyedTick < 0)
			{
				return;
			}
			if (Find.TickManager.TicksGame - this.hiveDestroyedTick == 60)
			{
				this.DoGoodwillForDestroyingCore();
				this.hiveDestroyedTick = -1;
			}
		}

		// Token: 0x06009BAE RID: 39854 RVA: 0x00381CF0 File Offset: 0x0037FEF0
		private void DoGoodwillForDestroyingCore()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
			{
				if (faction != Faction.OfPlayer && faction.def.humanlikeFaction && !faction.def.permanentEnemy)
				{
					faction.TryAffectGoodwillWith(Faction.OfPlayer, 50, true, true, HistoryEventDefOf.DestroyedMechhive, null);
				}
			}
		}

		// Token: 0x06009BAF RID: 39855 RVA: 0x00381D7C File Offset: 0x0037FF7C
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.hiveDestroyedTick, "hiveDestroyedTick", -1, false);
		}

		// Token: 0x04005C94 RID: 23700
		private int hiveDestroyedTick = -1;
	}
}
