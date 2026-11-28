using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002298 RID: 8856
	public class QuestPart_SurveyScanner : QuestPartActivable
	{
		// Token: 0x0600DAAC RID: 55980 RVA: 0x00516891 File Offset: 0x00514A91
		public QuestPart_SurveyScanner()
		{
		}

		// Token: 0x0600DAAD RID: 55981 RVA: 0x00516899 File Offset: 0x00514A99
		public QuestPart_SurveyScanner(Site site, string inSignalEnable, string scannerTag, int duration)
		{
			this.site = site;
			this.inSignalEnable = inSignalEnable;
			this.scannerTag = scannerTag;
			this.duration = duration;
		}

		// Token: 0x0600DAAE RID: 55982 RVA: 0x005168C0 File Offset: 0x00514AC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<Thing>(ref this.scanner, "scanner", false);
			Scribe_Values.Look<string>(ref this.scannerTag, "scannerTag", null, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x0600DAAF RID: 55983 RVA: 0x0051691C File Offset: 0x00514B1C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalEnable)
			{
				this.scanner = this.site.Map.listerThings.ThingsOfDef(ThingDefOf.SurveyScanner).FirstOrDefault<Thing>();
				this.scanner.TryGetComp<CompCountdown>().endTick = GenTicks.TicksGame + this.duration;
				QuestUtility.AddQuestTag(ref this.scanner.questTags, this.scannerTag);
				this.scanner.SetFactionDirect(Faction.OfPlayer);
				Find.LetterStack.ReceiveLetter("LetterLabelSurveyScannerArrived".Translate(), "LetterSurveyScannerArrived".Translate(this.duration.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")), LetterDefOf.NeutralEvent, this.scanner, null, null, null, null, 0, true);
			}
		}

		// Token: 0x04008BE2 RID: 35810
		private Site site;

		// Token: 0x04008BE3 RID: 35811
		private Thing scanner;

		// Token: 0x04008BE4 RID: 35812
		private string scannerTag;

		// Token: 0x04008BE5 RID: 35813
		private int duration;
	}
}
