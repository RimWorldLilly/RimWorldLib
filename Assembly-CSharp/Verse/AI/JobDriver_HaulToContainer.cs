using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000836 RID: 2102
	public class JobDriver_HaulToContainer : JobDriver, IBuildableDriver
	{
		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x001A0854 File Offset: 0x0019EA54
		public Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x001A0867 File Offset: 0x0019EA67
		public Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x001A087A File Offset: 0x0019EA7A
		public ThingDef ThingDef
		{
			get
			{
				return this.ThingToCarry.def;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x001A0888 File Offset: 0x0019EA88
		protected virtual int Duration
		{
			get
			{
				if (this.Container != null)
				{
					Building building = this.Container as Building;
					if (building != null)
					{
						return building.HaulToContainerDuration(this.ThingToCarry);
					}
				}
				return 0;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x001A08BA File Offset: 0x0019EABA
		protected virtual EffecterDef WorkEffecter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x060041CF RID: 16847 RVA: 0x001A08BD File Offset: 0x0019EABD
		protected virtual SoundDef WorkSustainer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x001A08C0 File Offset: 0x0019EAC0
		public bool TryGetBuildableRect(out CellRect rect)
		{
			if (this.Container is Blueprint)
			{
				rect = this.Container.OccupiedRect();
				return true;
			}
			rect = default(CellRect);
			return false;
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x001A08EC File Offset: 0x0019EAEC
		public override string GetReport()
		{
			Thing thing;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			if (thing == null || !this.job.targetB.HasThing)
			{
				return "ReportHaulingUnknown".Translate();
			}
			return ((this.job.GetTarget(TargetIndex.B).Thing is Building_Grave) ? "ReportHaulingToGrave" : "ReportHaulingTo").Translate(thing.Label, this.job.targetB.Thing.LabelShort.Named("DESTINATION"), thing.Named("THING"));
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x001A09C4 File Offset: 0x0019EBC4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.Container.Isnt<IHaulEnroute>())
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, 1, null, errorOnFailed, false))
				{
					return false;
				}
				this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			}
			this.UpdateEnrouteTrackers();
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x001A0A71 File Offset: 0x0019EC71
		protected virtual void ModifyPrepareToil(Toil toil)
		{
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x001A0A74 File Offset: 0x0019EC74
		private bool TryReplaceWithFrame(TargetIndex index)
		{
			Thing thing = base.GetActor().jobs.curJob.GetTarget(index).Thing;
			Building edifice = thing.Position.GetEdifice(this.pawn.Map);
			if (edifice != null)
			{
				Blueprint_Build blueprint_Build = thing as Blueprint_Build;
				if (blueprint_Build != null)
				{
					Frame frame = edifice as Frame;
					if (frame != null && frame.BuildDef == blueprint_Build.BuildDef)
					{
						this.job.SetTarget(TargetIndex.B, frame);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x001A0AF2 File Offset: 0x0019ECF2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HaulToContainer.<MakeNewToils>d__22 <MakeNewToils>d__ = new JobDriver_HaulToContainer.<MakeNewToils>d__22(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x001A0B04 File Offset: 0x0019ED04
		private void UpdateEnrouteTrackers()
		{
			int count = this.job.count;
			this.TryReserveEnroute(base.TargetThingC, ref count);
			if (base.TargetB != base.TargetC)
			{
				this.TryReserveEnroute(base.TargetThingB, ref count);
			}
			if (this.job.targetQueueB != null)
			{
				foreach (LocalTargetInfo a in this.job.targetQueueB)
				{
					if (!base.TargetC.HasThing || !(a == base.TargetThingC))
					{
						this.TryReserveEnroute(a.Thing, ref count);
					}
				}
			}
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x001A0BD0 File Offset: 0x0019EDD0
		private void TryReserveEnroute(Thing thing, ref int count)
		{
			IHaulEnroute haulEnroute = thing as IHaulEnroute;
			if (haulEnroute != null && !thing.DestroyedOrNull())
			{
				this.UpdateTracker(haulEnroute, ref count);
			}
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x001A0BF8 File Offset: 0x0019EDF8
		private void UpdateTracker(IHaulEnroute container, ref int count)
		{
			if (this.ThingToCarry.DestroyedOrNull())
			{
				return;
			}
			if (this.job.playerForced && container.GetSpaceRemainingWithEnroute(this.ThingDef, null) == 0)
			{
				container.Map.enrouteManager.InterruptEnroutePawns(container, this.pawn);
			}
			int num = Mathf.Min(count, container.GetSpaceRemainingWithEnroute(this.ThingDef, null));
			if (num > 0)
			{
				container.Map.enrouteManager.AddEnroute(container, this.pawn, base.TargetThingA.def, num);
			}
			count -= num;
		}

		// Token: 0x04002B39 RID: 11065
		private Effecter graveDigEffect;

		// Token: 0x04002B3A RID: 11066
		protected const TargetIndex CarryThingIndex = TargetIndex.A;

		// Token: 0x04002B3B RID: 11067
		public const TargetIndex DestIndex = TargetIndex.B;

		// Token: 0x04002B3C RID: 11068
		protected const TargetIndex PrimaryDestIndex = TargetIndex.C;

		// Token: 0x04002B3D RID: 11069
		protected const int DiggingEffectInterval = 80;
	}
}
