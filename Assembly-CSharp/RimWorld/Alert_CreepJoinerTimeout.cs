using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDE RID: 7134
	public class Alert_CreepJoinerTimeout : Alert
	{
		// Token: 0x17002101 RID: 8449
		// (get) Token: 0x0600B6A0 RID: 46752 RVA: 0x0040553C File Offset: 0x0040373C
		private List<Pawn> Creepjoiners
		{
			get
			{
				this.creepjoiners.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.AllHumanlikeSpawned)
					{
						if (pawn.IsCreepJoiner && pawn.creepjoiner.IsOnEntryLord && GenTicks.TicksAbs >= pawn.creepjoiner.timeoutAt - 30000)
						{
							this.creepjoiners.Add(pawn);
						}
					}
				}
				return this.creepjoiners;
			}
		}

		// Token: 0x0600B6A1 RID: 46753 RVA: 0x00405610 File Offset: 0x00403810
		public Alert_CreepJoinerTimeout()
		{
			this.defaultPriority = AlertPriority.High;
			this.requireAnomaly = true;
		}

		// Token: 0x0600B6A2 RID: 46754 RVA: 0x0040563C File Offset: 0x0040383C
		public override string GetLabel()
		{
			if (this.creepjoiners.NullOrEmpty<Pawn>())
			{
				return string.Empty;
			}
			return "CreepJoinerTimeout".Translate();
		}

		// Token: 0x0600B6A3 RID: 46755 RVA: 0x00405660 File Offset: 0x00403860
		public override TaggedString GetExplanation()
		{
			if (this.creepjoiners.NullOrEmpty<Pawn>())
			{
				return string.Empty;
			}
			this.sb.Length = 0;
			foreach (Pawn pawn in this.creepjoiners)
			{
				int numTicks = Mathf.Max(pawn.creepjoiner.timeoutAt - GenTicks.TicksAbs, 0);
				StringBuilder stringBuilder = this.sb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 3);
				defaultInterpolatedStringHandler.AppendLiteral("  - ");
				defaultInterpolatedStringHandler.AppendFormatted(pawn.LabelShortCap.Colorize(ColoredText.NameColor));
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted(pawn.creepjoiner.form.label.Colorize(Color.gray));
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(numTicks.ToStringTicksToPeriodVerbose(true, true));
				stringBuilder.AppendLineTagged(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("CreepJoinerTimeoutDesc".Translate());
			defaultInterpolatedStringHandler2.AppendLiteral(":\n\n");
			defaultInterpolatedStringHandler2.AppendFormatted<StringBuilder>(this.sb);
			defaultInterpolatedStringHandler2.AppendLiteral("\n");
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("CreepJoinerTimeoutDescAppended".Translate());
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x0600B6A4 RID: 46756 RVA: 0x004057D8 File Offset: 0x004039D8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Creepjoiners);
		}

		// Token: 0x040068DD RID: 26845
		private const int WarningTicks = 30000;

		// Token: 0x040068DE RID: 26846
		private readonly List<Pawn> creepjoiners = new List<Pawn>();

		// Token: 0x040068DF RID: 26847
		private StringBuilder sb = new StringBuilder();
	}
}
