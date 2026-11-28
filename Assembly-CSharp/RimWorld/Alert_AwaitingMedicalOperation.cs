using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEE RID: 7150
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x1700210C RID: 8460
		// (get) Token: 0x0600B6E3 RID: 46819 RVA: 0x00407200 File Offset: 0x00405400
		private List<Pawn> AwaitingMedicalOperation
		{
			get
			{
				this.awaitingMedicalOperationResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<get_AwaitingMedicalOperation>g__IsAwaiting|2_0(list[i]))
					{
						this.awaitingMedicalOperationResult.Add(list[i]);
					}
				}
				List<Pawn> allMaps_PrisonersOfColonySpawned = PawnsFinder.AllMaps_PrisonersOfColonySpawned;
				for (int j = 0; j < allMaps_PrisonersOfColonySpawned.Count; j++)
				{
					if (this.<get_AwaitingMedicalOperation>g__IsAwaiting|2_0(allMaps_PrisonersOfColonySpawned[j]) && (!ModsConfig.BiotechActive || allMaps_PrisonersOfColonySpawned[j].health.surgeryBills.Count != 1 || allMaps_PrisonersOfColonySpawned[j].health.surgeryBills[0].recipe != RecipeDefOf.ExtractHemogenPack || allMaps_PrisonersOfColonySpawned[j].guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.HemogenFarm)))
					{
						this.awaitingMedicalOperationResult.Add(allMaps_PrisonersOfColonySpawned[j]);
					}
				}
				return this.awaitingMedicalOperationResult;
			}
		}

		// Token: 0x0600B6E4 RID: 46820 RVA: 0x004072F4 File Offset: 0x004054F4
		public override string GetLabel()
		{
			return "PatientsAwaitingMedicalOperation".Translate(this.awaitingMedicalOperationResult.Count.ToStringCached());
		}

		// Token: 0x0600B6E5 RID: 46821 RVA: 0x0040731C File Offset: 0x0040551C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.awaitingMedicalOperationResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B6E6 RID: 46822 RVA: 0x004073B8 File Offset: 0x004055B8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		// Token: 0x0600B6E8 RID: 46824 RVA: 0x004073E3 File Offset: 0x004055E3
		[CompilerGenerated]
		private bool <get_AwaitingMedicalOperation>g__IsAwaiting|2_0(Pawn p)
		{
			return HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed() && !this.awaitingMedicalOperationResult.Contains(p);
		}

		// Token: 0x040068F5 RID: 26869
		private List<Pawn> awaitingMedicalOperationResult = new List<Pawn>();

		// Token: 0x040068F6 RID: 26870
		private StringBuilder sb = new StringBuilder();
	}
}
