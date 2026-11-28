using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C01 RID: 7169
	public class Alert_PennedAnimalHungry : Alert
	{
		// Token: 0x0600B729 RID: 46889 RVA: 0x00408CB4 File Offset: 0x00406EB4
		public Alert_PennedAnimalHungry()
		{
			this.defaultLabel = "AlertPennedAnimalHungry".Translate();
		}

		// Token: 0x0600B72A RID: 46890 RVA: 0x00408CE8 File Offset: 0x00406EE8
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnNames.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn) && pawn.needs.food != null && pawn.needs.food.TicksStarving > 2500 && AnimalPenUtility.GetCurrentPenOf(pawn, false) != null)
				{
					this.targets.Add(pawn);
					this.pawnNames.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x0600B72B RID: 46891 RVA: 0x00408DB0 File Offset: 0x00406FB0
		public override TaggedString GetExplanation()
		{
			return "AlertPennedAnimalHungryDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x0600B72C RID: 46892 RVA: 0x00408DD1 File Offset: 0x00406FD1
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006912 RID: 26898
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006913 RID: 26899
		private List<string> pawnNames = new List<string>();

		// Token: 0x04006914 RID: 26900
		public const int TicksStarvingThresh = 2500;
	}
}
