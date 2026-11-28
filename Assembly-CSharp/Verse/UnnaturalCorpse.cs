using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020005A8 RID: 1448
	public class UnnaturalCorpse : Corpse
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x00109DA8 File Offset: 0x00107FA8
		public CompForbiddable Forbiddable
		{
			get
			{
				CompForbiddable result;
				if ((result = this.compForbidInt) == null)
				{
					result = (this.compForbidInt = base.GetComp<CompForbiddable>());
				}
				return result;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x00109DCE File Offset: 0x00107FCE
		public UnnaturalCorpseTracker Tracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x00109DD6 File Offset: 0x00107FD6
		public UnnaturalCorpse()
		{
			this.innerContainer.contentsLookMode = LookMode.Deep;
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x00109DEA File Offset: 0x00107FEA
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			UnnaturalCorpseTracker unnaturalCorpseTracker = this.tracker;
			if (unnaturalCorpseTracker == null)
			{
				return;
			}
			unnaturalCorpseTracker.Notify_CorpseDestroyed();
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x00109E03 File Offset: 0x00108003
		public override void PostMake()
		{
			base.PostMake();
			this.lastTeleport = GenTicks.TicksGame;
			this.Forbiddable.Forbidden = true;
			this.TryGetComp<CompRottable>().disabled = true;
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x00109E30 File Offset: 0x00108030
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.disappear)
			{
				this.DoDisappear();
				return;
			}
			if (base.Destroyed)
			{
				return;
			}
			if (base.ParentHolder != null && !(base.ParentHolder is Map))
			{
				return;
			}
			if (Rand.MTBEventOccurs(24f, 2500f, (float)delta))
			{
				this.teleport = true;
			}
			IntVec3 intVec;
			if (this.teleport && this.IsOutsideView() && AnomalyUtility.TryGetNearbyUnseenCell(this.tracker.Haunted, out intVec))
			{
				if (base.MapHeld != this.tracker.Haunted.MapHeld)
				{
					SkipUtility.SkipTo(this, intVec, this.tracker.Haunted.MapHeld);
				}
				else
				{
					base.Position = intVec;
				}
				this.teleport = false;
				this.lastTeleport = GenTicks.TicksGame;
				base.Map.listerHaulables.Notify_Unforbidden(this);
			}
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x00109F0C File Offset: 0x0010810C
		public override void TickRare()
		{
			base.TickRareInt();
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00109F14 File Offset: 0x00108114
		public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
			base.Notify_Studied(studier, amount, category);
			if (!category.IsAnomalyResearchCategory())
			{
				return;
			}
			this.tracker.Notify_Studied(studier, amount);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00109F38 File Offset: 0x00108138
		public void DoStudiedDeactivation(Pawn interactor)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map mapHeld = base.MapHeld;
			this.tracker.Notify_Finished();
			EffecterDefOf.MeatExplosion.Spawn(base.PositionHeld, base.MapHeld, 1f).Cleanup();
			this.Destroy(DestroyMode.Vanish);
			Thing t = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Shard, null), positionHeld, mapHeld, WipeMode.Vanish);
			TaggedString label = "UnnaturalCorpseDeactivatedLetter".Translate();
			TaggedString taggedString = "UnnaturalCorpseDeactivatedLetterDesc".Translate(interactor.Named("PAWN"));
			taggedString += "\n\n" + "UnnaturalCorpseDeactivatedLetterDescAppended".Translate(this.tracker.Haunted.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.PositiveEvent, t, null, null, null, null, 0, true);
			Hediff hediff;
			if (this.tracker.Haunted.health.hediffSet.TryGetHediff(HediffDefOf.CorpseTorment, out hediff))
			{
				this.tracker.Haunted.health.RemoveHediff(hediff);
			}
			Find.Anomaly.RemoveCorpseTracker(this.tracker.Haunted);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x0010A05C File Offset: 0x0010825C
		public void DoDisappear()
		{
			this.disappear = true;
			Thing spawnedParentOrMe = base.SpawnedParentOrMe;
			Building_Casket building_Casket = spawnedParentOrMe as Building_Casket;
			if (building_Casket != null)
			{
				this.SendDisappearedLetter();
				building_Casket.EjectContents();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (spawnedParentOrMe != this)
			{
				this.SendDisappearedLetter();
				Thing thing;
				base.ParentHolder.GetDirectlyHeldThings().TryDrop(this, ThingPlaceMode.Near, 1, out thing, null, null);
				SkipUtility.SkipDeSpawn(this);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.IsOutsideView())
			{
				this.SendDisappearedLetter();
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x0010A0DC File Offset: 0x001082DC
		private void SendDisappearedLetter()
		{
			TaggedString label = "LetterLabelCorpseDisappeared".Translate();
			TaggedString taggedString = "LetterCorpseDisappeared".Translate();
			if (!this.tracker.Haunted.DestroyedOrNull() && !this.tracker.Haunted.Dead)
			{
				taggedString += " " + "LetterCorpseDisappearedPawnAliveAppend".Translate(this.tracker.Haunted.Named("PAWN"));
			}
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x0010A167 File Offset: 0x00108367
		public void LinkToTracker(UnnaturalCorpseTracker newTracker)
		{
			if (this.tracker != null)
			{
				Log.ErrorOnce("mysterious corpse " + this.Label + " was linked to a new tracker when it still had one assigned.", 62521241);
			}
			this.tracker = newTracker;
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x0010A197 File Offset: 0x00108397
		public override IEnumerable<Gizmo> GetGizmos()
		{
			UnnaturalCorpse.<GetGizmos>d__20 <GetGizmos>d__ = new UnnaturalCorpse.<GetGizmos>d__20(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x0010A1A8 File Offset: 0x001083A8
		private bool IsOutsideView()
		{
			return (!base.SpawnedOrAnyParentSpawned || !base.MapHeld.reservationManager.IsReservedByAnyoneOf(this, Faction.OfPlayer)) && (Find.CurrentMap != base.MapHeld || !Find.CameraDriver.CurrentViewRect.ExpandedBy(1).Contains(base.PositionHeld));
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x0010A210 File Offset: 0x00108410
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnnaturalCorpseTracker>(ref this.tracker, "tracker", false);
			Scribe_Values.Look<bool>(ref this.disappear, "disappears", false, false);
			Scribe_Values.Look<bool>(ref this.teleport, "teleport", false, false);
			Scribe_Values.Look<int>(ref this.lastTeleport, "lastTeleport", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.tracker == null)
			{
				Log.Error("Failed to find corpse tracker, this should not happen. Destroying corpse.");
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04001D94 RID: 7572
		private UnnaturalCorpseTracker tracker;

		// Token: 0x04001D95 RID: 7573
		private bool disappear;

		// Token: 0x04001D96 RID: 7574
		private bool teleport;

		// Token: 0x04001D97 RID: 7575
		private int lastTeleport;

		// Token: 0x04001D98 RID: 7576
		private CompForbiddable compForbidInt;

		// Token: 0x04001D99 RID: 7577
		private const int MTBTeleportHours = 24;
	}
}
