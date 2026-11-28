using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C6B RID: 7275
	public class Designator_Install : Designator_Place
	{
		// Token: 0x17002195 RID: 8597
		// (get) Token: 0x0600B9AE RID: 47534 RVA: 0x004182F0 File Offset: 0x004164F0
		protected Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing is MinifiedThing)
				{
					return singleSelectedThing;
				}
				Building building = singleSelectedThing as Building;
				if (building != null && building.def.Minifiable)
				{
					return singleSelectedThing;
				}
				return null;
			}
		}

		// Token: 0x17002196 RID: 8598
		// (get) Token: 0x0600B9AF RID: 47535 RVA: 0x0041832C File Offset: 0x0041652C
		protected Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17002197 RID: 8599
		// (get) Token: 0x0600B9B0 RID: 47536 RVA: 0x00418339 File Offset: 0x00416539
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002198 RID: 8600
		// (get) Token: 0x0600B9B1 RID: 47537 RVA: 0x0041833C File Offset: 0x0041653C
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		// Token: 0x17002199 RID: 8601
		// (get) Token: 0x0600B9B2 RID: 47538 RVA: 0x00418349 File Offset: 0x00416549
		public override string Label
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstall".Translate();
				}
				return "CommandReinstall".Translate();
			}
		}

		// Token: 0x1700219A RID: 8602
		// (get) Token: 0x0600B9B3 RID: 47539 RVA: 0x00418377 File Offset: 0x00416577
		public override string Desc
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstallDesc".Translate();
				}
				return "CommandReinstallDesc".Translate();
			}
		}

		// Token: 0x1700219B RID: 8603
		// (get) Token: 0x0600B9B4 RID: 47540 RVA: 0x004183A5 File Offset: 0x004165A5
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x1700219C RID: 8604
		// (get) Token: 0x0600B9B5 RID: 47541 RVA: 0x004183AC File Offset: 0x004165AC
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		// Token: 0x1700219D RID: 8605
		// (get) Token: 0x0600B9B6 RID: 47542 RVA: 0x004183C2 File Offset: 0x004165C2
		public override ThingDef StuffDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700219E RID: 8606
		// (get) Token: 0x0600B9B7 RID: 47543 RVA: 0x004183C5 File Offset: 0x004165C5
		public override ThingStyleDef ThingStyleDefForPreview
		{
			get
			{
				Thing thingToInstall = this.ThingToInstall;
				if (thingToInstall == null)
				{
					return null;
				}
				return thingToInstall.StyleDef;
			}
		}

		// Token: 0x0600B9B8 RID: 47544 RVA: 0x004183D8 File Offset: 0x004165D8
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.Order = -10f;
		}

		// Token: 0x0600B9B9 RID: 47545 RVA: 0x0041840B File Offset: 0x0041660B
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		// Token: 0x0600B9BA RID: 47546 RVA: 0x00418418 File Offset: 0x00416618
		public override void ProcessInput(Event ev)
		{
			Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(miniToInstallOrBuildingToReinstall);
				if (!((ThingDef)this.PlacingDef).rotatable)
				{
					this.placingRot = Rot4.North;
				}
				if (ModsConfig.AnomalyActive)
				{
					Building_HoldingPlatform building_HoldingPlatform = miniToInstallOrBuildingToReinstall as Building_HoldingPlatform;
					if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
					{
						Messages.Message("MessageOccupiedHoldingPlatformReinstalled".Translate(), building_HoldingPlatform, MessageTypeDefOf.CautionInput, true);
					}
				}
			}
			base.ProcessInput(ev);
		}

		// Token: 0x0600B9BB RID: 47547 RVA: 0x00418494 File Offset: 0x00416694
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing) && c.GetThingList(base.Map).Find((Thing x) => x.Position == c && x.Rotation == this.placingRot && x.def == this.PlacingDef) != null)
			{
				return new AcceptanceReport("IdenticalThingExists".Translate());
			}
			return GenConstruct.CanPlaceBlueprintAt(this.PlacingDef, c, this.placingRot, base.Map, false, this.MiniToInstallOrBuildingToReinstall, this.ThingToInstall, null, false, false, false);
		}

		// Token: 0x0600B9BC RID: 47548 RVA: 0x00418544 File Offset: 0x00416744
		public override void DesignateSingleCell(IntVec3 c)
		{
			GenSpawn.WipeExistingThings(c, this.placingRot, this.PlacingDef.installBlueprintDef, base.Map, DestroyMode.Deconstruct);
			MinifiedThing minifiedThing = this.MiniToInstallOrBuildingToReinstall as MinifiedThing;
			if (minifiedThing != null)
			{
				GenConstruct.PlaceBlueprintForInstall(minifiedThing, c, base.Map, this.placingRot, Faction.OfPlayer, true);
			}
			else
			{
				GenConstruct.PlaceBlueprintForReinstall((Building)this.MiniToInstallOrBuildingToReinstall, c, base.Map, this.placingRot, Faction.OfPlayer, true);
			}
			FleckMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x0600B9BD RID: 47549 RVA: 0x004185EC File Offset: 0x004167EC
		protected override void DrawGhost(Color ghostCol)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				GauranlenUtility.DrawConnectionsAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				PsychicRitualUtility.DrawPsychicRitualSpotsAffectedByThingOverlay(base.Map, thingDef, UI.MouseCell(), this.placingRot);
			}
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall, null);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint, this.ThingToInstall, true, this.StuffDef);
		}

		// Token: 0x0600B9BE RID: 47550 RVA: 0x004186A3 File Offset: 0x004168A3
		protected override bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return this.ThingToInstall != thing && !GenThing.CloserThingBetween(def, a, b, map, this.ThingToInstall);
		}

		// Token: 0x0600B9BF RID: 47551 RVA: 0x004186C4 File Offset: 0x004168C4
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
