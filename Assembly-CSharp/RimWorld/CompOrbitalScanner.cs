using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001925 RID: 6437
	public class CompOrbitalScanner : ThingComp
	{
		// Token: 0x17001BE3 RID: 7139
		// (get) Token: 0x0600A181 RID: 41345 RVA: 0x0039BB3C File Offset: 0x00399D3C
		private CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.compPowerTrader) == null)
				{
					result = (this.compPowerTrader = this.parent.TryGetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x0600A182 RID: 41346 RVA: 0x0039BB68 File Offset: 0x00399D68
		private OrbitalScannerWorldComponent OrbitalScannerWorldComponent
		{
			get
			{
				OrbitalScannerWorldComponent result;
				if ((result = this.orbitalScannerWorldComponent) == null)
				{
					result = (this.orbitalScannerWorldComponent = Find.World.GetComponent<OrbitalScannerWorldComponent>());
				}
				return result;
			}
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x0600A183 RID: 41347 RVA: 0x0039BB94 File Offset: 0x00399D94
		private List<QuestScriptDef> ScannerQuests
		{
			get
			{
				List<QuestScriptDef> result;
				if ((result = this.scannerQuests) == null)
				{
					result = (this.scannerQuests = QuestUtility.GetGiverQuests(QuestGiverTag.OrbitalScanner).ToList<QuestScriptDef>());
				}
				return result;
			}
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x0600A184 RID: 41348 RVA: 0x0039BBBF File Offset: 0x00399DBF
		private bool IsLocating
		{
			get
			{
				return this.locateSignalTick > 0;
			}
		}

		// Token: 0x17001BE7 RID: 7143
		// (get) Token: 0x0600A185 RID: 41349 RVA: 0x0039BBCA File Offset: 0x00399DCA
		private int TicksUntilLocated
		{
			get
			{
				return this.locateSignalTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600A186 RID: 41350 RVA: 0x0039BBDD File Offset: 0x00399DDD
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.locateSignalTick, "locateSignalTick", 0, false);
		}

		// Token: 0x0600A187 RID: 41351 RVA: 0x0039BBF4 File Offset: 0x00399DF4
		public override void CompTick()
		{
			if (!this.PowerTrader.PowerOn)
			{
				if (this.locateSignalTick > 0)
				{
					this.locateSignalTick++;
				}
				return;
			}
			if (!this.IsLocating)
			{
				this.OrbitalScannerWorldComponent.Notify_ScannerWorking(this);
				return;
			}
			if (Find.TickManager.TicksGame >= this.locateSignalTick)
			{
				this.LocateSignal();
			}
		}

		// Token: 0x0600A188 RID: 41352 RVA: 0x0039BC54 File Offset: 0x00399E54
		public void ReceiveSignal()
		{
			this.locateSignalTick = Find.TickManager.TicksGame + CompOrbitalScanner.TrackSignalDurationRangeTicks.RandomInRange;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("MessageOrbitalSignalDetected".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("OrbitalScannerCompletesIn".Translate(this.TicksUntilLocated.ToStringTicksToPeriod(true, false, true, true, false)));
			defaultInterpolatedStringHandler.AppendLiteral(").");
			Messages.Message(defaultInterpolatedStringHandler.ToStringAndClear(), this.parent, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x0600A189 RID: 41353 RVA: 0x0039BCF4 File Offset: 0x00399EF4
		private void LocateSignal()
		{
			float points = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map);
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromOrbitalScanner".Translate(), false);
			QuestScriptDef questScriptDef = this.ScannerQuests.RandomElementByWeight((QuestScriptDef q) => NaturalRandomQuestChooser.GetNaturalRandomSelectionWeight(q, points, Find.World.StoryState));
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questScriptDef, slate);
			if (!quest.hidden && questScriptDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod", null, false));
			}
			this.locateSignalTick = -1;
		}

		// Token: 0x0600A18A RID: 41354 RVA: 0x0039BD98 File Offset: 0x00399F98
		public override string CompInspectStringExtra()
		{
			if (!this.PowerTrader.PowerOn)
			{
				return null;
			}
			if (this.IsLocating)
			{
				return "OrbitalScannerLocating".Translate() + ": " + this.TicksUntilLocated.ToStringTicksToPeriod(true, false, true, true, false);
			}
			if (this.PowerTrader.PowerOn)
			{
				return "OrbitalScannerScanning".Translate();
			}
			return null;
		}

		// Token: 0x0600A18B RID: 41355 RVA: 0x0039BE09 File Offset: 0x0039A009
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompOrbitalScanner.<CompGetGizmosExtra>d__20 <CompGetGizmosExtra>d__ = new CompOrbitalScanner.<CompGetGizmosExtra>d__20(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005F6B RID: 24427
		private int locateSignalTick = -1;

		// Token: 0x04005F6C RID: 24428
		private CompPowerTrader compPowerTrader;

		// Token: 0x04005F6D RID: 24429
		private OrbitalScannerWorldComponent orbitalScannerWorldComponent;

		// Token: 0x04005F6E RID: 24430
		private List<QuestScriptDef> scannerQuests;

		// Token: 0x04005F6F RID: 24431
		private static readonly IntRange TrackSignalDurationRangeTicks = new IntRange(480000, 600000);
	}
}
