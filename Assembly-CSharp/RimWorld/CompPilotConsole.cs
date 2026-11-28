using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019EB RID: 6635
	[StaticConstructorOnStartup]
	public class CompPilotConsole : CompGravshipFacility
	{
		// Token: 0x17001D45 RID: 7493
		// (get) Token: 0x0600A715 RID: 42773 RVA: 0x003B8708 File Offset: 0x003B6908
		private CompBreakdownable Breakdownable
		{
			get
			{
				CompBreakdownable result;
				if ((result = this.breakdownableComp) == null)
				{
					result = (this.breakdownableComp = this.parent.GetComp<CompBreakdownable>());
				}
				return result;
			}
		}

		// Token: 0x0600A716 RID: 42774 RVA: 0x003B8734 File Offset: 0x003B6934
		private int GetMaxLaunchDistance(PlanetLayer layer)
		{
			Building_GravEngine engine = this.engine;
			int? num = (engine != null) ? new int?(engine.MaxLaunchDistance) : null;
			return Mathf.FloorToInt((((num != null) ? new float?((float)num.GetValueOrDefault()) : null) / layer.Def.rangeDistanceFactor).GetValueOrDefault());
		}

		// Token: 0x0600A717 RID: 42775 RVA: 0x003B87C1 File Offset: 0x003B69C1
		public static Material GetThrusterRadiusMat(PlanetTile tile)
		{
			if (!tile.LayerDef.isSpace)
			{
				return CompPilotConsole.ThrusterRadiusMat;
			}
			return CompPilotConsole.ThrusterRadiusMatHighVis;
		}

		// Token: 0x0600A718 RID: 42776 RVA: 0x003B87DC File Offset: 0x003B69DC
		public static Material GetFuelRadiusMat(PlanetTile tile)
		{
			if (!tile.LayerDef.isSpace)
			{
				return CompPilotConsole.FuelRadiusMat;
			}
			return CompPilotConsole.FuelRadiusMatHighVis;
		}

		// Token: 0x0600A719 RID: 42777 RVA: 0x003B87F7 File Offset: 0x003B69F7
		public void StartChoosingDestination()
		{
			this.StartChoosingDestination_NewTemp(true);
		}

		// Token: 0x0600A71A RID: 42778 RVA: 0x003B8800 File Offset: 0x003B6A00
		public void StartChoosingDestination_NewTemp(bool launching = true)
		{
			if (this.engine == null)
			{
				return;
			}
			if (launching && DebugSettings.skipGravshipTileSelection)
			{
				Find.GravshipController.InitiateTakeoff(this.engine, default(PlanetTile));
				return;
			}
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent), CameraJumper.MovementMode.Pan);
			Find.WorldSelector.ClearSelection();
			PlanetTile curTile = this.parent.Map.Tile;
			PlanetLayer curLayer = this.parent.Map.Tile.Layer;
			PlanetTile cachedClosestLayerTile = PlanetTile.Invalid;
			TilePicker tilePicker = Find.TilePicker;
			Func<PlanetTile, bool> validator = delegate(PlanetTile tile)
			{
				CompPilotConsole.cannotPlaceTileReason.Clear();
				float num;
				int num2;
				if (!GravshipUtility.TryGetPathFuelCost(curTile, tile, out num, out num2, 10f, this.engine.FuelUseageFactor) && !DebugSettings.ignoreGravshipRange)
				{
					Messages.Message("CannotLaunchDestination".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				MapParent mapParent;
				if (!this.engine.HasSignalJammer && Find.WorldObjects.TryGetWorldObjectAt<MapParent>(tile, out mapParent) && mapParent.RequiresSignalJammerToReach)
				{
					Messages.Message("TransportPodDestinationRequiresSignalJammer".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (num > this.engine.TotalFuel && !DebugSettings.ignoreGravshipRange)
				{
					Messages.Message("CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (num2 > this.GetMaxLaunchDistance(tile.Layer) && !DebugSettings.ignoreGravshipRange)
				{
					Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (tile == this.parent.Tile && !this.parent.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
				{
					Messages.Message("CannotLandOnSameTile".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				MapParent mapParent2 = Find.World.worldObjects.MapParentAt(tile);
				if (mapParent2 != null && mapParent2.HasMap)
				{
					return true;
				}
				if (!TileFinder.IsValidTileForNewSettlement(tile, CompPilotConsole.cannotPlaceTileReason, true))
				{
					Messages.Message(CompPilotConsole.cannotPlaceTileReason.ToString(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				return true;
			};
			Action <>9__6;
			Action<PlanetTile> tileChosen = delegate(PlanetTile tile)
			{
				if (!launching)
				{
					return;
				}
				PlanetTile tile2 = tile;
				Action settleAction = delegate()
				{
					WorldComponent_GravshipController.DestroyTreesAroundSubstructure(this.engine.Map, this.engine.ValidSubstructure, 2, null);
					Find.World.renderer.wantedMode = WorldRenderMode.None;
					this.engine.ConsumeFuel(tile);
					Find.GravshipController.InitiateTakeoff(this.engine, tile);
					SoundDefOf.Gravship_Launch.PlayOneShotOnCamera(null);
				};
				Action cancelAction;
				if ((cancelAction = <>9__6) == null)
				{
					cancelAction = (<>9__6 = delegate()
					{
						this.StartChoosingDestination_NewTemp(true);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile2, settleAction, cancelAction, this.engine);
			};
			Action onGuiAction = delegate()
			{
				WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
				PlanetTile planetTile = GenWorld.MouseTile(false);
				PlanetTile planetTile2 = (!planetTile.Valid && singleSelectedObject != null) ? singleSelectedObject.Tile : planetTile;
				Vector2 mousePosition = Event.current.mousePosition;
				GUI.DrawTexture(new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f), CompPilotConsole.TargeterMouseAttachment);
				if (planetTile2.Valid)
				{
					bool flag = false;
					int maxLaunchDistance = this.GetMaxLaunchDistance(PlanetLayer.Selected);
					PlanetTile tileA = (curTile.Layer == planetTile2.Layer) ? curTile : planetTile2.Layer.GetClosestTile_NewTemp(curTile, false);
					string text;
					float num;
					int num2;
					if (Find.WorldGrid.ApproxDistanceInTiles(tileA, planetTile2) > (float)(maxLaunchDistance + 10))
					{
						text = string.Format(" ({0})", "TransportPodDestinationBeyondMaximumRange".Translate());
					}
					else if (GravshipUtility.TryGetPathFuelCost(curTile, planetTile2, out num, out num2, 10f, this.engine.FuelUseageFactor))
					{
						flag = (num <= this.engine.TotalFuel && num2 <= maxLaunchDistance);
						text = string.Format("{0}: {1}", "Cost".Translate().CapitalizeFirst(), "FuelAmount".Translate(num, ThingDefOf.Chemfuel));
						if (num2 > maxLaunchDistance)
						{
							text += string.Format(" ({0})", "TransportPodDestinationBeyondMaximumRange".Translate());
						}
						else if (!flag)
						{
							text += string.Format(" ({0})", "CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst());
						}
						else if (!this.engine.HasSignalJammer && singleSelectedObject is MapParent && singleSelectedObject.RequiresSignalJammerToReach)
						{
							flag = false;
							text += string.Format(" ({0})", "TransportPodDestinationRequiresSignalJammer".Translate());
						}
					}
					else
					{
						text = "CannotLaunchDestination".Translate();
					}
					if (singleSelectedObject != null && !planetTile.Valid)
					{
						Widgets.WorldAttachedLabel(singleSelectedObject.DrawPos, text, 0f, 0f, new Color?(flag ? Color.white : ColorLibrary.RedReadable));
						return;
					}
					Widgets.MouseAttachedLabel(text, 0f, 0f, new Color?(flag ? Color.white : ColorLibrary.RedReadable));
				}
			};
			Action onUpdateAction = delegate()
			{
				int maxLaunchDistance = this.GetMaxLaunchDistance(PlanetLayer.Selected);
				int num = GravshipUtility.MaxDistForFuel(this.engine.TotalFuel, curLayer, PlanetLayer.Selected, 10f, this.engine.FuelUseageFactor);
				PlanetTile planetTile = curTile;
				if (curTile.Layer != Find.WorldSelector.SelectedLayer)
				{
					if (cachedClosestLayerTile.Layer != Find.WorldSelector.SelectedLayer || !cachedClosestLayerTile.Valid)
					{
						cachedClosestLayerTile = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(curTile, false);
					}
					planetTile = cachedClosestLayerTile;
				}
				GenDraw.DrawWorldRadiusRing(planetTile, maxLaunchDistance, CompPilotConsole.GetThrusterRadiusMat(planetTile));
				if (num < maxLaunchDistance)
				{
					GenDraw.DrawWorldRadiusRing(planetTile, num, CompPilotConsole.GetFuelRadiusMat(planetTile));
				}
			};
			bool allowEscape = true;
			Action noTileChosen = delegate()
			{
				CameraJumper.TryJump(this.parent, CameraJumper.MovementMode.Cut);
			};
			string title = "ChooseWhereToLand".Translate();
			bool showRandomButton = false;
			bool selectTileBehindObject = true;
			bool hideFormCaravanGizmo = true;
			string noTileChosenMessage = "MessageNoLandingSiteSelected".Translate();
			tilePicker.StartTargeting_NewTemp(validator, tileChosen, onGuiAction, onUpdateAction, allowEscape, noTileChosen, title, showRandomButton, selectTileBehindObject, hideFormCaravanGizmo, launching, true, noTileChosenMessage);
		}

		// Token: 0x0600A71B RID: 42779 RVA: 0x003B8923 File Offset: 0x003B6B23
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPilotConsole.<CompGetGizmosExtra>d__15 <CompGetGizmosExtra>d__ = new CompPilotConsole.<CompGetGizmosExtra>d__15(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A71C RID: 42780 RVA: 0x003B8934 File Offset: 0x003B6B34
		public AcceptanceReport CanUseNow()
		{
			if (this.engine == null)
			{
				return new AcceptanceReport("CannotLaunchNoEngine".Translate().CapitalizeFirst());
			}
			if (this.Breakdownable.BrokenDown)
			{
				return new AcceptanceReport("BrokenDown".Translate().CapitalizeFirst());
			}
			if (!this.engine.ValidSubstructureAt(this.parent.InteractionCell))
			{
				return new AcceptanceReport("PilotConsoleInaccessible".Translate().CapitalizeFirst());
			}
			AcceptanceReport result = this.engine.CanLaunch(this);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600A71D RID: 42781 RVA: 0x003B89E4 File Offset: 0x003B6BE4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (this.engine != null)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("GravshipName".Translate().CapitalizeFirst()).Append(": ").Append(this.engine.RenamableLabel);
				stringBuilder.AppendInNewLine("GravshipRange".Translate().CapitalizeFirst()).Append(": ").Append(this.GetMaxLaunchDistance(this.parent.Map.Tile.Layer).ToString("F0"));
				foreach (CompGravshipFacility compGravshipFacility in this.engine.GravshipComponents)
				{
					CompGravshipThruster compGravshipThruster = compGravshipFacility as CompGravshipThruster;
					if (compGravshipThruster != null && compGravshipThruster.Blocked)
					{
						stringBuilder.Append(" (").Append("ThrusterBlocked".Translate()).Append(")");
						break;
					}
				}
				stringBuilder.AppendInNewLine("StoredChemfuel".Translate().CapitalizeFirst()).Append(": ").Append(this.engine.TotalFuel.ToString("F0")).Append(" / ").Append(this.engine.MaxFuel.ToString("F0"));
				stringBuilder.AppendInNewLine("FuelConsumption".Translate().CapitalizeFirst()).Append(": ").Append("FuelPerTile".Translate(this.engine.FuelPerTile.ToString("0.#")).CapitalizeFirst());
				if (GenTicks.TicksGame < this.engine.cooldownCompleteTick)
				{
					stringBuilder.AppendInNewLine("CannotLaunchOnCooldown".Translate((this.engine.cooldownCompleteTick - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A71E RID: 42782 RVA: 0x003B8C5C File Offset: 0x003B6E5C
		private AcceptanceReport ValidateNavigator(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.IsColonistPlayerControlled)
			{
				return false;
			}
			if (pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))
			{
				return "Incapable".Translate();
			}
			if (pawn.skills == null || pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
			{
				return "Incapable".Translate();
			}
			return true;
		}

		// Token: 0x0600A71F RID: 42783 RVA: 0x003B8CF3 File Offset: 0x003B6EF3
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompPilotConsole.<CompFloatMenuOptions>d__19 <CompFloatMenuOptions>d__ = new CompPilotConsole.<CompFloatMenuOptions>d__19(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0400626C RID: 25196
		[Unsaved(false)]
		private CompBreakdownable breakdownableComp;

		// Token: 0x0400626D RID: 25197
		public static readonly Material ThrusterRadiusMat = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, ColorLibrary.White, 3590);

		// Token: 0x0400626E RID: 25198
		public static readonly Material FuelRadiusMat = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, ColorLibrary.Yellow, 3590);

		// Token: 0x0400626F RID: 25199
		public static readonly Material ThrusterRadiusMatHighVis = MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, ColorLibrary.White, 3590);

		// Token: 0x04006270 RID: 25200
		public static readonly Material FuelRadiusMatHighVis = MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, ColorLibrary.Yellow, 3590);

		// Token: 0x04006271 RID: 25201
		private static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x04006272 RID: 25202
		private static readonly StringBuilder cannotPlaceTileReason = new StringBuilder();

		// Token: 0x04006273 RID: 25203
		private static readonly CachedTexture ViewRangeTex = new CachedTexture("UI/Commands/ViewRange");
	}
}
