using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015A4 RID: 5540
	[StaticConstructorOnStartup]
	public class Pawn_RopeTracker : IExposable
	{
		// Token: 0x060088C6 RID: 35014 RVA: 0x00317E5B File Offset: 0x0031605B
		public Pawn_RopeTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x060088C7 RID: 35015 RVA: 0x00317E80 File Offset: 0x00316080
		public bool IsRoped
		{
			get
			{
				return this.ropedTo.IsValid;
			}
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x060088C8 RID: 35016 RVA: 0x00317E8D File Offset: 0x0031608D
		public bool IsRopedByPawn
		{
			get
			{
				return this.RopedByPawn != null;
			}
		}

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x060088C9 RID: 35017 RVA: 0x00317E98 File Offset: 0x00316098
		public bool IsRopedToSpot
		{
			get
			{
				return this.ropedTo.IsValid && !this.IsRopedByPawn;
			}
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x060088CA RID: 35018 RVA: 0x00317EB2 File Offset: 0x003160B2
		public bool IsRopedToHitchingPost
		{
			get
			{
				return this.RopedToHitchingSpot != null;
			}
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x060088CB RID: 35019 RVA: 0x00317EBD File Offset: 0x003160BD
		public bool IsRopingOthers
		{
			get
			{
				return this.Ropees.Any<Pawn>();
			}
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x060088CC RID: 35020 RVA: 0x00317ECA File Offset: 0x003160CA
		public bool HasAnyRope
		{
			get
			{
				return this.IsRoped || this.IsRopingOthers;
			}
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x060088CD RID: 35021 RVA: 0x00317EDC File Offset: 0x003160DC
		public Pawn RopedByPawn
		{
			get
			{
				return this.ropedTo.Thing as Pawn;
			}
		}

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x060088CE RID: 35022 RVA: 0x00317EEE File Offset: 0x003160EE
		public IntVec3 RopedToSpot
		{
			get
			{
				if (!this.IsRopedToSpot)
				{
					return IntVec3.Invalid;
				}
				return this.ropedTo.Cell;
			}
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x060088CF RID: 35023 RVA: 0x00317F09 File Offset: 0x00316109
		public LocalTargetInfo RopedTo
		{
			get
			{
				return this.ropedTo;
			}
		}

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x060088D0 RID: 35024 RVA: 0x00317F11 File Offset: 0x00316111
		public List<Pawn> Ropees
		{
			get
			{
				return this.ropees;
			}
		}

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x060088D1 RID: 35025 RVA: 0x00317F19 File Offset: 0x00316119
		public Building RopedToHitchingSpot
		{
			get
			{
				return this.hitchingSpotInt;
			}
		}

		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x060088D2 RID: 35026 RVA: 0x00317F24 File Offset: 0x00316124
		public bool AnyRopeesFenceBlocked
		{
			get
			{
				if (this.ropees.Count == 0)
				{
					return false;
				}
				for (int i = 0; i < this.ropees.Count; i++)
				{
					if (this.ropees[i].FenceBlocked)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x060088D3 RID: 35027 RVA: 0x00317F6C File Offset: 0x0031616C
		public string InspectLine
		{
			get
			{
				if (this.IsRopedByPawn)
				{
					return "RopedByPawn".Translate() + ": " + this.RopedByPawn.LabelShort;
				}
				if (this.ropedTo.HasThing)
				{
					return "RopedToThing".Translate() + ": " + this.ropedTo.Label;
				}
				return "RopedToSpot".Translate();
			}
		}

		// Token: 0x060088D4 RID: 35028 RVA: 0x00317FF4 File Offset: 0x003161F4
		public void RopingTick()
		{
			if (!this.HasAnyRope)
			{
				return;
			}
			if (this.pawn.Dead || this.pawn.Downed || this.pawn.Drafted || (!this.pawn.Awake() && this.IsRopedByPawn) || this.ShouldDropRopesDueToMentalState() || this.pawn.IsBurning())
			{
				this.BreakAllRopes();
				return;
			}
			if (this.ropees.Any<Pawn>() && !this.IsStillDoingRopingJob(this.pawn))
			{
				this.BreakAllRopes();
				return;
			}
			if (this.ropedTo.IsValid && !this.pawn.CanReach(this.ropedTo, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				this.BreakRopeWithRoper();
			}
			if (this.IsRopedToHitchingPost && !this.RopedToHitchingSpot.Spawned)
			{
				this.UnropeFromSpot();
			}
		}

		// Token: 0x060088D5 RID: 35029 RVA: 0x003180CA File Offset: 0x003162CA
		private bool IsStillDoingRopingJob(Pawn roper)
		{
			return roper.jobs.curJob == null || roper.jobs.curDriver is JobDriver_RopeToDestination;
		}

		// Token: 0x060088D6 RID: 35030 RVA: 0x003180EE File Offset: 0x003162EE
		private bool ShouldDropRopesDueToMentalState()
		{
			return this.pawn.InMentalState && this.pawn.MentalStateDef != MentalStateDefOf.Roaming;
		}

		// Token: 0x060088D7 RID: 35031 RVA: 0x00318114 File Offset: 0x00316314
		public void RopingDraw()
		{
			if (!this.ropedTo.IsValid)
			{
				return;
			}
			Vector3 vector = this.ropedTo.CenterVector3.Yto0();
			if (!this.ropedTo.HasThing && this.ropedTo.Cell.GetFirstThing(this.pawn.Map, ThingDefOf.CaravanPackingSpot) != null)
			{
				vector += Pawn_RopeTracker.CaravanHitchingPostOffset;
			}
			GenDraw.DrawLineBetween(this.pawn.DrawPos.Yto0(), vector, AltitudeLayer.PawnRope.AltitudeFor(), Pawn_RopeTracker.RopeLineMat, 0.2f);
		}

		// Token: 0x060088D8 RID: 35032 RVA: 0x003181A2 File Offset: 0x003163A2
		public void Notify_DeSpawned()
		{
			this.BreakAllRopes();
		}

		// Token: 0x060088D9 RID: 35033 RVA: 0x003181AA File Offset: 0x003163AA
		private void BreakRopeWithRoper()
		{
			Pawn ropedByPawn = this.RopedByPawn;
			if (ropedByPawn != null)
			{
				ropedByPawn.roping.DropRope(this.pawn);
			}
			this.ropedTo = LocalTargetInfo.Invalid;
		}

		// Token: 0x060088DA RID: 35034 RVA: 0x003181D3 File Offset: 0x003163D3
		public void BreakAllRopes()
		{
			this.BreakRopeWithRoper();
			this.DropRopes();
			this.TryUnropeFromHitchingSpot();
		}

		// Token: 0x060088DB RID: 35035 RVA: 0x003181E8 File Offset: 0x003163E8
		public void RopeToSpot(IntVec3 spot)
		{
			Building edifice = spot.GetEdifice(this.pawn.Map);
			Pawn_RopeTracker.CreateRope(spot, this.pawn);
			if (edifice != null && edifice.def == ThingDefOf.CaravanPackingSpot)
			{
				this.hitchingSpotInt = edifice;
				this.RopedToHitchingSpot.GetComp<CompHitchingSpot>().AddPawn(this.pawn);
			}
		}

		// Token: 0x060088DC RID: 35036 RVA: 0x00318245 File Offset: 0x00316445
		public void TryUnropeFromHitchingSpot()
		{
			if (this.RopedToHitchingSpot != null)
			{
				this.RopedToHitchingSpot.GetComp<CompHitchingSpot>().RemovePawn(this.pawn);
			}
			this.hitchingSpotInt = null;
		}

		// Token: 0x060088DD RID: 35037 RVA: 0x0031826C File Offset: 0x0031646C
		public void RopePawn(Pawn ropee)
		{
			Pawn_RopeTracker.CreateRope(this.pawn, ropee);
		}

		// Token: 0x060088DE RID: 35038 RVA: 0x00318280 File Offset: 0x00316480
		private static void CreateRope(LocalTargetInfo roperTarget, Pawn ropee)
		{
			Pawn pawn = roperTarget.Thing as Pawn;
			ropee.roping.DropRopes();
			ropee.roping.TryUnropeFromHitchingSpot();
			ropee.roping.ropedTo = roperTarget;
			if (pawn != null)
			{
				pawn.roping.ropees.Add(ropee);
				ReachabilityUtility.ClearCacheFor(pawn);
			}
			ReachabilityUtility.ClearCacheFor(ropee);
			if (ropee.jobs != null && ropee.CurJob != null)
			{
				ropee.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x060088DF RID: 35039 RVA: 0x003182FC File Offset: 0x003164FC
		public void UnropeFromSpot()
		{
			if (!this.IsRopedToSpot)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to unrope ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" from spot, but not roped to a spot");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.TryUnropeFromHitchingSpot();
			this.ropedTo = LocalTargetInfo.Invalid;
			ReachabilityUtility.ClearCacheFor(this.pawn);
		}

		// Token: 0x060088E0 RID: 35040 RVA: 0x0031836C File Offset: 0x0031656C
		public void DropRope(Pawn ropee)
		{
			if (this.pawn != ropee.roping.RopedByPawn)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 3);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to drop for ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(ropee);
				defaultInterpolatedStringHandler.AppendLiteral(" but ropee holder was ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(ropee.roping.RopedByPawn);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.TryUnropeFromHitchingSpot();
			ropee.roping.ropedTo = LocalTargetInfo.Invalid;
			this.ropees.Remove(ropee);
			ReachabilityUtility.ClearCacheFor(ropee);
			ReachabilityUtility.ClearCacheFor(this.pawn);
		}

		// Token: 0x060088E1 RID: 35041 RVA: 0x00318418 File Offset: 0x00316618
		public void DropRopes()
		{
			if (this.ropees.Count == 0)
			{
				return;
			}
			foreach (Pawn ropee in new List<Pawn>(this.ropees))
			{
				this.DropRope(ropee);
			}
		}

		// Token: 0x060088E2 RID: 35042 RVA: 0x00318480 File Offset: 0x00316680
		public void ExposeData()
		{
			Scribe_TargetInfo.Look(ref this.ropedTo, "ropedTo", LocalTargetInfo.Invalid);
			Scribe_References.Look<Building>(ref this.hitchingSpotInt, "hitchingPostInt", false);
			Scribe_Collections.Look<Pawn>(ref this.ropees, "ropees", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ropees.RemoveAll((Pawn x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x060088E3 RID: 35043 RVA: 0x003184FC File Offset: 0x003166FC
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
			stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			if (this.ropedTo.HasThing)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(10, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" ropedBy: ");
				defaultInterpolatedStringHandler2.AppendFormatted<Thing>(this.ropedTo.Thing);
				stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			else if (this.ropedTo.IsValid)
			{
				StringBuilder stringBuilder4 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler3.AppendLiteral(" roped at: ");
				defaultInterpolatedStringHandler3.AppendFormatted<IntVec3>(this.ropedTo.Cell);
				stringBuilder4.Append(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			stringBuilder.AppendLine();
			foreach (Pawn value in this.Ropees)
			{
				StringBuilder stringBuilder5 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(9, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("  ropee: ");
				defaultInterpolatedStringHandler4.AppendFormatted<Pawn>(value);
				stringBuilder5.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400545B RID: 21595
		public const float RopeLength = 8f;

		// Token: 0x0400545C RID: 21596
		private static readonly Vector3 CaravanHitchingPostOffset = new Vector3(0f, 0f, 0.19f);

		// Token: 0x0400545D RID: 21597
		private static readonly string RopeTexPath = "UI/Overlays/Rope";

		// Token: 0x0400545E RID: 21598
		private static readonly Material RopeLineMat = MaterialPool.MatFrom(Pawn_RopeTracker.RopeTexPath, ShaderDatabase.Transparent, GenColor.FromBytes(99, 70, 41, 255));

		// Token: 0x0400545F RID: 21599
		private Pawn pawn;

		// Token: 0x04005460 RID: 21600
		private LocalTargetInfo ropedTo = LocalTargetInfo.Invalid;

		// Token: 0x04005461 RID: 21601
		private List<Pawn> ropees = new List<Pawn>();

		// Token: 0x04005462 RID: 21602
		private Building hitchingSpotInt;
	}
}
