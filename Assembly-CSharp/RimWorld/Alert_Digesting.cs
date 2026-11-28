using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BAC RID: 7084
	public class Alert_Digesting : Alert_Critical
	{
		// Token: 0x170020E5 RID: 8421
		// (get) Token: 0x0600B5D5 RID: 46549 RVA: 0x004009F2 File Offset: 0x003FEBF2
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020E6 RID: 8422
		// (get) Token: 0x0600B5D6 RID: 46550 RVA: 0x004009F8 File Offset: 0x003FEBF8
		private List<Pawn> Digesting
		{
			get
			{
				this.digesting.Clear();
				foreach (Map map in Find.Maps)
				{
					using (IEnumerator<Pawn> enumerator2 = map.mapPawns.AllPawnsSpawned.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CompDevourer compDevourer;
							if (enumerator2.Current.TryGetComp(out compDevourer) && compDevourer.Digesting && (compDevourer.DigestingPawn.IsColonist || compDevourer.DigestingPawn.IsColonyMech))
							{
								this.digesting.Add(compDevourer.DigestingPawn);
							}
						}
					}
				}
				return this.digesting;
			}
		}

		// Token: 0x0600B5D7 RID: 46551 RVA: 0x00400AC8 File Offset: 0x003FECC8
		public Alert_Digesting()
		{
			this.defaultLabel = "AlertDigestion".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x0600B5D8 RID: 46552 RVA: 0x00400AF8 File Offset: 0x003FECF8
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.digesting)
			{
				int ticksLeftThisToil = ((Pawn)pawn.SpawnedParentOrMe).jobs.curDriver.ticksLeftThisToil;
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve().CapitalizeFirst() + ": " + ticksLeftThisToil.ToStringSecondsFromTicks());
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AlertDigestionDesc".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(":\n");
			defaultInterpolatedStringHandler.AppendFormatted(stringBuilder.ToString().TrimEndNewlines());
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AlertDigestionDescAppended".Translate());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600B5D9 RID: 46553 RVA: 0x00400BF8 File Offset: 0x003FEDF8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Digesting);
		}

		// Token: 0x0400688E RID: 26766
		private readonly List<Pawn> digesting = new List<Pawn>();
	}
}
