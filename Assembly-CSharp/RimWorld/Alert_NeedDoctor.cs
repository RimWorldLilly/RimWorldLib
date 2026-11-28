using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC3 RID: 7107
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x0600B635 RID: 46645 RVA: 0x004023A7 File Offset: 0x004005A7
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170020F3 RID: 8435
		// (get) Token: 0x0600B636 RID: 46646 RVA: 0x004023E4 File Offset: 0x004005E4
		private List<Pawn> Patients
		{
			get
			{
				this.patientsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						bool flag = false;
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							foreach (Pawn pawn2 in maps[i].mapPawns.FreeColonistsSpawned)
							{
								if (pawn2.Spawned || pawn2.BrieflyDespawned())
								{
									if (pawn2.Downed && !LifeStageUtility.AlwaysDowned(pawn2))
									{
										Pawn_NeedsTracker needs = pawn2.needs;
										if (((needs != null) ? needs.food : null) != null && pawn2.needs.food.CurCategory > HungerCategory.Fed && pawn2.InBed())
										{
											goto IL_128;
										}
									}
									if (!HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2))
									{
										continue;
									}
									IL_128:
									this.patientsResult.Add(pawn2);
								}
							}
						}
					}
				}
				return this.patientsResult;
			}
		}

		// Token: 0x0600B637 RID: 46647 RVA: 0x00402574 File Offset: 0x00400774
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.patientsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "NeedDoctorDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B638 RID: 46648 RVA: 0x00402610 File Offset: 0x00400810
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Patients);
		}

		// Token: 0x040068AC RID: 26796
		private List<Pawn> patientsResult = new List<Pawn>();

		// Token: 0x040068AD RID: 26797
		private StringBuilder sb = new StringBuilder();
	}
}
