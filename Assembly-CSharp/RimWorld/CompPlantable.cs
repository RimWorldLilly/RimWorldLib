using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200192F RID: 6447
	[StaticConstructorOnStartup]
	public class CompPlantable : ThingComp
	{
		// Token: 0x17001BF2 RID: 7154
		// (get) Token: 0x0600A1B6 RID: 41398 RVA: 0x0039CE14 File Offset: 0x0039B014
		private static TargetingParameters TargetingParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetPawns = false,
					canTargetLocations = true
				};
			}
		}

		// Token: 0x17001BF3 RID: 7155
		// (get) Token: 0x0600A1B7 RID: 41399 RVA: 0x0039CE29 File Offset: 0x0039B029
		public CompProperties_Plantable Props
		{
			get
			{
				return (CompProperties_Plantable)this.props;
			}
		}

		// Token: 0x17001BF4 RID: 7156
		// (get) Token: 0x0600A1B8 RID: 41400 RVA: 0x0039CE36 File Offset: 0x0039B036
		public List<IntVec3> PlantCells
		{
			get
			{
				return this.plantCells;
			}
		}

		// Token: 0x0600A1B9 RID: 41401 RVA: 0x0039CE3E File Offset: 0x0039B03E
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPlantable.<CompGetGizmosExtra>d__8 <CompGetGizmosExtra>d__ = new CompPlantable.<CompGetGizmosExtra>d__8(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A1BA RID: 41402 RVA: 0x0039CE50 File Offset: 0x0039B050
		private void BeginTargeting()
		{
			Find.Targeter.BeginTargeting(CompPlantable.TargetingParams, delegate(LocalTargetInfo t)
			{
				if (!this.ValidateTarget(t))
				{
					this.BeginTargeting();
					return;
				}
				List<Thing> list;
				if (this.ConnectionStrengthReducedByNearbyBuilding(t.Cell, out list))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("TreeBlockedByNearbyBuildingsDesc".Translate(this.Props.plantDefToSpawn.Named("PLANT")), delegate()
					{
						this.plantCells.Add(t.Cell);
					}, false, null, WindowLayer.Dialog));
					return;
				}
				this.plantCells.Add(t.Cell);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}, delegate(LocalTargetInfo t)
			{
				if (this.CanPlantAt(t.Cell, this.parent.MapHeld).Accepted)
				{
					GenDraw.DrawTargetHighlight(t);
					this.DrawSurroundingsInfo(t.Cell);
				}
			}, (LocalTargetInfo t) => true, null, null, this.Props.plantDefToSpawn.uiIcon, false, null, null);
		}

		// Token: 0x0600A1BB RID: 41403 RVA: 0x0039CEB8 File Offset: 0x0039B0B8
		private bool ConnectionStrengthReducedByNearbyBuilding(IntVec3 cell, out List<Thing> buildings)
		{
			CompProperties_TreeConnection compProperties = this.Props.plantDefToSpawn.GetCompProperties<CompProperties_TreeConnection>();
			if (compProperties != null)
			{
				buildings = this.parent.MapHeld.listerArtificialBuildingsForMeditation.GetForCell(cell, compProperties.radiusToBuildingForConnectionStrengthLoss);
				if (buildings.Any<Thing>())
				{
					return true;
				}
			}
			buildings = null;
			return false;
		}

		// Token: 0x0600A1BC RID: 41404 RVA: 0x0039CF08 File Offset: 0x0039B108
		private void DrawSurroundingsInfo(IntVec3 cell)
		{
			CompProperties_TreeConnection compProperties = this.Props.plantDefToSpawn.GetCompProperties<CompProperties_TreeConnection>();
			if (compProperties != null)
			{
				Color color = Color.white;
				List<Thing> list;
				if (this.ConnectionStrengthReducedByNearbyBuilding(cell, out list))
				{
					color = Color.red;
					foreach (Thing t in list)
					{
						GenDraw.DrawLineBetween(cell.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), t.TrueCenter(), SimpleColor.Red, 0.2f);
					}
				}
				color.a = 0.5f;
				GenDraw.DrawRadiusRing(cell, compProperties.radiusToBuildingForConnectionStrengthLoss, color, null);
			}
			for (int i = 0; i < this.Props.plantDefToSpawn.comps.Count; i++)
			{
				IPlantEffectRadius plantEffectRadius = this.Props.plantDefToSpawn.comps[i] as IPlantEffectRadius;
				if (plantEffectRadius != null)
				{
					Color white = Color.white;
					white.a = 0.5f;
					GenDraw.DrawRadiusRing(cell, plantEffectRadius.PlantEffectRadius, white, null);
					return;
				}
			}
		}

		// Token: 0x0600A1BD RID: 41405 RVA: 0x0039D01C File Offset: 0x0039B21C
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.plantCells.Count; i++)
			{
				if (this.plantCells[i].IsValid)
				{
					IntVec3 intVec = this.plantCells[i];
					GenDraw.DrawLineBetween(this.parent.PositionHeld.ToVector3Shifted(), intVec.ToVector3Shifted(), AltitudeLayer.Blueprint.AltitudeFor());
					GhostDrawer.DrawGhostThing(intVec, Rot4.North, this.Props.plantDefToSpawn, this.Props.plantDefToSpawn.graphic, Color.white, AltitudeLayer.Blueprint, null, true, null);
					this.DrawSurroundingsInfo(intVec);
				}
			}
		}

		// Token: 0x0600A1BE RID: 41406 RVA: 0x0039D0C8 File Offset: 0x0039B2C8
		public void DoPlant(Pawn planter, IntVec3 cell, Map map)
		{
			Plant plant = (Plant)ThingMaker.MakeThing(this.Props.plantDefToSpawn, null);
			if (GenPlace.TryPlaceThing(plant, cell, map, ThingPlaceMode.Direct, null, null, null, 1))
			{
				planter.records.Increment(RecordDefOf.PlantsSown);
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, planter.Named(HistoryEventArgsNames.Doer)), true);
				if (this.Props.plantDefToSpawn.plant.humanFoodPlant)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, planter.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (ModsConfig.IdeologyActive && plant.def == ThingDefOf.Plant_TreeGauranlen)
				{
					EffecterDefOf.GauranlenLeavesBatch.Spawn(cell, map, 1f).Cleanup();
				}
				plant.Growth = 0.0001f;
				plant.sown = true;
				map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Things);
				this.parent.Destroy(DestroyMode.Vanish);
				if (ModsConfig.IdeologyActive && plant.def == ThingDefOf.Plant_TreeGauranlen)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PlantedGauranlenTree, planter.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
		}

		// Token: 0x0600A1BF RID: 41407 RVA: 0x0039D1FF File Offset: 0x0039B3FF
		public void Notify_SeedRemovedFromStackForPlantingAt(IntVec3 cell)
		{
			if (this.plantCells.Contains(cell))
			{
				this.plantCells.Remove(cell);
			}
		}

		// Token: 0x0600A1C0 RID: 41408 RVA: 0x0039D21C File Offset: 0x0039B41C
		public void Notify_RemovedFromStackForPlantingAt(IntVec3 newPlantCell)
		{
			this.plantCells.Clear();
			if (newPlantCell.IsValid)
			{
				this.plantCells.Add(newPlantCell);
			}
		}

		// Token: 0x0600A1C1 RID: 41409 RVA: 0x0039D240 File Offset: 0x0039B440
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			CompPlantable compPlantable = otherStack.TryGetComp<CompPlantable>();
			if (compPlantable == null)
			{
				return;
			}
			List<IntVec3> list = compPlantable.PlantCells;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsValid && !this.plantCells.Contains(list[i]))
				{
					this.plantCells.Add(list[i]);
				}
			}
		}

		// Token: 0x0600A1C2 RID: 41410 RVA: 0x0039D2A8 File Offset: 0x0039B4A8
		public override void PostSplitOff(Thing piece)
		{
			CompPlantable compPlantable = piece.TryGetComp<CompPlantable>();
			if (compPlantable == null)
			{
				return;
			}
			List<IntVec3> list = this.plantCells;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsValid && !compPlantable.plantCells.Contains(list[i]))
				{
					compPlantable.plantCells.Add(list[i]);
				}
			}
		}

		// Token: 0x0600A1C3 RID: 41411 RVA: 0x0039D310 File Offset: 0x0039B510
		public AcceptanceReport CanPlantAt(IntVec3 cell, Map map)
		{
			if (!cell.IsValid || cell.Fogged(map))
			{
				return false;
			}
			Thing thing;
			AcceptanceReport acceptanceReport = this.Props.plantDefToSpawn.CanEverPlantAt(cell, map, out thing, true, false, false);
			if (!acceptanceReport.Accepted)
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + acceptanceReport.Reason.CapitalizeFirst();
			}
			TerrainDef terrain = cell.GetTerrain(map);
			if (terrain.IsFloor && terrain.fertility <= 0f)
			{
				return "CannotPlantMissingTerrainTag".Translate();
			}
			if (this.Props.plantDefToSpawn.plant.interferesWithRoof && cell.Roofed(map))
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + "BlockedByRoof".Translate().CapitalizeFirst();
			}
			Thing thing2 = PlantUtility.AdjacentSowBlocker(this.Props.plantDefToSpawn, cell, map);
			if (thing2 != null)
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + "AdjacentSowBlocker".Translate(thing2);
			}
			if (this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant > 0f)
			{
				foreach (Thing thing3 in map.listerThings.ThingsOfDef(this.Props.plantDefToSpawn))
				{
					if (thing3.Position.InHorDistOf(cell, this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant))
					{
						return "CannotPlantThing".Translate(this.parent) + ": " + "TooCloseToOtherPlant".Translate(thing3);
					}
				}
				foreach (Thing thing4 in map.listerThings.ThingsOfDef(this.parent.def))
				{
					CompPlantable compPlantable = thing4.TryGetComp<CompPlantable>();
					if (compPlantable != null && !compPlantable.PlantCells.NullOrEmpty<IntVec3>())
					{
						for (int i = 0; i < compPlantable.PlantCells.Count; i++)
						{
							if (compPlantable.PlantCells[i].InHorDistOf(cell, this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant))
							{
								return "CannotPlantThing".Translate(this.parent) + ": " + "TooCloseToOtherSeedPlantCell".Translate(thing4.GetCustomLabelNoCount(false));
							}
						}
					}
				}
			}
			List<Thing> list = map.thingGrid.ThingsListAt(cell);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j] is Building_PlantGrower)
				{
					return "CannotPlantThing".Translate(this.parent) + ": " + "BlockedBy".Translate(list[j]).CapitalizeFirst();
				}
			}
			return true;
		}

		// Token: 0x0600A1C4 RID: 41412 RVA: 0x0039D6CC File Offset: 0x0039B8CC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.plantCells, "plantCells", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.plantCells == null)
			{
				this.plantCells = new List<IntVec3>();
			}
		}

		// Token: 0x0600A1C5 RID: 41413 RVA: 0x0039D708 File Offset: 0x0039B908
		public bool ValidateTarget(LocalTargetInfo target)
		{
			AcceptanceReport acceptanceReport = this.CanPlantAt(target.Cell, this.parent.MapHeld);
			if (!acceptanceReport.Accepted)
			{
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					Messages.Message(acceptanceReport.Reason, this.parent, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04005F90 RID: 24464
		private List<IntVec3> plantCells = new List<IntVec3>();

		// Token: 0x04005F91 RID: 24465
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
