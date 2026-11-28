using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFD RID: 7165
	public class Alert_AnimalPenNeeded : Alert
	{
		// Token: 0x0600B719 RID: 46873 RVA: 0x0040857C File Offset: 0x0040677C
		public Alert_AnimalPenNeeded()
		{
			this.defaultLabel = "AlertAnimalPenNeeded".Translate();
		}

		// Token: 0x0600B71A RID: 46874 RVA: 0x004085B0 File Offset: 0x004067B0
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.anyHandlers = false;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					Pawn pawn = null;
					int num = -1;
					foreach (Pawn pawn2 in map.mapPawns.FreeColonistsSpawned)
					{
						if (pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Handling))
						{
							int level = pawn2.skills.GetSkill(SkillDefOf.Animals).Level;
							if (level > num)
							{
								pawn = pawn2;
								num = level;
							}
						}
					}
					foreach (Pawn pawn3 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
					{
						if (AnimalPenUtility.NeedsToBeManagedByRope(pawn3) && !pawn3.InAggroMentalState && AnimalPenUtility.GetCurrentPenOf(pawn3, true) == null && pawn3.CanReachMapEdge())
						{
							bool flag = !AnimalPenUtility.AnySuitablePens(pawn3, true) && !AnimalPenUtility.AnySuitableHitch(pawn3);
							if (pawn == null)
							{
								flag = true;
							}
							else
							{
								this.anyHandlers = true;
							}
							if (flag)
							{
								this.targets.Add(pawn3);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B71B RID: 46875 RVA: 0x00408768 File Offset: 0x00406968
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			this.sb.AppendLine("AlertAnimalPenNeededDesc".Translate());
			if (!this.anyHandlers)
			{
				this.sb.AppendLine();
				this.sb.AppendLine("AlertAnimalPenNeededNoHandlers".Translate());
			}
			this.sb.AppendLine();
			this.sb.Append("AlertAnimalPenNeededDescExplanation".Translate());
			return this.sb.ToString();
		}

		// Token: 0x0600B71C RID: 46876 RVA: 0x00408802 File Offset: 0x00406A02
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400690B RID: 26891
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400690C RID: 26892
		private bool anyHandlers;

		// Token: 0x0400690D RID: 26893
		private StringBuilder sb = new StringBuilder();
	}
}
