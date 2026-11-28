using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B2B RID: 6955
	[StaticConstructorOnStartup]
	public class PitGate : MapPortal
	{
		// Token: 0x17002023 RID: 8227
		// (get) Token: 0x0600B243 RID: 45635 RVA: 0x003EC2AD File Offset: 0x003EA4AD
		private IEnumerable<PitGateIncidentDef> Incidents
		{
			get
			{
				return DefDatabase<PitGateIncidentDef>.AllDefsListForReading;
			}
		}

		// Token: 0x17002024 RID: 8228
		// (get) Token: 0x0600B244 RID: 45636 RVA: 0x003EC2B4 File Offset: 0x003EA4B4
		private bool HasActiveIncident
		{
			get
			{
				return this.currentIncident != null;
			}
		}

		// Token: 0x17002025 RID: 8229
		// (get) Token: 0x0600B245 RID: 45637 RVA: 0x003EC2BF File Offset: 0x003EA4BF
		private bool IsCoolingDown
		{
			get
			{
				return (float)Find.TickManager.TicksGame < (float)this.lastIncidentTick + 720000f || Find.TickManager.TicksGame < this.tickOpened + 450;
			}
		}

		// Token: 0x17002026 RID: 8230
		// (get) Token: 0x0600B246 RID: 45638 RVA: 0x003EC2F5 File Offset: 0x003EA4F5
		public bool IsCollapsing
		{
			get
			{
				return this.isCollapsing;
			}
		}

		// Token: 0x17002027 RID: 8231
		// (get) Token: 0x0600B247 RID: 45639 RVA: 0x003EC2FD File Offset: 0x003EA4FD
		public int CollapseStage
		{
			get
			{
				if (this.collapseTick - Find.TickManager.TicksGame >= 3600)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x17002028 RID: 8232
		// (get) Token: 0x0600B248 RID: 45640 RVA: 0x003EC31A File Offset: 0x003EA51A
		public int TicksUntilCollapse
		{
			get
			{
				return this.collapseTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002029 RID: 8233
		// (get) Token: 0x0600B249 RID: 45641 RVA: 0x003EC32D File Offset: 0x003EA52D
		public override bool AutoDraftOnEnter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B24A RID: 45642 RVA: 0x003EC330 File Offset: 0x003EA530
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.pointsMultiplier, "pointsMultiplier", 0f, false);
			Scribe_Values.Look<int>(ref this.tickOpened, "tickOpened", 0, false);
			Scribe_Values.Look<int>(ref this.lastIncidentTick, "lastIncidentTick", 0, false);
			Scribe_Values.Look<int>(ref this.nextIncidentTick, "nextIncidentTick", 0, false);
			Scribe_Values.Look<int>(ref this.collapseTick, "collapseTick", 0, false);
			Scribe_Values.Look<bool>(ref this.isCollapsing, "isCollapsing", false, false);
			Scribe_Deep.Look<PitGateIncidentWorker>(ref this.currentIncident, "currentWorker", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.workerEnd, "workerEnd", 0, false);
			Scribe_Values.Look<int>(ref this.numIncidentsFired, "numIncidentsFired", 0, false);
		}

		// Token: 0x0600B24B RID: 45643 RVA: 0x003EC3EC File Offset: 0x003EA5EC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.tickOpened = Find.TickManager.TicksGame;
				EffecterDefOf.ImpactDustCloud.Spawn(base.Position, base.Map, 1f).Cleanup();
				Find.CameraDriver.shaker.DoShake(0.1f, 120);
				SoundDefOf.PitGateOpen.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
			}
		}

		// Token: 0x0600B24C RID: 45644 RVA: 0x003EC460 File Offset: 0x003EA660
		protected override void Tick()
		{
			base.Tick();
			if (this.IsCollapsing)
			{
				if (this.CollapseStage == 1)
				{
					if (this.collapseEffecter1 == null)
					{
						this.collapseEffecter1 = EffecterDefOf.PitGateAboveGroundCollapseStage1.Spawn(this, base.Map, 1f);
					}
				}
				else if (this.CollapseStage == 2)
				{
					if (this.collapseSustainer == null)
					{
						this.collapseSustainer = SoundDefOf.PitGateCollapsing.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
					}
					this.collapseSustainer.Maintain();
					if (this.collapseEffecter2 == null)
					{
						this.collapseEffecter2 = EffecterDefOf.PitGateAboveGroundCollapseStage2.Spawn(this, base.Map, 1f);
					}
					if (Find.CurrentMap == base.Map && Rand.MTBEventOccurs(2f, 60f, 1f))
					{
						Find.CameraDriver.shaker.DoShake(0.2f);
					}
				}
				Effecter effecter = this.collapseEffecter1;
				if (effecter != null)
				{
					effecter.EffectTick(this, this);
				}
				Effecter effecter2 = this.collapseEffecter2;
				if (effecter2 != null)
				{
					effecter2.EffectTick(this, this);
				}
				if (Find.TickManager.TicksGame >= this.collapseTick)
				{
					this.Collapse();
				}
				return;
			}
			if (this.HasActiveIncident)
			{
				if (Find.TickManager.TicksGame > this.workerEnd)
				{
					this.currentIncident = null;
					return;
				}
				this.currentIncident.Tick();
			}
			if (!this.IsCoolingDown && this.numIncidentsFired < 1)
			{
				this.FireRandomIncidentByWeight();
			}
			else if (!this.IsCoolingDown && Rand.MTBEventOccurs(2f, 60000f, 1f))
			{
				this.nextIncidentTick = Find.TickManager.TicksGame + PitGate.IncidentDelayTicksRange.RandomInRange;
				Find.LetterStack.ReceiveLetter("EmergenceWarningLabel".Translate(), "EmergenceWarningText".Translate(), LetterDefOf.ThreatBig, this, null, null, null, null, 0, true);
			}
			if (this.nextIncidentTick > 0 && Find.TickManager.TicksGame > this.nextIncidentTick)
			{
				this.FireRandomIncidentByWeight();
				this.nextIncidentTick = -999999;
			}
		}

		// Token: 0x0600B24D RID: 45645 RVA: 0x003EC67C File Offset: 0x003EA87C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Vanish)
			{
				base.Destroy(mode);
				return;
			}
			Map map = base.Map;
			base.Destroy(mode);
			EffecterDefOf.ImpactDustCloud.Spawn(base.Position, map, 1f).Cleanup();
			Messages.Message("MessagePitGateCollapsed".Translate(), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600B24E RID: 45646 RVA: 0x003EC6EC File Offset: 0x003EA8EC
		private void FireRandomIncidentByWeight()
		{
			List<PitGateIncidentDef> list = this.Incidents.ToList<PitGateIncidentDef>();
			while (list.Any<PitGateIncidentDef>())
			{
				PitGateIncidentDef pitGateIncidentDef = list.RandomElementByWeightWithFallback((PitGateIncidentDef i) => i.baseChance, null);
				list.Remove(pitGateIncidentDef);
				if (this.TryFireIncident(pitGateIncidentDef, false, -1f))
				{
					break;
				}
			}
		}

		// Token: 0x0600B24F RID: 45647 RVA: 0x003EC750 File Offset: 0x003EA950
		private bool TryFireIncident(PitGateIncidentDef def, bool force = false, float points = -1f)
		{
			PitGateIncidentWorker pitGateIncidentWorker = (PitGateIncidentWorker)Activator.CreateInstance(def.workerClass);
			pitGateIncidentWorker.pitGate = this;
			pitGateIncidentWorker.def = def;
			if (points < 0f)
			{
				points = StorytellerUtility.DefaultThreatPointsNow(base.Map);
			}
			points *= this.pointsMultiplier;
			if (force)
			{
				this.currentIncident = null;
				this.workerEnd = -999999;
			}
			if (pitGateIncidentWorker.CanFireNow())
			{
				this.currentIncident = pitGateIncidentWorker;
				this.currentIncident.Setup(points);
				this.workerEnd = Find.TickManager.TicksGame + def.durationRangeTicks.RandomInRange;
				Find.LetterStack.ReceiveLetter(def.letterLabel, def.letterText, LetterDefOf.ThreatBig, new TargetInfo(this), null, null, null, null, 0, true);
				this.numIncidentsFired++;
				this.lastIncidentTick = Find.TickManager.TicksGame;
				return true;
			}
			return false;
		}

		// Token: 0x0600B250 RID: 45648 RVA: 0x003EC844 File Offset: 0x003EAA44
		public void BeginCollapsing()
		{
			int randomInRange = PitGate.CollapseDurationTicks.RandomInRange;
			this.collapseTick = Find.TickManager.TicksGame + randomInRange;
			Map pocketMap = this.pocketMap;
			if (pocketMap != null)
			{
				pocketMap.GetComponent<UndercaveMapComponent>().Notify_BeginCollapsing(randomInRange);
			}
			this.isCollapsing = true;
		}

		// Token: 0x0600B251 RID: 45649 RVA: 0x003EC890 File Offset: 0x003EAA90
		private void Collapse()
		{
			this.collapseSustainer.End();
			Effecter effecter = this.collapseEffecter2;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.collapseEffecter2 = null;
			Effecter effecter2 = this.collapseEffecter1;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.collapseEffecter1 = null;
			EffecterDefOf.PitGateAboveGroundCollapsed.Spawn(base.Position, base.Map, 1f);
			if (Find.CurrentMap == this.pocketMap)
			{
				SoundDefOf.UndercaveCollapsing_End.PlayOneShotOnCamera(null);
			}
			else
			{
				SoundDefOf.PitGateCollapsing_End.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (base.PocketMapExists)
			{
				DamageInfo damageInfo = new DamageInfo(DamageDefOf.Crush, 99999f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				for (int i = this.pocketMap.mapPawns.AllPawns.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.pocketMap.mapPawns.AllPawns[i];
					pawn.TakeDamage(damageInfo);
					if (!pawn.Dead)
					{
						pawn.Kill(new DamageInfo?(damageInfo), null);
					}
				}
				PocketMapUtility.DestroyPocketMap(this.pocketMap);
			}
			Thing.allowDestroyNonDestroyable = true;
			this.Destroy(DestroyMode.Deconstruct);
			Thing.allowDestroyNonDestroyable = false;
		}

		// Token: 0x0600B252 RID: 45650 RVA: 0x003EC9D4 File Offset: 0x003EABD4
		public override bool IsEnterable(out string reason)
		{
			reason = "";
			if (Find.TickManager.TicksGame < this.tickOpened + 450)
			{
				reason = "PitGateSettling".Translate();
				return false;
			}
			if (this.HasActiveIncident && Find.TickManager.TicksGame < this.currentIncident.fireTick + this.currentIncident.def.disableEnteringTicks)
			{
				reason = this.currentIncident.def.disableEnteringReason.CapitalizeFirst();
				return false;
			}
			if (this.isCollapsing && !this.beenEntered)
			{
				reason = "PitGateCollapsing".Translate();
				return false;
			}
			return true;
		}

		// Token: 0x0600B253 RID: 45651 RVA: 0x003ECA80 File Offset: 0x003EAC80
		public override IEnumerable<Gizmo> GetGizmos()
		{
			PitGate.<GetGizmos>d__45 <GetGizmos>d__ = new PitGate.<GetGizmos>d__45(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x040066D7 RID: 26327
		private const float IncidentMTBDays = 2f;

		// Token: 0x040066D8 RID: 26328
		private const float IncidentMinimumIntervalDays = 12f;

		// Token: 0x040066D9 RID: 26329
		private const int InitialIncidentDelayTicks = 450;

		// Token: 0x040066DA RID: 26330
		private static readonly IntRange IncidentDelayTicksRange = new IntRange(1800, 3600);

		// Token: 0x040066DB RID: 26331
		private static readonly IntRange CollapseDurationTicks = new IntRange(25000, 27500);

		// Token: 0x040066DC RID: 26332
		private const int CollapseStageTwoDurationTicks = 3600;

		// Token: 0x040066DD RID: 26333
		private const float StageTwoShakeAmount = 0.2f;

		// Token: 0x040066DE RID: 26334
		private const float StageTwoShakeMTBSeconds = 2f;

		// Token: 0x040066DF RID: 26335
		private const float OpenedScreenShakeMagnitude = 0.1f;

		// Token: 0x040066E0 RID: 26336
		private const int OpenedScreenShakeDuration = 120;

		// Token: 0x040066E1 RID: 26337
		public float pointsMultiplier = 1f;

		// Token: 0x040066E2 RID: 26338
		private int tickOpened = -999999;

		// Token: 0x040066E3 RID: 26339
		private int lastIncidentTick = -999999;

		// Token: 0x040066E4 RID: 26340
		private int nextIncidentTick = -999999;

		// Token: 0x040066E5 RID: 26341
		private int collapseTick = -999999;

		// Token: 0x040066E6 RID: 26342
		private bool isCollapsing;

		// Token: 0x040066E7 RID: 26343
		private PitGateIncidentWorker currentIncident;

		// Token: 0x040066E8 RID: 26344
		private int workerEnd = -999999;

		// Token: 0x040066E9 RID: 26345
		private int numIncidentsFired;

		// Token: 0x040066EA RID: 26346
		private Sustainer collapseSustainer;

		// Token: 0x040066EB RID: 26347
		private Effecter collapseEffecter1;

		// Token: 0x040066EC RID: 26348
		private Effecter collapseEffecter2;
	}
}
