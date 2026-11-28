using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE0 RID: 7136
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x17002103 RID: 8451
		// (get) Token: 0x0600B6AA RID: 46762 RVA: 0x00405894 File Offset: 0x00403A94
		private List<Pawn> IdleColonists
		{
			get
			{
				this.idleColonistsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (pawn.mindState.IsIdle && !pawn.IsQuestLodger())
							{
								if (pawn.royalty != null)
								{
									RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
									if (mostSeniorTitle == null || !mostSeniorTitle.def.suppressIdleAlert)
									{
										this.idleColonistsResult.Add(pawn);
									}
								}
								else
								{
									this.idleColonistsResult.Add(pawn);
								}
							}
						}
					}
				}
				return this.idleColonistsResult;
			}
		}

		// Token: 0x0600B6AB RID: 46763 RVA: 0x00405980 File Offset: 0x00403B80
		public override string GetLabel()
		{
			if (this.idleColonistsResult.Count == 1)
			{
				return "ColonistIdle".Translate();
			}
			return "ColonistsIdle".Translate(this.idleColonistsResult.Count.ToStringCached());
		}

		// Token: 0x0600B6AC RID: 46764 RVA: 0x004059D0 File Offset: 0x00403BD0
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.idleColonistsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsIdleDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B6AD RID: 46765 RVA: 0x00405A6C File Offset: 0x00403C6C
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		// Token: 0x040068E1 RID: 26849
		public const int MinDaysPassed = 1;

		// Token: 0x040068E2 RID: 26850
		private List<Pawn> idleColonistsResult = new List<Pawn>();

		// Token: 0x040068E3 RID: 26851
		private StringBuilder sb = new StringBuilder();
	}
}
