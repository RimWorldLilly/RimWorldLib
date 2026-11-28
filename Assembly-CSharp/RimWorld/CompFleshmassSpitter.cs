using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200188F RID: 6287
	public class CompFleshmassSpitter : CompFleshmassHeartChild, IAttackTargetSearcher, IVerbOwner
	{
		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x06009D61 RID: 40289 RVA: 0x0038973F File Offset: 0x0038793F
		private Verb AttackVerb
		{
			get
			{
				return this.AllVerbs[0];
			}
		}

		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x06009D62 RID: 40290 RVA: 0x0038974D File Offset: 0x0038794D
		private int TicksToNextSpit
		{
			get
			{
				return this.lastSpitTick + this.nextSpitDelay - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x06009D63 RID: 40291 RVA: 0x00389767 File Offset: 0x00387967
		private bool OnCooldown
		{
			get
			{
				return this.TicksToNextSpit > 0;
			}
		}

		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x06009D64 RID: 40292 RVA: 0x00389772 File Offset: 0x00387972
		public Thing Thing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x06009D65 RID: 40293 RVA: 0x0038977A File Offset: 0x0038797A
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x06009D66 RID: 40294 RVA: 0x00389782 File Offset: 0x00387982
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x06009D67 RID: 40295 RVA: 0x0038978A File Offset: 0x0038798A
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastSpitTick;
			}
		}

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x06009D68 RID: 40296 RVA: 0x00389794 File Offset: 0x00387994
		public VerbTracker VerbTracker
		{
			get
			{
				VerbTracker result;
				if ((result = this.verbTracker) == null)
				{
					result = (this.verbTracker = new VerbTracker(this));
				}
				return result;
			}
		}

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x06009D69 RID: 40297 RVA: 0x003897BA File Offset: 0x003879BA
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x06009D6A RID: 40298 RVA: 0x003897CC File Offset: 0x003879CC
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x06009D6B RID: 40299 RVA: 0x003897DE File Offset: 0x003879DE
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x06009D6C RID: 40300 RVA: 0x003897E5 File Offset: 0x003879E5
		public Thing ConstantCaster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06009D6D RID: 40301 RVA: 0x003897ED File Offset: 0x003879ED
		public string UniqueVerbOwnerID()
		{
			return "CompFleshmassSpitter_" + this.parent.ThingID;
		}

		// Token: 0x06009D6E RID: 40302 RVA: 0x00389804 File Offset: 0x00387A04
		public bool VerbsStillUsableBy(Pawn p)
		{
			return false;
		}

		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x06009D6F RID: 40303 RVA: 0x00389807 File Offset: 0x00387A07
		public List<Verb> AllVerbs
		{
			get
			{
				return this.VerbTracker.AllVerbs;
			}
		}

		// Token: 0x06009D70 RID: 40304 RVA: 0x00389814 File Offset: 0x00387A14
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.SetupVerbs();
			if (!respawningAfterLoad)
			{
				this.lastSpitTick = Find.TickManager.TicksGame;
				this.nextSpitDelay = CompFleshmassSpitter.SpitIntervalRangeTicks.RandomInRange;
			}
		}

		// Token: 0x06009D71 RID: 40305 RVA: 0x00389850 File Offset: 0x00387A50
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpitDelay, "nextSpitDelay", 0, false);
			Scribe_Values.Look<int>(ref this.lastSpitTick, "lastSpitTick", 0, false);
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06009D72 RID: 40306 RVA: 0x003898B1 File Offset: 0x00387AB1
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompFleshmassSpitter.<CompGetGizmosExtra>d__39 <CompGetGizmosExtra>d__ = new CompFleshmassSpitter.<CompGetGizmosExtra>d__39(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009D73 RID: 40307 RVA: 0x003898C4 File Offset: 0x00387AC4
		private void SetupVerbs()
		{
			foreach (Verb verb in this.AllVerbs)
			{
				verb.caster = this.parent;
			}
		}

		// Token: 0x06009D74 RID: 40308 RVA: 0x0038991C File Offset: 0x00387B1C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.ForceEnd();
			}
			this.progressBarEffecter = null;
		}

		// Token: 0x06009D75 RID: 40309 RVA: 0x00389938 File Offset: 0x00387B38
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.OnCooldown)
			{
				if (this.progressBarEffecter == null)
				{
					this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
				}
				this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
				mote.progress = 1f - (float)Mathf.Max(this.TicksToNextSpit, 0) / (float)this.nextSpitDelay;
				mote.offsetZ = -0.8f;
				this.parent.overrideGraphicIndex = new int?(1);
			}
			else
			{
				this.parent.overrideGraphicIndex = new int?(0);
			}
			if (this.TicksToNextSpit <= 0 && this.parent.IsHashIntervalTick(180))
			{
				Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, TargetScanFlags.NeedThreat, (Thing t) => !t.Position.Roofed(t.Map), 0f, 9999f);
				if (thing == null)
				{
					return;
				}
				this.AttackVerb.TryStartCastOn(thing, false, true, false, false);
				this.lastSpitTick = Find.TickManager.TicksGame;
				this.nextSpitDelay = CompFleshmassSpitter.SpitIntervalRangeTicks.RandomInRange;
				Messages.Message("SpitterAttacking".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06009D76 RID: 40310 RVA: 0x00389ABB File Offset: 0x00387CBB
		public override void PostDrawExtraSelectionOverlays()
		{
			this.AttackVerb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x06009D77 RID: 40311 RVA: 0x00389AD0 File Offset: 0x00387CD0
		public override string CompInspectStringExtra()
		{
			if (this.TicksToNextSpit > 0)
			{
				return "SpitterGatheringSpit".Translate() + ": " + this.TicksToNextSpit.ToStringTicksToPeriod(true, false, true, true, false);
			}
			return "SpitterReadyToSpit".Translate();
		}

		// Token: 0x04005D41 RID: 23873
		private static readonly IntRange SpitIntervalRangeTicks = new IntRange(5000, 7500);

		// Token: 0x04005D42 RID: 23874
		private const int SpitCheckIntervalTicks = 180;

		// Token: 0x04005D43 RID: 23875
		private const int ReadySpriteIndex = 0;

		// Token: 0x04005D44 RID: 23876
		private const int CooldownSpriteIndex = 1;

		// Token: 0x04005D45 RID: 23877
		private int lastSpitTick = -99999;

		// Token: 0x04005D46 RID: 23878
		private int nextSpitDelay = -99999;

		// Token: 0x04005D47 RID: 23879
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x04005D48 RID: 23880
		private VerbTracker verbTracker;

		// Token: 0x04005D49 RID: 23881
		private Effecter progressBarEffecter;
	}
}
