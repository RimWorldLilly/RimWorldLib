using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEB RID: 7147
	public class Alert_Boredom : Alert
	{
		// Token: 0x0600B6D8 RID: 46808 RVA: 0x00406D67 File Offset: 0x00404F67
		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		// Token: 0x1700210B RID: 8459
		// (get) Token: 0x0600B6D9 RID: 46809 RVA: 0x00406DA4 File Offset: 0x00404FA4
		private List<Pawn> BoredPawns
		{
			get
			{
				this.boredPawnsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						List<Pawn> freeColonistsSpawned = maps[i].mapPawns.FreeColonistsSpawned;
						if (freeColonistsSpawned.Any<Pawn>())
						{
							List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(maps[i]);
							try
							{
								foreach (Pawn pawn in freeColonistsSpawned)
								{
									if (pawn.needs.joy != null && (pawn.needs.joy.CurLevelPercentage < 0.24000001f || pawn.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && pawn.needs.joy.tolerances.BoredOfKinds(list))
									{
										this.boredPawnsResult.Add(pawn);
									}
								}
							}
							finally
							{
								list.Clear();
							}
						}
					}
				}
				return this.boredPawnsResult;
			}
		}

		// Token: 0x0600B6DA RID: 46810 RVA: 0x00406EC4 File Offset: 0x004050C4
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns);
		}

		// Token: 0x0600B6DB RID: 46811 RVA: 0x00406ED4 File Offset: 0x004050D4
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			Pawn pawn = null;
			foreach (Pawn pawn2 in this.boredPawnsResult)
			{
				this.sb.AppendLine("  - " + pawn2.NameShortColored.Resolve());
				if (pawn == null)
				{
					pawn = pawn2;
				}
			}
			string value = (pawn != null) ? JoyUtility.JoyKindsOnMapString(pawn.Map) : string.Empty;
			return "BoredomDesc".Translate(this.sb.ToString().TrimEndNewlines(), pawn.LabelShort, value, pawn.Named("PAWN"));
		}

		// Token: 0x040068F2 RID: 26866
		private const float JoyNeedThreshold = 0.24000001f;

		// Token: 0x040068F3 RID: 26867
		private List<Pawn> boredPawnsResult = new List<Pawn>();

		// Token: 0x040068F4 RID: 26868
		private StringBuilder sb = new StringBuilder();
	}
}
