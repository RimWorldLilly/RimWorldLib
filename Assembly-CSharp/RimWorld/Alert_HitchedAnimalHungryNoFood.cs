using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C02 RID: 7170
	public class Alert_HitchedAnimalHungryNoFood : Alert
	{
		// Token: 0x0600B72D RID: 46893 RVA: 0x00408DE4 File Offset: 0x00406FE4
		public Alert_HitchedAnimalHungryNoFood()
		{
			this.defaultLabel = "AlertHitchedAnimalHungryNoFood".Translate();
		}

		// Token: 0x0600B72E RID: 46894 RVA: 0x00408E18 File Offset: 0x00407018
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				Pawn_RopeTracker roping = pawn.roping;
				if (roping != null && roping.IsRopedToHitchingPost && pawn.needs.food.TicksStarving > 2500)
				{
					this.targets.Add(pawn);
				}
			}
		}

		// Token: 0x0600B72F RID: 46895 RVA: 0x00408EB0 File Offset: 0x004070B0
		public override TaggedString GetExplanation()
		{
			this.animalNames.Clear();
			foreach (GlobalTargetInfo globalTargetInfo in this.targets)
			{
				this.animalNames.Add(((Pawn)globalTargetInfo.Thing).NameShortColored.Resolve());
			}
			return "AlertHitchedAnimalHungryNoFoodDesc".Translate(this.animalNames.ToLineList("  - "));
		}

		// Token: 0x0600B730 RID: 46896 RVA: 0x00408F4C File Offset: 0x0040714C
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006915 RID: 26901
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006916 RID: 26902
		private List<string> animalNames = new List<string>();
	}
}
