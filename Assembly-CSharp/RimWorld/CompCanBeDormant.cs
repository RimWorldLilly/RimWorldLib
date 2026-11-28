using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001845 RID: 6213
	public class CompCanBeDormant : ThingComp
	{
		// Token: 0x17001A89 RID: 6793
		// (get) Token: 0x06009B84 RID: 39812 RVA: 0x00380DB0 File Offset: 0x0037EFB0
		public CompProperties_CanBeDormant Props
		{
			get
			{
				return (CompProperties_CanBeDormant)this.props;
			}
		}

		// Token: 0x17001A8A RID: 6794
		// (get) Token: 0x06009B85 RID: 39813 RVA: 0x00380DBD File Offset: 0x0037EFBD
		private bool WaitingToWakeUp
		{
			get
			{
				return this.wakeUpOnTick != int.MinValue;
			}
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x06009B86 RID: 39814 RVA: 0x00380DCF File Offset: 0x0037EFCF
		protected virtual JobDef SleepJob
		{
			get
			{
				return JobDefOf.Wait_AsleepDormancy;
			}
		}

		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x06009B87 RID: 39815 RVA: 0x00380DD8 File Offset: 0x0037EFD8
		public virtual bool Awake
		{
			get
			{
				if (this.wokeUpTick == -2147483648 || this.wokeUpTick > Find.TickManager.TicksGame)
				{
					if (this.parent.Spawned && this.Props.jobDormancy)
					{
						Pawn pawn = this.parent as Pawn;
						if (pawn != null)
						{
							return pawn.CurJobDef != this.SleepJob;
						}
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x06009B88 RID: 39816 RVA: 0x00380E42 File Offset: 0x0037F042
		protected virtual bool ShowZs
		{
			get
			{
				return !(this.parent is Pawn) && this.Props.showSleepingZs && (this.Props.delayedWakeUpDoesZs || this.wakeUpOnTick == int.MinValue);
			}
		}

		// Token: 0x06009B89 RID: 39817 RVA: 0x00380E7E File Offset: 0x0037F07E
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.makeTick = GenTicks.TicksGame;
			if (!this.Props.startsDormant)
			{
				this.WakeUp();
			}
		}

		// Token: 0x06009B8A RID: 39818 RVA: 0x00380EA4 File Offset: 0x0037F0A4
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpSignalTag = this.Props.wakeUpSignalTag;
		}

		// Token: 0x06009B8B RID: 39819 RVA: 0x00380EBE File Offset: 0x0037F0BE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompCanBeDormant.<CompGetGizmosExtra>d__20 <CompGetGizmosExtra>d__ = new CompCanBeDormant.<CompGetGizmosExtra>d__20(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009B8C RID: 39820 RVA: 0x00380ED0 File Offset: 0x0037F0D0
		public override string CompInspectStringExtra()
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && (pawn.IsSelfShutdown() || pawn.IsDeactivated() || pawn.Awake()))
			{
				return null;
			}
			if (this.Awake)
			{
				if (this.parent.Faction != Faction.OfPlayer && this.makeTick != this.wokeUpTick)
				{
					Pawn pawn2 = this.parent as Pawn;
					if (pawn2 == null || !pawn2.Downed)
					{
						return this.Props.awakeStateLabelKey.Translate((GenTicks.TicksGame - this.wokeUpTick).TicksToDays().ToString("0.#"));
					}
				}
				return null;
			}
			if (!this.Props.wakeUpDelayStateLabelKey.NullOrEmpty() && this.wakeUpOnTick != -2147483648)
			{
				return this.Props.wakeUpDelayStateLabelKey.Translate();
			}
			return this.Props.dormantStateLabelKey.Translate();
		}

		// Token: 0x06009B8D RID: 39821 RVA: 0x00380FCC File Offset: 0x0037F1CC
		public void WakeUpWithDelay()
		{
			if (!this.Awake)
			{
				this.wakeUpOnTick = Find.TickManager.TicksGame + this.Props.wakeUpDelayRange.RandomInRange;
				this.repeatWakeUpSignalOnTick = ((this.Props.wakeUpRepeatSignalDelayRange != null) ? (Find.TickManager.TicksGame + this.Props.wakeUpRepeatSignalDelayRange.Value.RandomInRange) : int.MinValue);
			}
		}

		// Token: 0x06009B8E RID: 39822 RVA: 0x00381044 File Offset: 0x0037F244
		public virtual void WakeUp()
		{
			if (this.Awake)
			{
				return;
			}
			this.wokeUpTick = GenTicks.TicksGame;
			this.wakeUpOnTick = int.MinValue;
			Pawn pawn = this.parent as Pawn;
			Building building = this.parent as Building;
			Lord lord = ((pawn != null) ? pawn.GetLord() : null) ?? ((building != null) ? building.GetLord() : null);
			if (lord != null)
			{
				lord.Notify_DormancyWakeup();
			}
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
				if (this.Props.jobDormancy)
				{
					Pawn pawn2 = this.parent as Pawn;
					if (pawn2 != null && pawn2.CurJobDef == this.SleepJob)
					{
						pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x06009B8F RID: 39823 RVA: 0x0038111C File Offset: 0x0037F31C
		public virtual void ToSleep()
		{
			if (!this.Awake)
			{
				return;
			}
			this.wokeUpTick = int.MinValue;
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
				if (this.Props.jobDormancy)
				{
					Pawn pawn = this.parent as Pawn;
					if (pawn != null)
					{
						Job job = JobMaker.MakeJob(this.SleepJob, pawn.Position);
						job.forceSleep = true;
						pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					}
				}
			}
		}

		// Token: 0x06009B90 RID: 39824 RVA: 0x003811D6 File Offset: 0x0037F3D6
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			this.TickRareWorker();
		}

		// Token: 0x06009B91 RID: 39825 RVA: 0x0038120C File Offset: 0x0037F40C
		public override void CompTick()
		{
			base.CompTick();
			if (this.wakeUpOnTick != -2147483648)
			{
				if (Find.TickManager.TicksGame >= this.wakeUpOnTick)
				{
					this.WakeUp();
					Effecter effecter = this.wakeUpEffect;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.wakeUpEffect = null;
				}
				else
				{
					if (this.Props.wakeUpEffect != null && this.wakeUpEffect == null)
					{
						this.wakeUpEffect = this.Props.wakeUpEffect.SpawnAttached(this.parent, this.parent.Map, 1f);
					}
					Effecter effecter2 = this.wakeUpEffect;
					if (effecter2 != null)
					{
						effecter2.EffectTick(this.parent, this.parent);
					}
					if (this.repeatWakeUpSignalOnTick != -2147483648 && Find.TickManager.TicksGame == this.repeatWakeUpSignalOnTick)
					{
						Find.SignalManager.SendSignal(new Signal(this.wakeUpSignalTag, this.parent.Named("SUBJECT")));
					}
				}
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x06009B92 RID: 39826 RVA: 0x00381328 File Offset: 0x0037F528
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.Awake)
			{
				return;
			}
			if (this.ShowZs && !this.parent.Position.Fogged(this.parent.Map))
			{
				FleckMaker.ThrowMetaIcon(this.parent.Position, this.parent.Map, FleckDefOf.SleepZ, 0.42f);
			}
		}

		// Token: 0x06009B93 RID: 39827 RVA: 0x00381398 File Offset: 0x0037F598
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.wakeUpSignalTag) || this.Awake)
			{
				return;
			}
			Thing thing;
			Faction faction;
			if ((signal.tag == this.wakeUpSignalTag || (this.wakeUpSignalTags != null && this.wakeUpSignalTags.Contains(signal.tag))) && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= this.Props.maxDistAwakenByOther && (!signal.args.TryGetArg<Faction>("FACTION", out faction) || faction == null || faction == this.parent.Faction) && (this.Props.canWakeUpFogged || !this.parent.Fogged()) && !this.WaitingToWakeUp)
			{
				this.WakeUpWithDelay();
			}
		}

		// Token: 0x06009B94 RID: 39828 RVA: 0x003814A0 File Offset: 0x0037F6A0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.wokeUpTick, "wokeUpTick", int.MinValue, false);
			Scribe_Values.Look<int>(ref this.wakeUpOnTick, "wakeUpOnTick", int.MinValue, false);
			Scribe_Values.Look<string>(ref this.wakeUpSignalTag, "wakeUpSignalTag", null, false);
			Scribe_Collections.Look<string>(ref this.wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.makeTick, "makeTick", 0, false);
			Scribe_Values.Look<int>(ref this.repeatWakeUpSignalOnTick, "repeatWakeUpSignalOnTick", int.MinValue, false);
		}

		// Token: 0x04005C7C RID: 23676
		private int makeTick;

		// Token: 0x04005C7D RID: 23677
		public int wokeUpTick = int.MinValue;

		// Token: 0x04005C7E RID: 23678
		public int wakeUpOnTick = int.MinValue;

		// Token: 0x04005C7F RID: 23679
		private int repeatWakeUpSignalOnTick = int.MaxValue;

		// Token: 0x04005C80 RID: 23680
		private Effecter wakeUpEffect;

		// Token: 0x04005C81 RID: 23681
		public string wakeUpSignalTag;

		// Token: 0x04005C82 RID: 23682
		public List<string> wakeUpSignalTags;

		// Token: 0x04005C83 RID: 23683
		public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";
	}
}
