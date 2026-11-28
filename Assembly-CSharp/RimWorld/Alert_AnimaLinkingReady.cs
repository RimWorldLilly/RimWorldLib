using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C06 RID: 7174
	public class Alert_AnimaLinkingReady : Alert
	{
		// Token: 0x0600B742 RID: 46914 RVA: 0x0040963C File Offset: 0x0040783C
		public Alert_AnimaLinkingReady()
		{
			this.defaultLabel = "AnimaLinkingReadyLabel".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B743 RID: 46915 RVA: 0x0040968C File Offset: 0x0040788C
		private void GetTargets()
		{
			this.culprits.Clear();
			this.targetLabels.Clear();
			this.tempPawns.Clear();
			foreach (LordJob_Ritual lordJob_Ritual in Find.IdeoManager.GetActiveRituals(Find.CurrentMap))
			{
				if (lordJob_Ritual.Ritual != null && lordJob_Ritual.Ritual.def == PreceptDefOf.AnimaTreeLinking)
				{
					return;
				}
			}
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.GetPsylinkLevel() < pawn.GetMaxPsylinkLevel() && MeditationFocusDefOf.Natural.CanPawnUse(pawn) && pawn.psychicEntropy.IsPsychicallySensitive && (!ModsConfig.BiotechActive || !pawn.DevelopmentalStage.Juvenile()))
				{
					this.tempPawns.Add(pawn);
				}
			}
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeAnima))
			{
				CompPsylinkable compPsylinkable = thing.TryGetComp<CompPsylinkable>();
				int count = compPsylinkable.CompSubplant.SubplantsForReading.Count;
				bool flag = false;
				foreach (Pawn pawn2 in this.tempPawns)
				{
					if (compPsylinkable.GetRequiredPlantCount(pawn2) <= count)
					{
						if (!this.culprits.Contains(pawn2))
						{
							this.culprits.Add(pawn2);
							this.targetLabels.Add(pawn2.NameFullColored);
						}
						flag = true;
					}
				}
				if (flag)
				{
					this.culprits.Add(thing);
				}
			}
		}

		// Token: 0x0600B744 RID: 46916 RVA: 0x004098B0 File Offset: 0x00407AB0
		public override TaggedString GetExplanation()
		{
			return "AnimaLinkingReadyDesc".Translate() + ":\n\n" + this.targetLabels.ToLineList("  - ") + "\n\n" + "AnimaLinkingReadyDescExtra".Translate();
		}

		// Token: 0x0600B745 RID: 46917 RVA: 0x004098FF File Offset: 0x00407AFF
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return AlertReport.Inactive;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.culprits);
		}

		// Token: 0x0400691F RID: 26911
		private List<Thing> culprits = new List<Thing>();

		// Token: 0x04006920 RID: 26912
		private List<string> targetLabels = new List<string>();

		// Token: 0x04006921 RID: 26913
		private List<Pawn> tempPawns = new List<Pawn>();
	}
}
