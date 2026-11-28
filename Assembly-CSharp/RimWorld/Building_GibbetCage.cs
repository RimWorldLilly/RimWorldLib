using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AB4 RID: 6836
	public class Building_GibbetCage : Building_CorpseCasket, IObservedThoughtGiver, INotifyHauledTo
	{
		// Token: 0x17001EF8 RID: 7928
		// (get) Token: 0x0600AD37 RID: 44343 RVA: 0x003D1010 File Offset: 0x003CF210
		private float RandomCorpseRotation
		{
			get
			{
				return Rand.Range(-45f, 45f);
			}
		}

		// Token: 0x17001EF9 RID: 7929
		// (get) Token: 0x0600AD38 RID: 44344 RVA: 0x003D1021 File Offset: 0x003CF221
		public override bool ShouldTickContents
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600AD39 RID: 44345 RVA: 0x003D1024 File Offset: 0x003CF224
		public Building_GibbetCage()
		{
			if (this.corpseRotation == 0f)
			{
				this.corpseRotation = Rand.Range(-45f, 45f);
			}
		}

		// Token: 0x0600AD3A RID: 44346 RVA: 0x003D104E File Offset: 0x003CF24E
		public override bool Accepts(Thing thing)
		{
			return base.Accepts(thing) && !base.HasCorpse && this.storageSettings.AllowedToAccept(thing);
		}

		// Token: 0x0600AD3B RID: 44347 RVA: 0x003D1078 File Offset: 0x003CF278
		public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			Building_GibbetCage.tmpRadialPositions.Clear();
			this.corpseRotation = this.RandomCorpseRotation;
			if (base.Corpse.GetRotStage() != RotStage.Dessicated)
			{
				int num = GenRadial.NumCellsInRadius(1.5f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 item = base.Position + GenRadial.RadialPattern[i];
					Building_GibbetCage.tmpRadialPositions.Add(item);
				}
				int num2 = Mathf.Min(Building_GibbetCage.tmpRadialPositions.Count, Rand.Range(2, 4));
				for (int j = 0; j < num2; j++)
				{
					IntVec3 intVec = Building_GibbetCage.tmpRadialPositions.RandomElement<IntVec3>();
					Building_GibbetCage.tmpRadialPositions.Remove(intVec);
					FilthMaker.TryMakeFilth(intVec, base.Map, ThingDefOf.Filth_Blood, 1, FilthSourceFlags.None, true);
				}
			}
			if (this.def.building.gibbetCagePlaceCorpseEffecter != null)
			{
				this.def.building.gibbetCagePlaceCorpseEffecter.Spawn(this, base.Map, 1f);
			}
		}

		// Token: 0x0600AD3C RID: 44348 RVA: 0x003D116F File Offset: 0x003CF36F
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace && base.HasCorpse)
			{
				this.EjectContents();
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600AD3D RID: 44349 RVA: 0x003D118A File Offset: 0x003CF38A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.corpseRotation, "corpseRotation", 0f, false);
		}

		// Token: 0x0600AD3E RID: 44350 RVA: 0x003D11A8 File Offset: 0x003CF3A8
		public static Building_GibbetCage FindGibbetCageFor(Corpse c, Pawn traveler, bool ignoreOtherReservations = false)
		{
			if (Building_GibbetCage.cachedCages == null)
			{
				Building_GibbetCage.cachedCages = (from def in DefDatabase<ThingDef>.AllDefs
				where def.IsGibbetCage
				select def).ToList<ThingDef>();
			}
			Predicate<Thing> <>9__1;
			foreach (ThingDef singleDef in Building_GibbetCage.cachedCages)
			{
				IntVec3 positionHeld = c.PositionHeld;
				Map mapHeld = c.MapHeld;
				ThingRequest thingReq = ThingRequest.ForDef(singleDef);
				PathEndMode peMode = PathEndMode.InteractionCell;
				TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
				float maxDistance = 9999f;
				Predicate<Thing> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((Thing x) => !((Building_GibbetCage)x).HasCorpse && ((Building_GibbetCage)x).Accepts(c) && traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations)));
				}
				Building_GibbetCage building_GibbetCage = (Building_GibbetCage)GenClosest.ClosestThingReachable(positionHeld, mapHeld, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (building_GibbetCage != null)
				{
					return building_GibbetCage;
				}
			}
			return null;
		}

		// Token: 0x0600AD3F RID: 44351 RVA: 0x003D12BC File Offset: 0x003CF4BC
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if (observer.Ideo != null && observer.Ideo.IdeoApprovesOfSlavery())
			{
				return null;
			}
			Thought_MemoryObservation thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedGibbetCage);
			thought_MemoryObservation.Target = this;
			return thought_MemoryObservation;
		}

		// Token: 0x0600AD40 RID: 44352 RVA: 0x003D12EB File Offset: 0x003CF4EB
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}

		// Token: 0x0600AD41 RID: 44353 RVA: 0x003D12F0 File Offset: 0x003CF4F0
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			if (base.HasCorpse)
			{
				Vector3 drawLoc2 = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop) + this.def.building.gibbetCorposeDrawOffset;
				base.Corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(this.corpseRotation);
				base.Corpse.DynamicDrawPhaseAt(phase, drawLoc2, false);
			}
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		}

		// Token: 0x0600AD42 RID: 44354 RVA: 0x003D1368 File Offset: 0x003CF568
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.cageTopGraphic == null)
			{
				this.cageTopGraphic = this.def.building.gibbetCageTopGraphicData.GraphicColoredFor(this);
			}
			this.cageTopGraphic.Draw(base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Item), Rot4.North, this, 0f);
		}

		// Token: 0x0600AD43 RID: 44355 RVA: 0x003D13C7 File Offset: 0x003CF5C7
		public override void Notify_ColorChanged()
		{
			this.cageTopGraphic = null;
			base.Notify_ColorChanged();
		}

		// Token: 0x04006473 RID: 25715
		private static List<ThingDef> cachedCages;

		// Token: 0x04006474 RID: 25716
		[Unsaved(false)]
		private Graphic cageTopGraphic;

		// Token: 0x04006475 RID: 25717
		private float corpseRotation;

		// Token: 0x04006476 RID: 25718
		private static readonly List<IntVec3> tmpRadialPositions = new List<IntVec3>();
	}
}
