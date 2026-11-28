using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001861 RID: 6241
	public class CompDevourer : ThingComp, IThingHolder
	{
		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x06009C23 RID: 39971 RVA: 0x00383983 File Offset: 0x00381B83
		public CompProperties_Devourer Props
		{
			get
			{
				return (CompProperties_Devourer)this.props;
			}
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x06009C24 RID: 39972 RVA: 0x00383990 File Offset: 0x00381B90
		public Thing DigestingThing
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

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x06009C25 RID: 39973 RVA: 0x003839B8 File Offset: 0x00381BB8
		public Pawn DigestingPawn
		{
			get
			{
				Thing digestingThing = this.DigestingThing;
				if (digestingThing == null)
				{
					return null;
				}
				Corpse corpse = digestingThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return digestingThing as Pawn;
			}
		}

		// Token: 0x17001AAF RID: 6831
		// (get) Token: 0x06009C26 RID: 39974 RVA: 0x003839E8 File Offset: 0x00381BE8
		public bool Digesting
		{
			get
			{
				return this.DigestingThing != null;
			}
		}

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x06009C27 RID: 39975 RVA: 0x003839F3 File Offset: 0x00381BF3
		public Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x06009C28 RID: 39976 RVA: 0x00383A00 File Offset: 0x00381C00
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06009C29 RID: 39977 RVA: 0x00383A0E File Offset: 0x00381C0E
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06009C2A RID: 39978 RVA: 0x00383A16 File Offset: 0x00381C16
		public CompDevourer()
		{
			this.innerContainer = new ThingOwner<Thing>(this, LookMode.Deep, false);
		}

		// Token: 0x06009C2B RID: 39979 RVA: 0x00383A2C File Offset: 0x00381C2C
		public override void CompTick()
		{
			if (this.Digesting)
			{
				this.ticksDigesting++;
			}
			if (this.Digesting && this.DigestingPawn.Dead)
			{
				this.CompleteDigestion();
			}
		}

		// Token: 0x06009C2C RID: 39980 RVA: 0x00383A60 File Offset: 0x00381C60
		public override string CompInspectStringExtra()
		{
			if (!this.Digesting)
			{
				return null;
			}
			int digestionTicks = this.GetDigestionTicks();
			int ticksLeftThisToil = this.Pawn.jobs.curDriver.ticksLeftThisToil;
			int num = (ticksLeftThisToil < 0) ? digestionTicks : ticksLeftThisToil;
			float num2 = (float)(digestionTicks - (digestionTicks - num)) / 60f;
			return this.Props.digestingInspector.Formatted(this.DigestingThing.Named("PAWN"), num2.Named("SECONDS"));
		}

		// Token: 0x06009C2D RID: 39981 RVA: 0x00383ADF File Offset: 0x00381CDF
		public override void Notify_Downed()
		{
			this.AbortDigestion(this.Pawn.MapHeld);
		}

		// Token: 0x06009C2E RID: 39982 RVA: 0x00383AF2 File Offset: 0x00381CF2
		public override void Notify_Killed(Map prevMap, DamageInfo? _ = null)
		{
			this.AbortDigestion(prevMap);
		}

		// Token: 0x06009C2F RID: 39983 RVA: 0x00383AFB File Offset: 0x00381CFB
		public void DigestJobFinished()
		{
			if (this.Pawn.BeingTransportedOnGravship)
			{
				return;
			}
			if (this.ticksDigesting >= this.ticksToDigestFully)
			{
				this.CompleteDigestion();
				return;
			}
			this.AbortDigestion(this.Pawn.MapHeld);
		}

		// Token: 0x06009C30 RID: 39984 RVA: 0x00383B34 File Offset: 0x00381D34
		public override void PostSwapMap()
		{
			if (this.Digesting)
			{
				this.Pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.DevourerDigest), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
		}

		// Token: 0x06009C31 RID: 39985 RVA: 0x00383B80 File Offset: 0x00381D80
		private void AbortDigestion(Map map)
		{
			if (!this.Digesting)
			{
				return;
			}
			Pawn pawn = this.DropPawn(map);
			Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_DevourerDigestionAborted, this.Pawn));
			float amount = this.Props.timeDamageCurve.Evaluate((float)this.ticksDigesting / 60f);
			DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, amount, 0f, -1f, this.Pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
			dinfo.SetApplyAllDamage(true);
			pawn.TakeDamage(dinfo);
			if (pawn.Faction == Faction.OfPlayer)
			{
				string text = this.Pawn.Dead ? this.Props.messageEmergedCorpse : this.Props.messageEmerged;
				if (!text.NullOrEmpty())
				{
					text = text.Formatted(pawn.Named("PAWN"));
					Messages.Message(text, pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			}
			this.EndDigestingJob();
			this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
			if (this.Pawn.Drawer.renderer.CurAnimation == AnimationDefOf.DevourerDigesting)
			{
				this.Pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		// Token: 0x06009C32 RID: 39986 RVA: 0x00383CC0 File Offset: 0x00381EC0
		private void CompleteDigestion()
		{
			if (!this.Digesting)
			{
				return;
			}
			Pawn pawn = this.DropPawn(this.Pawn.MapHeld);
			Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_DevourerDigestionCompleted, this.Pawn));
			DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, (float)this.Props.completeDigestionDamage, 0f, -1f, this.Pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
			dinfo.SetApplyAllDamage(true);
			pawn.TakeDamage(dinfo);
			if (!this.Props.messageDigestionCompleted.NullOrEmpty() && !pawn.Dead && pawn.Faction == Faction.OfPlayer)
			{
				Messages.Message(this.Props.messageDigestionCompleted.Formatted(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NegativeEvent, true);
			}
			this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
			if (this.Pawn.Drawer.renderer.CurAnimation == AnimationDefOf.DevourerDigesting)
			{
				this.Pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		// Token: 0x06009C33 RID: 39987 RVA: 0x00383DE4 File Offset: 0x00381FE4
		public void StartDigesting(IntVec3 origin, LocalTargetInfo target)
		{
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null && pawn.Spawned)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.AcidBurn, 99f, 0f, -1f, this.parent, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnDamaged(pawn, dinfo);
					}
					if (pawn.drafter != null)
					{
						this.wasDrafted = pawn.drafter.Drafted;
					}
					pawn.DeSpawn(DestroyMode.Vanish);
					this.ticksDigesting = 0;
					this.innerContainer.TryAdd(pawn, true);
					this.ticksToDigestFully = this.GetDigestionTicks() - 30;
					this.Pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.DevourerDigest), JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					if (!this.Props.messageDigested.NullOrEmpty() && pawn.Faction == Faction.OfPlayer)
					{
						Messages.Message(this.Props.messageDigested.Formatted(pawn.Named("PAWN")), this.Pawn, MessageTypeDefOf.NegativeEvent, true);
					}
					this.Pawn.Rotation = Rot4.FromAngleFlat((this.parent.Position - origin).AngleFlat);
					this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
					if (this.Pawn.Drawer.renderer.CurAnimation != AnimationDefOf.DevourerDigesting)
					{
						this.Pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DevourerDigesting);
					}
					Find.BattleLog.Add(new BattleLogEntry_Event(pawn, RulePackDefOf.Event_DevourerConsumeLeap, this.Pawn));
					return;
				}
			}
			this.Pawn.abilities.GetAbility(AbilityDefOf.ConsumeLeap_Devourer, false).ResetCooldown();
		}

		// Token: 0x06009C34 RID: 39988 RVA: 0x00383FCC File Offset: 0x003821CC
		private Pawn DropPawn(Map map)
		{
			if (!this.Digesting)
			{
				return null;
			}
			Thing thing;
			if (!this.innerContainer.TryDrop(this.DigestingThing, this.Pawn.PositionHeld, map, ThingPlaceMode.Near, out thing, null, null))
			{
				IntVec3 loc;
				if (!RCellFinder.TryFindRandomCellNearWith(this.Pawn.PositionHeld, (IntVec3 c) => c.Standable(map), map, out loc, 1, 2147483647))
				{
					Debug.LogError("Could not drop digesting pawn from devourer!");
					return null;
				}
				thing = GenSpawn.Spawn(this.innerContainer.Take(this.DigestingThing), loc, map, WipeMode.Vanish);
			}
			Corpse corpse = thing as Corpse;
			if (corpse != null)
			{
				return corpse.InnerPawn;
			}
			Pawn pawn = (Pawn)thing;
			pawn.stances.stunner.StunFor(60, this.Pawn, false, false, false);
			if (pawn.drafter != null)
			{
				pawn.drafter.Drafted = this.wasDrafted;
			}
			return pawn;
		}

		// Token: 0x06009C35 RID: 39989 RVA: 0x003840C0 File Offset: 0x003822C0
		public int GetDigestionTicks()
		{
			if (this.DigestingThing == null)
			{
				return 0;
			}
			return Mathf.CeilToInt(this.Props.bodySizeDigestTimeCurve.Evaluate(this.DigestingPawn.BodySize) * 60f);
		}

		// Token: 0x06009C36 RID: 39990 RVA: 0x003840F4 File Offset: 0x003822F4
		private void EndDigestingJob()
		{
			if (!this.Pawn.Dead && this.Pawn.CurJobDef == JobDefOf.DevourerDigest && this.Pawn.jobs.curDriver != null && !this.Pawn.jobs.curDriver.ended)
			{
				this.Pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06009C37 RID: 39991 RVA: 0x00384160 File Offset: 0x00382360
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksDigesting, "ticksDigesting", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToDigestFully, "ticksToDigestFully", 0, false);
			Scribe_Values.Look<bool>(ref this.wasDrafted, "wasDrafted", false, false);
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.innerContainer.removeContentsIfDestroyed)
			{
				this.innerContainer.removeContentsIfDestroyed = false;
			}
		}

		// Token: 0x04005CD0 RID: 23760
		private ThingOwner<Thing> innerContainer;

		// Token: 0x04005CD1 RID: 23761
		private int ticksDigesting;

		// Token: 0x04005CD2 RID: 23762
		private int ticksToDigestFully;

		// Token: 0x04005CD3 RID: 23763
		private bool wasDrafted;
	}
}
