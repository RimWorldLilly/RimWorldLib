using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBD RID: 7101
	public class Alert_HypothermicAnimals : Alert
	{
		// Token: 0x170020EF RID: 8431
		// (get) Token: 0x0600B61D RID: 46621 RVA: 0x00401D14 File Offset: 0x003FFF14
		private List<Pawn> HypothermicAnimals
		{
			get
			{
				this.hypothermicAnimalsResult.Clear();
				IReadOnlyList<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
				for (int i = 0; i < allMaps_Spawned.Count; i++)
				{
					if (allMaps_Spawned[i].IsAnimal && allMaps_Spawned[i].Faction == null && allMaps_Spawned[i].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) != null)
					{
						this.hypothermicAnimalsResult.Add(allMaps_Spawned[i]);
					}
				}
				return this.hypothermicAnimalsResult;
			}
		}

		// Token: 0x0600B61E RID: 46622 RVA: 0x00401D95 File Offset: 0x003FFF95
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		// Token: 0x0600B61F RID: 46623 RVA: 0x00401D9C File Offset: 0x003FFF9C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Debug alert:\n\nThese wild animals are hypothermic. This may indicate a bug (but it may not, if the animals are trapped or in some other wierd but legitimate situation):");
			foreach (Pawn pawn in this.HypothermicAnimals)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "    ";
				Pawn pawn2 = pawn;
				stringBuilder2.AppendLine(str + ((pawn2 != null) ? pawn2.ToString() : null) + " at " + pawn.Position.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B620 RID: 46624 RVA: 0x00401E3C File Offset: 0x0040003C
		public override AlertReport GetReport()
		{
			if (!Prefs.DevMode)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.HypothermicAnimals);
		}

		// Token: 0x040068A7 RID: 26791
		private List<Pawn> hypothermicAnimalsResult = new List<Pawn>();
	}
}
