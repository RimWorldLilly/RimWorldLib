using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0D RID: 7181
	public class Alert_WarqueenHasLowResources : Alert
	{
		// Token: 0x0600B760 RID: 46944 RVA: 0x0040A4A2 File Offset: 0x004086A2
		public Alert_WarqueenHasLowResources()
		{
			this.requireBiotech = true;
		}

		// Token: 0x0600B761 RID: 46945 RVA: 0x0040A4BC File Offset: 0x004086BC
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				CompMechCarrier compMechCarrier;
				if (pawn.IsColonyMech && pawn.TryGetComp(out compMechCarrier) && compMechCarrier.LowIngredientCount)
				{
					this.targets.Add(pawn);
				}
			}
		}

		// Token: 0x0600B762 RID: 46946 RVA: 0x0040A544 File Offset: 0x00408744
		public override string GetLabel()
		{
			string result;
			if ((result = this.defaultLabel) == null)
			{
				result = (this.defaultLabel = "AlertWarqueenHasLowResources".Translate(PawnKindDefOf.Mech_Warqueen.LabelCap));
			}
			return result;
		}

		// Token: 0x0600B763 RID: 46947 RVA: 0x0040A584 File Offset: 0x00408784
		public override TaggedString GetExplanation()
		{
			string str;
			if ((str = this.defaultExplanation) == null)
			{
				str = (this.defaultExplanation = "AlertWarqueenHasLowResourcesDesc".Translate(PawnKindDefOf.Mech_Warqueen.labelPlural));
			}
			return str;
		}

		// Token: 0x0600B764 RID: 46948 RVA: 0x0040A5C7 File Offset: 0x004087C7
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006930 RID: 26928
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
