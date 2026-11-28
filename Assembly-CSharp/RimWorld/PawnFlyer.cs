using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B34 RID: 6964
	public class PawnFlyer : Thing, IThingHolderTickable, IThingHolder, ISearchableContents
	{
		// Token: 0x17002037 RID: 8247
		// (get) Token: 0x0600B29D RID: 45725 RVA: 0x003EE2A8 File Offset: 0x003EC4A8
		public bool ShouldTickContents
		{
			get
			{
				return this.ticksFlying < this.ticksFlightTime;
			}
		}

		// Token: 0x17002038 RID: 8248
		// (get) Token: 0x0600B29E RID: 45726 RVA: 0x003EE2B8 File Offset: 0x003EC4B8
		protected Thing FlyingThing
		{
			get
			{
				if (this.innerContainer.InnerListForReading.Count <= 0)
				{
					return null;
				}
				return this.innerContainer.InnerListForReading[0];
			}
		}

		// Token: 0x17002039 RID: 8249
		// (get) Token: 0x0600B29F RID: 45727 RVA: 0x003EE2E0 File Offset: 0x003EC4E0
		public Pawn FlyingPawn
		{
			get
			{
				return this.FlyingThing as Pawn;
			}
		}

		// Token: 0x1700203A RID: 8250
		// (get) Token: 0x0600B2A0 RID: 45728 RVA: 0x003EE2ED File Offset: 0x003EC4ED
		public Thing CarriedThing
		{
			get
			{
				return this.carriedThing;
			}
		}

		// Token: 0x1700203B RID: 8251
		// (get) Token: 0x0600B2A1 RID: 45729 RVA: 0x003EE2F5 File Offset: 0x003EC4F5
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x1700203C RID: 8252
		// (get) Token: 0x0600B2A2 RID: 45730 RVA: 0x003EE2FD File Offset: 0x003EC4FD
		public override Vector3 DrawPos
		{
			get
			{
				this.RecomputePosition();
				return this.effectivePos;
			}
		}

		// Token: 0x1700203D RID: 8253
		// (get) Token: 0x0600B2A3 RID: 45731 RVA: 0x003EE30B File Offset: 0x003EC50B
		public override int UpdateRateTicks
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600B2A4 RID: 45732 RVA: 0x003EE310 File Offset: 0x003EC510
		private void RecomputePosition()
		{
			if (this.positionLastComputedTick == this.ticksFlying)
			{
				return;
			}
			this.positionLastComputedTick = this.ticksFlying;
			float t = (float)this.ticksFlying / (float)this.ticksFlightTime;
			float t2 = this.def.pawnFlyer.Worker.AdjustedProgress(t);
			this.effectiveHeight = this.def.pawnFlyer.Worker.GetHeight(t2);
			this.groundPos = Vector3.Lerp(this.startVec, this.DestinationPos, t2);
			Vector3 b = Altitudes.AltIncVect * this.effectiveHeight;
			Vector3 b2 = Vector3.forward * (this.def.pawnFlyer.heightFactor * this.effectiveHeight);
			this.effectivePos = this.groundPos + b + b2;
			base.Position = this.groundPos.ToIntVec3();
		}

		// Token: 0x0600B2A5 RID: 45733 RVA: 0x003EE3EF File Offset: 0x003EC5EF
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600B2A6 RID: 45734 RVA: 0x003EE3F7 File Offset: 0x003EC5F7
		public PawnFlyer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600B2A7 RID: 45735 RVA: 0x003EE421 File Offset: 0x003EC621
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.flightEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			base.Destroy(mode);
		}

		// Token: 0x0600B2A8 RID: 45736 RVA: 0x003EE43B File Offset: 0x003EC63B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x1700203E RID: 8254
		// (get) Token: 0x0600B2A9 RID: 45737 RVA: 0x003EE44C File Offset: 0x003EC64C
		public Vector3 DestinationPos
		{
			get
			{
				Thing flyingThing = this.FlyingThing;
				return GenThing.TrueCenter(this.destCell, flyingThing.Rotation, flyingThing.def.size, flyingThing.def.Altitude);
			}
		}

		// Token: 0x0600B2AA RID: 45738 RVA: 0x003EE488 File Offset: 0x003EC688
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				float num = Mathf.Max(this.flightDistance, 1f) / this.def.pawnFlyer.flightSpeed;
				num = Mathf.Max(num, this.def.pawnFlyer.flightDurationMin);
				this.ticksFlightTime = num.SecondsToTicks();
				this.ticksFlying = 0;
			}
		}

		// Token: 0x0600B2AB RID: 45739 RVA: 0x003EE4F4 File Offset: 0x003EC6F4
		protected virtual void RespawnPawn()
		{
			Thing flyingThing = this.FlyingThing;
			this.LandingEffects();
			Thing thing;
			this.innerContainer.TryDrop(flyingThing, this.destCell, flyingThing.MapHeld, ThingPlaceMode.Direct, out thing, null, null, false);
			Pawn pawn = flyingThing as Pawn;
			if (((pawn != null) ? pawn.drafter : null) != null)
			{
				pawn.drafter.Drafted = this.pawnWasDrafted;
				pawn.drafter.FireAtWill = this.pawnCanFireAtWill;
			}
			flyingThing.Rotation = base.Rotation;
			if (this.carriedThing != null && this.innerContainer.TryDrop(this.carriedThing, this.destCell, flyingThing.MapHeld, ThingPlaceMode.Direct, out thing, null, null, false) && pawn != null)
			{
				this.carriedThing.DeSpawn(DestroyMode.Vanish);
				if (!pawn.carryTracker.TryStartCarry(this.carriedThing))
				{
					Log.Error("Could not carry " + this.carriedThing.ToStringSafe<Thing>() + " after respawning flyer pawn.");
				}
			}
			if (pawn != null)
			{
				if (this.jobQueue != null)
				{
					pawn.jobs.RestoreCapturedJobs(this.jobQueue, true);
				}
				pawn.jobs.CheckForJobOverride(0f, false);
				if (this.def.pawnFlyer.stunDurationTicksRange != IntRange.Zero)
				{
					pawn.stances.stunner.StunFor(this.def.pawnFlyer.stunDurationTicksRange.RandomInRange, null, false, false, false);
				}
				if (this.triggeringAbility != null)
				{
					Ability ability = pawn.abilities.GetAbility(this.triggeringAbility, false);
					if (((ability != null) ? ability.comps : null) != null)
					{
						foreach (AbilityComp abilityComp in ability.comps)
						{
							ICompAbilityEffectOnJumpCompleted compAbilityEffectOnJumpCompleted = abilityComp as ICompAbilityEffectOnJumpCompleted;
							if (compAbilityEffectOnJumpCompleted != null)
							{
								compAbilityEffectOnJumpCompleted.OnJumpCompleted(this.startVec.ToIntVec3(), this.target);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B2AC RID: 45740 RVA: 0x003EE6E0 File Offset: 0x003EC8E0
		private void LandingEffects()
		{
			SoundDef soundDef = this.soundLanding;
			if (soundDef != null)
			{
				soundDef.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			FleckMaker.ThrowDustPuff(this.DestinationPos + Gen.RandomHorizontalVector(0.5f), base.Map, 2f);
		}

		// Token: 0x0600B2AD RID: 45741 RVA: 0x003EE73C File Offset: 0x003EC93C
		protected override void TickInterval(int delta)
		{
			if (this.flightEffecter == null && this.flightEffecterDef != null)
			{
				this.flightEffecter = this.flightEffecterDef.Spawn();
				this.flightEffecter.Trigger(this, TargetInfo.Invalid, -1);
			}
			else
			{
				Effecter effecter = this.flightEffecter;
				if (effecter != null)
				{
					effecter.EffectTick(this, TargetInfo.Invalid);
				}
			}
			if (this.ticksFlying >= this.ticksFlightTime)
			{
				this.RespawnPawn();
				this.Destroy(DestroyMode.Vanish);
			}
			else if (this.IsHashIntervalTick(15, delta))
			{
				this.CheckDestination();
			}
			this.ticksFlying += delta;
		}

		// Token: 0x0600B2AE RID: 45742 RVA: 0x003EE7DC File Offset: 0x003EC9DC
		private void CheckDestination()
		{
			if (JumpUtility.ValidJumpTarget(this.FlyingThing, base.Map, this.destCell))
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(3.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 cell = this.destCell + GenRadial.RadialPattern[i];
				if (JumpUtility.ValidJumpTarget(this.FlyingThing, base.Map, cell))
				{
					this.destCell = cell;
					return;
				}
			}
		}

		// Token: 0x0600B2AF RID: 45743 RVA: 0x003EE850 File Offset: 0x003ECA50
		public void Notify_TransportedOnGravship(Gravship gravship)
		{
			IntVec3 b = gravship.Engine.Position - gravship.originalPosition;
			this.startVec += b.ToVector3();
			this.destCell += b;
			this.positionLastComputedTick = -1;
		}

		// Token: 0x0600B2B0 RID: 45744 RVA: 0x003EE8A8 File Offset: 0x003ECAA8
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			this.RecomputePosition();
			if (this.FlyingPawn != null)
			{
				this.FlyingPawn.DynamicDrawPhaseAt(phase, this.effectivePos, false);
			}
			else
			{
				Thing flyingThing = this.FlyingThing;
				if (flyingThing != null)
				{
					flyingThing.DynamicDrawPhaseAt(phase, this.effectivePos, false);
				}
			}
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		}

		// Token: 0x0600B2B1 RID: 45745 RVA: 0x003EE8FA File Offset: 0x003ECAFA
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawShadow(this.groundPos, this.effectiveHeight);
			if (this.CarriedThing != null && this.FlyingPawn != null)
			{
				PawnRenderUtility.DrawCarriedThing(this.FlyingPawn, this.effectivePos, this.CarriedThing);
			}
		}

		// Token: 0x0600B2B2 RID: 45746 RVA: 0x003EE938 File Offset: 0x003ECB38
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			Material shadowMaterial = this.def.pawnFlyer.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			float num = Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, shadowMaterial, 0);
		}

		// Token: 0x0600B2B3 RID: 45747 RVA: 0x003EE9A4 File Offset: 0x003ECBA4
		public static PawnFlyer MakeFlyer(ThingDef flyingDef, Pawn pawn, IntVec3 destCell, EffecterDef flightEffecterDef, SoundDef landingSound, bool flyWithCarriedThing = false, Vector3? overrideStartVec = null, Ability triggeringAbility = null, LocalTargetInfo target = default(LocalTargetInfo))
		{
			PawnFlyer pawnFlyer = (PawnFlyer)ThingMaker.MakeThing(flyingDef, null);
			pawnFlyer.startVec = (overrideStartVec ?? pawn.TrueCenter());
			pawnFlyer.Rotation = pawn.Rotation;
			pawnFlyer.flightDistance = pawn.Position.DistanceTo(destCell);
			pawnFlyer.destCell = destCell;
			pawnFlyer.pawnWasDrafted = pawn.Drafted;
			pawnFlyer.flightEffecterDef = flightEffecterDef;
			pawnFlyer.soundLanding = landingSound;
			pawnFlyer.triggeringAbility = ((triggeringAbility != null) ? triggeringAbility.def : null);
			pawnFlyer.target = target;
			if (pawn.drafter != null)
			{
				pawnFlyer.pawnCanFireAtWill = pawn.drafter.FireAtWill;
			}
			if (pawn.CurJob != null)
			{
				if (pawn.CurJob.def == JobDefOf.CastJump)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
				else
				{
					pawn.jobs.SuspendCurrentJob(JobCondition.InterruptForced, true, null);
				}
			}
			pawnFlyer.jobQueue = pawn.jobs.CaptureAndClearJobQueue();
			if (flyWithCarriedThing && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Direct, out pawnFlyer.carriedThing, null))
			{
				if (pawnFlyer.carriedThing.holdingOwner != null)
				{
					pawnFlyer.carriedThing.holdingOwner.Remove(pawnFlyer.carriedThing);
				}
				pawnFlyer.carriedThing.DeSpawn(DestroyMode.Vanish);
			}
			if (pawn.Spawned)
			{
				pawn.DeSpawn(DestroyMode.WillReplace);
			}
			if (!pawnFlyer.innerContainer.TryAdd(pawn, true))
			{
				Log.Error("Could not add " + pawn.ToStringSafe<Pawn>() + " to a flyer.");
				pawn.Destroy(DestroyMode.Vanish);
			}
			if (pawnFlyer.carriedThing != null && !pawnFlyer.innerContainer.TryAdd(pawnFlyer.carriedThing, true))
			{
				Log.Error("Could not add " + pawnFlyer.carriedThing.ToStringSafe<Thing>() + " to a flyer.");
			}
			return pawnFlyer;
		}

		// Token: 0x0600B2B4 RID: 45748 RVA: 0x003EEB80 File Offset: 0x003ECD80
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.startVec, "startVec", default(Vector3), false);
			Scribe_Values.Look<IntVec3>(ref this.destCell, "destCell", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.flightDistance, "flightDistance", 0f, false);
			Scribe_Values.Look<bool>(ref this.pawnWasDrafted, "pawnWasDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.pawnCanFireAtWill, "pawnCanFireAtWill", true, false);
			Scribe_Values.Look<int>(ref this.ticksFlightTime, "ticksFlightTime", 0, false);
			Scribe_Values.Look<int>(ref this.ticksFlying, "ticksFlying", 0, false);
			Scribe_Defs.Look<EffecterDef>(ref this.flightEffecterDef, "flightEffecterDef");
			Scribe_Defs.Look<SoundDef>(ref this.soundLanding, "soundLanding");
			Scribe_Defs.Look<AbilityDef>(ref this.triggeringAbility, "triggeringAbility");
			Scribe_References.Look<Thing>(ref this.carriedThing, "carriedThing", false);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<JobQueue>(ref this.jobQueue, "jobQueue", Array.Empty<object>());
			Scribe_TargetInfo.Look(ref this.target, "target");
		}

		// Token: 0x0400670C RID: 26380
		private ThingOwner<Thing> innerContainer;

		// Token: 0x0400670D RID: 26381
		protected Vector3 startVec;

		// Token: 0x0400670E RID: 26382
		private IntVec3 destCell;

		// Token: 0x0400670F RID: 26383
		private float flightDistance;

		// Token: 0x04006710 RID: 26384
		private bool pawnWasDrafted;

		// Token: 0x04006711 RID: 26385
		private bool pawnCanFireAtWill = true;

		// Token: 0x04006712 RID: 26386
		protected int ticksFlightTime = 120;

		// Token: 0x04006713 RID: 26387
		protected int ticksFlying;

		// Token: 0x04006714 RID: 26388
		private JobQueue jobQueue;

		// Token: 0x04006715 RID: 26389
		protected EffecterDef flightEffecterDef;

		// Token: 0x04006716 RID: 26390
		protected SoundDef soundLanding;

		// Token: 0x04006717 RID: 26391
		private Thing carriedThing;

		// Token: 0x04006718 RID: 26392
		private LocalTargetInfo target;

		// Token: 0x04006719 RID: 26393
		private AbilityDef triggeringAbility;

		// Token: 0x0400671A RID: 26394
		private Effecter flightEffecter;

		// Token: 0x0400671B RID: 26395
		private int positionLastComputedTick = -1;

		// Token: 0x0400671C RID: 26396
		private Vector3 groundPos;

		// Token: 0x0400671D RID: 26397
		private Vector3 effectivePos;

		// Token: 0x0400671E RID: 26398
		private float effectiveHeight;

		// Token: 0x0400671F RID: 26399
		private const int CheckDestinationInterval = 15;
	}
}
