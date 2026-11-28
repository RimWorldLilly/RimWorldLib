using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C6C RID: 7276
	public abstract class Designator_Place : DesignatorWithEyedropper
	{
		// Token: 0x1700219F RID: 8607
		// (get) Token: 0x0600B9C0 RID: 47552
		public abstract BuildableDef PlacingDef { get; }

		// Token: 0x170021A0 RID: 8608
		// (get) Token: 0x0600B9C1 RID: 47553
		public abstract ThingStyleDef ThingStyleDefForPreview { get; }

		// Token: 0x170021A1 RID: 8609
		// (get) Token: 0x0600B9C2 RID: 47554
		public abstract ThingDef StuffDef { get; }

		// Token: 0x170021A2 RID: 8610
		// (get) Token: 0x0600B9C3 RID: 47555 RVA: 0x004186DD File Offset: 0x004168DD
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return this.PlacingDef.drawStyleCategory;
			}
		}

		// Token: 0x0600B9C4 RID: 47556 RVA: 0x004186EA File Offset: 0x004168EA
		public Designator_Place()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragBuilding;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_PlaceBuilding;
		}

		// Token: 0x0600B9C5 RID: 47557 RVA: 0x0041871C File Offset: 0x0041691C
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			Map currentMap = Find.CurrentMap;
			currentMap.deepResourceGrid.DrawPlacingMouseAttachments(this.PlacingDef);
			Vector2 vector = Event.current.mousePosition + Designator_Place.PlaceMouseAttachmentDrawOffset;
			float x = vector.x;
			float y = vector.y;
			this.DrawPlaceMouseAttachments(x, ref y);
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawPlaceMouseAttachments(x, ref y, this.PlacingDef, UI.MouseCell(), this.placingRot);
				}
				foreach (PlaceWorker placeWorker2 in this.PlacingDef.PlaceWorkers)
				{
					placeWorker2.DrawMouseAttachments(this.PlacingDef);
				}
			}
			if (currentMap != null)
			{
				ThingDef thingDef = this.PlacingDef as ThingDef;
				if (thingDef != null && thingDef.displayNumbersBetweenSameDefDistRange.max > 0f)
				{
					IntVec3 intVec = UI.MouseCell();
					Designator_Place.tmpThings.Clear();
					Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsOfDef(thingDef));
					Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint));
					foreach (Thing thing in Designator_Place.tmpThings)
					{
						if ((thing.def == thingDef || thing.def.entityDefToBuild == thingDef) && (thing.Position.x == intVec.x || thing.Position.z == intVec.z) && this.CanDrawNumbersBetween(thing, thingDef, intVec, thing.Position, currentMap))
						{
							IntVec3 intVec2 = thing.Position - intVec;
							intVec2.x = Mathf.Abs(intVec2.x) + 1;
							intVec2.z = Mathf.Abs(intVec2.z) + 1;
							if (intVec2.x >= 3)
							{
								Vector2 screenPos = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
								screenPos.y = thing.Position.ToUIPosition().y;
								Color textColor = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.x) ? Color.white : Color.red;
								Widgets.DrawNumberOnMap(screenPos, intVec2.x, textColor);
							}
							if (intVec2.z >= 3)
							{
								Vector2 screenPos2 = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
								screenPos2.x = thing.Position.ToUIPosition().x;
								Color textColor2 = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.z) ? Color.white : Color.red;
								Widgets.DrawNumberOnMap(screenPos2, intVec2.z, textColor2);
							}
						}
					}
					Designator_Place.tmpThings.Clear();
					return;
				}
			}
		}

		// Token: 0x0600B9C6 RID: 47558 RVA: 0x00418A98 File Offset: 0x00416C98
		protected virtual void DrawPlaceMouseAttachments(float curX, ref float curY)
		{
		}

		// Token: 0x0600B9C7 RID: 47559 RVA: 0x00418A9A File Offset: 0x00416C9A
		protected virtual bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return !GenThing.CloserThingBetween(def, a, b, map, null);
		}

		// Token: 0x0600B9C8 RID: 47560 RVA: 0x00418AAC File Offset: 0x00416CAC
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawOnGUIExtra(this.PlacingDef);
				}
			}
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.placingRot, delegate(Rot4 rot)
				{
					this.placingRot = rot;
				});
				return;
			}
			base.DoExtraGuiControls(leftX, bottomY);
		}

		// Token: 0x0600B9C9 RID: 47561 RVA: 0x00418B50 File Offset: 0x00416D50
		public override void SelectedProcessInput(Event ev)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				this.HandleRotationShortcuts();
				return;
			}
			base.SelectedProcessInput(ev);
		}

		// Token: 0x0600B9CA RID: 47562 RVA: 0x00418B84 File Offset: 0x00416D84
		public override void SelectedUpdate()
		{
			if (!base.Map.IsPocketMap)
			{
				GenDraw.DrawNoBuildEdgeLines();
			}
			IntVec3 intVec = UI.MouseCell();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted) && intVec.InBounds(base.Map))
			{
				if (this.PlacingDef is TerrainDef)
				{
					GenUI.RenderMouseoverBracket();
					this.DrawPlaceWorkers();
					return;
				}
				this.DrawBeforeGhost();
				Color ghostCol;
				if (this.CanDesignateCell(intVec).Accepted)
				{
					ghostCol = Designator_Place.CanPlaceColor;
				}
				else
				{
					ghostCol = Designator_Place.CannotPlaceColor;
				}
				this.DrawGhost(ghostCol);
				if (this.CanDesignateCell(intVec).Accepted && this.PlacingDef.specialDisplayRadius > 0.01f)
				{
					GenDraw.DrawRadiusRing(intVec, this.PlacingDef.specialDisplayRadius);
				}
				GenDraw.DrawInteractionCells((ThingDef)this.PlacingDef, intVec, this.placingRot);
				ThingDef thingDef = this.PlacingDef as ThingDef;
				if (thingDef != null && thingDef.building != null && thingDef.building.isAttachment && GenConstruct.GetWallAttachedTo(intVec, this.placingRot, Find.CurrentMap) == null && this.HasPotentialAttachment(intVec))
				{
					this.HandleRotation(RotationDirection.Clockwise);
				}
			}
		}

		// Token: 0x0600B9CB RID: 47563 RVA: 0x00418CAC File Offset: 0x00416EAC
		protected virtual void DrawBeforeGhost()
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				GauranlenUtility.DrawConnectionsAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				PsychicRitualUtility.DrawPsychicRitualSpotsAffectedByThingOverlay(base.Map, thingDef, UI.MouseCell(), this.placingRot);
			}
		}

		// Token: 0x0600B9CC RID: 47564 RVA: 0x00418D18 File Offset: 0x00416F18
		protected virtual void DrawGhost(Color ghostCol)
		{
			IntVec3 center = UI.MouseCell();
			Rot4 rot = this.placingRot;
			ThingDef thingDef = (ThingDef)this.PlacingDef;
			ThingStyleDef thingStyleDefForPreview = this.ThingStyleDefForPreview;
			GhostDrawer.DrawGhostThing(center, rot, thingDef, (thingStyleDefForPreview != null) ? thingStyleDefForPreview.Graphic : null, ghostCol, AltitudeLayer.Blueprint, null, true, this.StuffDef);
		}

		// Token: 0x0600B9CD RID: 47565 RVA: 0x00418D60 File Offset: 0x00416F60
		protected virtual void DrawPlaceWorkers()
		{
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawGhost(null, UI.MouseCell(), this.placingRot, Designator_Place.CanPlaceColor, null);
				}
			}
		}

		// Token: 0x0600B9CE RID: 47566 RVA: 0x00418DD4 File Offset: 0x00416FD4
		private void HandleRotationShortcuts()
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.mouseDown)
				{
					Event.current.Use();
					Designator_Place.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.mouseUp && Time.realtimeSinceStartup - Designator_Place.middleMouseDownTime < 0.15f)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection != RotationDirection.None)
			{
				this.HandleRotation(rotationDirection);
			}
		}

		// Token: 0x0600B9CF RID: 47567 RVA: 0x00418E58 File Offset: 0x00417058
		private void HandleRotation(RotationDirection dir)
		{
			IntVec3 intVec = UI.MouseCell();
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.building.isAttachment && this.HasPotentialAttachment(intVec))
			{
				this.placingRot.Rotate(dir);
				while (GenConstruct.GetWallAttachedTo(intVec, this.placingRot, Find.CurrentMap) == null)
				{
					this.placingRot.Rotate(dir);
				}
				return;
			}
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.placingRot.Rotate(dir);
		}

		// Token: 0x0600B9D0 RID: 47568 RVA: 0x00418ED8 File Offset: 0x004170D8
		private bool HasPotentialAttachment(IntVec3 cell)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef == null || !thingDef.building.isAttachment)
			{
				return false;
			}
			foreach (Rot4 rot in Rot4.AllRotations)
			{
				if (GenConstruct.GetWallAttachedTo(cell, rot, Find.CurrentMap) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B9D1 RID: 47569 RVA: 0x00418F50 File Offset: 0x00417150
		public override void Selected()
		{
			this.placingRot = this.PlacingDef.defaultPlacingRot;
		}

		// Token: 0x040069D3 RID: 27091
		protected Rot4 placingRot = Rot4.North;

		// Token: 0x040069D4 RID: 27092
		protected static float middleMouseDownTime;

		// Token: 0x040069D5 RID: 27093
		public static readonly Color CanPlaceColor = new Color(0.5f, 1f, 0.6f, 0.4f);

		// Token: 0x040069D6 RID: 27094
		public static readonly Color CannotPlaceColor = new Color(1f, 0f, 0f, 0.4f);

		// Token: 0x040069D7 RID: 27095
		public static readonly Vector2 PlaceMouseAttachmentDrawOffset = new Vector2(19f, 17f);

		// Token: 0x040069D8 RID: 27096
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
