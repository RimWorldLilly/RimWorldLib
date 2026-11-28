using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C69 RID: 7273
	public class Designator_Build : Designator_Place
	{
		// Token: 0x17002187 RID: 8583
		// (get) Token: 0x0600B989 RID: 47497 RVA: 0x00416B69 File Offset: 0x00414D69
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		// Token: 0x17002188 RID: 8584
		// (get) Token: 0x0600B98A RID: 47498 RVA: 0x00416B74 File Offset: 0x00414D74
		public override ThingDef StuffDef
		{
			get
			{
				if (this.stuffDef != null)
				{
					return this.stuffDef;
				}
				ThingDef thingDef = null;
				ThingDef thingDef2 = this.entDef as ThingDef;
				if (thingDef2 != null && thingDef2.MadeFromStuff)
				{
					thingDef = GenStuff.DefaultStuffFor(thingDef2);
					if (Find.CurrentMap != null && Find.CurrentMap.resourceCounter.GetCount(thingDef) < thingDef2.CostStuffCount)
					{
						ThingDef thingDef3 = null;
						foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
						{
							if (keyValuePair.Key.IsStuff && keyValuePair.Key.stuffProps.canSuggestUseDefaultStuff && keyValuePair.Key.stuffProps.CanMake(thingDef2) && keyValuePair.Value >= thingDef2.CostStuffCount)
							{
								thingDef3 = keyValuePair.Key;
								break;
							}
						}
						if (thingDef3 != null)
						{
							thingDef = thingDef3;
						}
					}
				}
				return thingDef;
			}
		}

		// Token: 0x17002189 RID: 8585
		// (get) Token: 0x0600B98B RID: 47499 RVA: 0x00416C7C File Offset: 0x00414E7C
		public ThingDef StuffDefRaw
		{
			get
			{
				return this.stuffDef;
			}
		}

		// Token: 0x1700218A RID: 8586
		// (get) Token: 0x0600B98C RID: 47500 RVA: 0x00416C84 File Offset: 0x00414E84
		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				string text;
				if (thingDef != null && this.writeStuff)
				{
					text = GenLabel.ThingLabel(thingDef, this.StuffDef, 1);
				}
				else
				{
					text = this.entDef.label;
				}
				if (this.sourcePrecept != null)
				{
					text = this.sourcePrecept.TransformThingLabel(text);
				}
				if (thingDef != null && !this.writeStuff && thingDef.MadeFromStuff)
				{
					text += "...";
				}
				return text;
			}
		}

		// Token: 0x1700218B RID: 8587
		// (get) Token: 0x0600B98D RID: 47501 RVA: 0x00416CF9 File Offset: 0x00414EF9
		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		// Token: 0x1700218C RID: 8588
		// (get) Token: 0x0600B98E RID: 47502 RVA: 0x00416D08 File Offset: 0x00414F08
		public override Color IconDrawColor
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef, Faction.OfPlayer);
					if (ideoColorForBuilding != null)
					{
						return ideoColorForBuilding.Value;
					}
				}
				if (this.StuffDef != null)
				{
					return this.entDef.GetColorForStuff(this.StuffDef);
				}
				return this.entDef.uiIconColor;
			}
		}

		// Token: 0x1700218D RID: 8589
		// (get) Token: 0x0600B98F RID: 47503 RVA: 0x00416D68 File Offset: 0x00414F68
		public override bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (this.entDef.minTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel < this.entDef.minTechLevelToBuild)
				{
					return false;
				}
				if (this.entDef.maxTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel > this.entDef.maxTechLevelToBuild)
				{
					return false;
				}
				if (!this.entDef.IsResearchFinished)
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && this.entDef.minMonolithLevel > Find.Anomaly.HighestLevelReached && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				if (!Find.Storyteller.difficulty.AllowedToBuild(this.entDef))
				{
					return false;
				}
				if (this.entDef.PlaceWorkers != null)
				{
					using (List<PlaceWorker>.Enumerator enumerator = this.entDef.PlaceWorkers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsBuildDesignatorVisible(this.entDef))
							{
								return false;
							}
						}
					}
				}
				if (this.entDef.buildingPrerequisites != null)
				{
					for (int i = 0; i < this.entDef.buildingPrerequisites.Count; i++)
					{
						if (!base.Map.listerBuildings.ColonistsHaveBuilding(this.entDef.buildingPrerequisites[i]))
						{
							return false;
						}
					}
				}
				if (this.entDef.discoveryPrerequisites != null)
				{
					for (int j = 0; j < this.entDef.discoveryPrerequisites.Count; j++)
					{
						if (Find.HiddenItemsManager.Hidden(this.entDef.discoveryPrerequisites[j]))
						{
							return false;
						}
					}
				}
				return !ModsConfig.OdysseyActive || !this.entDef.requireInspectedGravEngine || Find.ResearchManager.gravEngineInspected;
			}
		}

		// Token: 0x1700218E RID: 8590
		// (get) Token: 0x0600B990 RID: 47504 RVA: 0x00416F48 File Offset: 0x00415148
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				if (!this.eyedropMode)
				{
					return this.entDef.drawStyleCategory;
				}
				return this.eyedropper.DrawStyleCategory;
			}
		}

		// Token: 0x1700218F RID: 8591
		// (get) Token: 0x0600B991 RID: 47505 RVA: 0x00416F69 File Offset: 0x00415169
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002190 RID: 8592
		// (get) Token: 0x0600B992 RID: 47506 RVA: 0x00416F6C File Offset: 0x0041516C
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17002191 RID: 8593
		// (get) Token: 0x0600B993 RID: 47507 RVA: 0x00416F73 File Offset: 0x00415173
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-Build-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x17002192 RID: 8594
		// (get) Token: 0x0600B994 RID: 47508 RVA: 0x00416FA4 File Offset: 0x004151A4
		public override ThingStyleDef ThingStyleDefForPreview
		{
			get
			{
				if (this.sourcePrecept == null)
				{
					return this.ThingStyleDefNonPreceptSource;
				}
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					return this.sourcePrecept.ideo.GetStyleFor(thingDef);
				}
				return null;
			}
		}

		// Token: 0x17002193 RID: 8595
		// (get) Token: 0x0600B995 RID: 47509 RVA: 0x00416FE4 File Offset: 0x004151E4
		public ThingStyleDef ThingStyleDefNonPreceptSource
		{
			get
			{
				ThingDef thingDef = this.PlacingDef as ThingDef;
				if (thingDef == null)
				{
					return null;
				}
				if (Find.IdeoManager.classicMode && this.styleOverridden)
				{
					return this.styleDef;
				}
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				if (ideos == null)
				{
					return null;
				}
				Ideo primaryIdeo = ideos.PrimaryIdeo;
				if (primaryIdeo == null)
				{
					return null;
				}
				return primaryIdeo.GetStyleFor(thingDef);
			}
		}

		// Token: 0x17002194 RID: 8596
		// (get) Token: 0x0600B996 RID: 47510 RVA: 0x0041703E File Offset: 0x0041523E
		public override float Order
		{
			get
			{
				return this.entDef.uiOrder;
			}
		}

		// Token: 0x0600B997 RID: 47511 RVA: 0x0041704B File Offset: 0x0041524B
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x0600B998 RID: 47512 RVA: 0x00417054 File Offset: 0x00415254
		private void UpdateIcon()
		{
			this.icon = this.entDef.GetUIIconForStuff(this.StuffDef);
			TerrainDef terrainDef = this.entDef as TerrainDef;
			if (terrainDef != null && terrainDef.cropIcon)
			{
				this.iconTexCoords = Widgets.CroppedTerrainTextureRect((Texture2D)this.icon);
			}
		}

		// Token: 0x0600B999 RID: 47513 RVA: 0x004170A5 File Offset: 0x004152A5
		public override void Deselected()
		{
			if (this.resetStuffAfterDeselect != null)
			{
				this.stuffDef = this.resetStuffAfterDeselect;
			}
			this.styleOverridden = false;
			this.glowerColorOverride = null;
			this.resetStuffAfterDeselect = null;
			this.UpdateIcon();
		}

		// Token: 0x0600B99A RID: 47514 RVA: 0x004170DC File Offset: 0x004152DC
		public Designator_Build(BuildableDef entDef)
		{
			this.entDef = entDef;
			this.iconAngle = entDef.uiIconAngle;
			this.iconOffset = entDef.uiIconOffset;
			this.hotKey = entDef.designationHotKey;
			this.tutorTag = entDef.defName;
			this.Order = 2000f;
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null)
			{
				this.iconProportions = (thingDef.defaultPlacingRot.IsHorizontal ? thingDef.graphicData.drawSize.Rotated() : thingDef.graphicData.drawSize);
				this.iconDrawScale = GenUI.IconDrawScale(thingDef);
			}
			else
			{
				this.iconProportions = new Vector2(1f, 1f);
				this.iconDrawScale = 1f;
			}
			this.ResetStuffToDefault();
			BuildableDef placingDef = this.PlacingDef;
			TerrainDef terrain = placingDef as TerrainDef;
			if (terrain != null && terrain.colorDef != null && terrain.designatorDropdown != null && terrain.designatorDropdown.includeEyeDropperTool)
			{
				Func<TerrainDef, bool> <>9__1;
				this.eyedropper = new Designator_Eyedropper(delegate(ColorDef newCol)
				{
					IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
					Func<TerrainDef, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((TerrainDef x) => x.designatorDropdown == terrain.designatorDropdown));
					}
					foreach (TerrainDef terrainDef in allDefs.Where(predicate))
					{
						if (terrainDef.colorDef == newCol)
						{
							Designator_Dropdown designator_Dropdown = BuildCopyCommandUtility.FindAllowedDesignatorRoot(terrainDef, true) as Designator_Dropdown;
							if (designator_Dropdown != null)
							{
								Log.Message("Selecting dropdown");
								Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(terrainDef, true);
								designator_Dropdown.SetActiveDesignator(des, true);
								Find.DesignatorManager.Select(des);
								break;
							}
						}
					}
				}, "SelectColoredFloor".Translate(), "DesignatorEyeDropperDesc_Carpet".Translate());
			}
		}

		// Token: 0x0600B99B RID: 47515 RVA: 0x00417224 File Offset: 0x00415424
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			}
			return result;
		}

		// Token: 0x0600B99C RID: 47516 RVA: 0x00417260 File Offset: 0x00415460
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			ThingStyleDef thingStyleDef = (this.PlacingDef is ThingDef) ? this.ThingStyleDefForPreview : null;
			Color? color = parms.lowLight ? new Color?(Command.LowLightIconColor) : null;
			Color value = color.GetValueOrDefault();
			if (color == null)
			{
				value = this.IconDrawColor;
				color = new Color?(value);
			}
			Widgets.DefIcon(rect, this.PlacingDef, this.StuffDef, 0.85f, thingStyleDef, false, color, this.overrideMaterial, null, 1f);
		}

		// Token: 0x0600B99D RID: 47517 RVA: 0x004172F4 File Offset: 0x004154F4
		public Texture ResolvedIcon(ThingStyleDef styleDef = null)
		{
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				ThingStyleDef thingStyleDef = styleDef ?? this.ThingStyleDefNonPreceptSource;
				return Widgets.GetIconFor(thingDef, this.StuffDef, thingStyleDef, null);
			}
			if (this.StuffDef != null)
			{
				Graphic_Appearances graphic_Appearances = this.entDef.graphic as Graphic_Appearances;
				if (graphic_Appearances != null)
				{
					return (Texture2D)graphic_Appearances.SubGraphicFor(this.StuffDef).MatAt(this.entDef.defaultPlacingRot, null).mainTexture;
				}
			}
			return this.icon;
		}

		// Token: 0x0600B99E RID: 47518 RVA: 0x0041737D File Offset: 0x0041557D
		public override void DrawMouseAttachments()
		{
			this.eyedropMode = (this.eyedropper != null && KeyBindingDefOf.ShowEyedropper.IsDown);
			if (this.eyedropMode)
			{
				this.eyedropper.DrawMouseAttachments();
				return;
			}
			base.DrawMouseAttachments();
		}

		// Token: 0x0600B99F RID: 47519 RVA: 0x004173B4 File Offset: 0x004155B4
		public void ResetStuffToDefault()
		{
			this.stuffDef = null;
			this.UpdateIcon();
		}

		// Token: 0x0600B9A0 RID: 47520 RVA: 0x004173C4 File Offset: 0x004155C4
		protected override void DrawPlaceMouseAttachments(float curX, ref float curY)
		{
			base.DrawPlaceMouseAttachments(curX, ref curY);
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				if (this.eyedropper != null)
				{
					Rect rect = new Rect(curX, curY, 9999f, Text.LineHeight * 2f);
					Widgets.Label(rect, "Color".Translate() + ": " + ((TerrainDef)this.PlacingDef).colorDef.LabelCap + "\n" + KeyBindingDefOf.ShowEyedropper.MainKeyLabel + ": " + "GrabExistingColor".Translate());
					curY += rect.height;
				}
				DesignationDragger dragger = Find.DesignatorManager.Dragger;
				int num;
				if (dragger.Dragging)
				{
					num = dragger.DragCells.Count<IntVec3>();
				}
				else
				{
					num = 1;
				}
				List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.StuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					float y = curY;
					Widgets.ThingIcon(new Rect(curX, y, 27f, 27f), thingDefCountClass.thingDef, null, null, 1f, null, null, 1f);
					Rect rect2 = new Rect(curX + 29f, y, 999f, 29f);
					int num2 = num * thingDefCountClass.count;
					string text = num2.ToString();
					if (base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < num2)
					{
						GUI.color = Color.red;
						text += " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect2, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					curY += 29f;
				}
			}
		}

		// Token: 0x0600B9A1 RID: 47521 RVA: 0x004175D0 File Offset: 0x004157D0
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef == null || !thingDef.MadeFromStuff)
			{
				base.ProcessInput(ev);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (ThingDef thingDef2 in base.Map.resourceCounter.AllCountedAmounts.Keys.OrderByDescending(delegate(ThingDef d)
			{
				StuffProperties stuffProps = d.stuffProps;
				if (stuffProps == null)
				{
					return float.PositiveInfinity;
				}
				return stuffProps.commonality;
			}).ThenBy((ThingDef d) => d.BaseMarketValue))
			{
				if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
				{
					ThingDef localStuffDef = thingDef2;
					string text;
					if (this.sourcePrecept != null)
					{
						text = "ThingMadeOfStuffLabel".Translate(localStuffDef.LabelAsStuff, this.sourcePrecept.Label);
					}
					else
					{
						text = GenLabel.ThingLabel(this.entDef, localStuffDef, 1);
					}
					text = text.CapitalizeFirst();
					list.Add(new FloatMenuOption(text, delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.stuffDef = localStuffDef;
						this.writeStuff = true;
					}, thingDef2, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null)
					{
						tutorTag = "SelectStuff-" + thingDef.defName + "-" + localStuffDef.defName
					});
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("NoStuffsToBuildWith".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.onCloseCallback = delegate()
			{
				this.writeStuff = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x0600B9A2 RID: 47522 RVA: 0x00417830 File Offset: 0x00415A30
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				return this.eyedropper.CanDesignateCell(c);
			}
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, this.placingRot, base.Map, DebugSettings.godMode, null, null, this.StuffDef, false, false, false);
		}

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x0041787C File Offset: 0x00415A7C
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
			{
				return;
			}
			if (this.eyedropMode)
			{
				this.eyedropper.DesignateSingleCell(c);
				return;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				Frame frame = thingList[i] as Frame;
				if (frame != null && this.entDef.blueprintDef.replaceTags.NotNullAndContainsAnyElement(frame.def.replaceTags))
				{
					frame.Destroy(DestroyMode.Cancel);
				}
			}
			if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.StuffDef) == 0f)
			{
				TerrainDef terrainDef = this.entDef as TerrainDef;
				if (terrainDef != null)
				{
					base.Map.terrainGrid.RemoveTempTerrain(c, false, false);
					if (terrainDef.isFoundation)
					{
						if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
						{
							base.Map.terrainGrid.RemoveTopLayer(c, !DebugSettings.godMode);
						}
						base.Map.terrainGrid.SetFoundation(c, terrainDef);
					}
					else if (terrainDef.temporary)
					{
						base.Map.terrainGrid.SetTempTerrain(c, terrainDef);
					}
					else
					{
						base.Map.terrainGrid.SetTerrain(c, terrainDef);
					}
				}
				else
				{
					Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.StuffDef);
					thing.SetFactionDirect(Faction.OfPlayer);
					Thing thing2 = GenSpawn.Spawn(thing, c, base.Map, this.placingRot, WipeMode.Vanish, false, false);
					if (this.sourcePrecept != null)
					{
						Building building = thing2 as Building;
						if (building != null)
						{
							building.StyleSourcePrecept = this.sourcePrecept;
							goto IL_1CE;
						}
					}
					if (this.sourcePrecept == null)
					{
						thing2.StyleDef = this.ThingStyleDefNonPreceptSource;
					}
					IL_1CE:
					if (this.glowerColorOverride != null)
					{
						CompGlower compGlower = thing2.TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							compGlower.GlowColor = this.glowerColorOverride.Value;
						}
					}
				}
			}
			else
			{
				GenSpawn.WipeExistingThings(c, this.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
				Blueprint_Build blueprint_Build = GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, this.placingRot, Faction.OfPlayer, this.StuffDef, null, null, true);
				blueprint_Build.StyleSourcePrecept = this.sourcePrecept;
				if (blueprint_Build.StyleSourcePrecept == null)
				{
					blueprint_Build.StyleDef = this.ThingStyleDefNonPreceptSource;
				}
				blueprint_Build.glowerColorOverride = this.glowerColorOverride;
			}
			FleckMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.entDef.Size), base.Map);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.IsOrbitalTradeBeacon)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BuildOrbitalTradeBeacon, KnowledgeAmount.Total);
			}
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, c));
			}
			if (this.entDef.PlaceWorkers != null)
			{
				for (int j = 0; j < this.entDef.PlaceWorkers.Count; j++)
				{
					this.entDef.PlaceWorkers[j].PostPlace(base.Map, this.entDef, c, this.placingRot);
				}
			}
		}

		// Token: 0x0600B9A4 RID: 47524 RVA: 0x00417BB3 File Offset: 0x00415DB3
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef, this.placingRot);
		}

		// Token: 0x0600B9A5 RID: 47525 RVA: 0x00417BCC File Offset: 0x00415DCC
		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.CostStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, thingDef, this.StuffDef);
			}
			else
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, this.entDef);
			}
			Text.Font = GameFont.Small;
			Designator_Build.tmpcostList.Clear();
			Designator_Build.tmpcostList.AddRange(this.entDef.CostListAdjusted(this.StuffDef, false));
			Designator_Build.tmpcostList.Sort((ThingDefCountClass a, ThingDefCountClass b) => b.count - a.count);
			for (int i = 0; i < Designator_Build.tmpcostList.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = Designator_Build.tmpcostList[i];
				Color color = GUI.color;
				Widgets.ThingIcon(new Rect(0f, curY, 20f, 20f), thingDefCountClass.thingDef, null, null, 1f, null, null, 1f);
				GUI.color = color;
				if (thingDefCountClass.thingDef != null && thingDefCountClass.thingDef.resourceReadoutPriority != ResourceCountPriority.Uncounted && base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					GUI.color = Color.red;
				}
				Widgets.Label(new Rect(26f, curY + 2f, 50f, 100f), thingDefCountClass.count.ToString());
				GUI.color = Color.white;
				string text;
				if (thingDefCountClass.thingDef == null)
				{
					text = "(" + "UnchosenStuff".Translate() + ")";
				}
				else
				{
					text = thingDefCountClass.thingDef.LabelCap;
				}
				float width2 = width - 60f;
				float num = Text.CalcHeight(text, width2) - 5f;
				Widgets.Label(new Rect(60f, curY + 2f, width2, num + 5f), text);
				curY += num;
			}
			StyleCategoryDef styleCategoryDef = this.entDef.dominantStyleCategory;
			if (styleCategoryDef == null && thingDef != null)
			{
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				StyleCategoryDef styleCategoryDef2;
				if (ideos == null)
				{
					styleCategoryDef2 = null;
				}
				else
				{
					Ideo primaryIdeo = ideos.PrimaryIdeo;
					styleCategoryDef2 = ((primaryIdeo != null) ? primaryIdeo.GetStyleCategoryFor(thingDef) : null);
				}
				styleCategoryDef = styleCategoryDef2;
			}
			if (styleCategoryDef != null)
			{
				TaggedString taggedString = "DominantStyle".Translate().CapitalizeFirst() + ": " + styleCategoryDef.LabelCap;
				Rect rect = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString, width));
				Widgets.Label(rect, taggedString);
				curY += rect.height;
			}
			if (this.entDef.constructionSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Construction, this.entDef.constructionSkillPrerequisite, width, ref curY);
			}
			if (this.entDef.artisticSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Artistic, this.entDef.artisticSkillPrerequisite, width, ref curY);
			}
			bool flag = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(SkillDefOf.Construction).Level >= this.entDef.constructionSkillPrerequisite && pawn.skills.GetSkill(SkillDefOf.Artistic).Level >= this.entDef.artisticSkillPrerequisite)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TaggedString taggedString2 = "NoColonistWithAllSkillsForConstructing".Translate(Faction.OfPlayer.def.pawnsPlural);
				GUI.color = Color.red;
				Pawn pawn2;
				if (this.AnyMechWithSkillsRequired(this.entDef, out pawn2))
				{
					taggedString2 += " " + "MechCanBuildThis".Translate(pawn2.kindDef.label);
					GUI.color = Color.white;
				}
				Rect rect2 = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString2, width));
				Widgets.Label(rect2, taggedString2);
				GUI.color = Color.white;
				curY += rect2.height;
			}
			curY += 4f;
		}

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x0041804C File Offset: 0x0041624C
		private bool AnyColonistWithSkill(int skill, SkillDef skillDef, bool careIfDisabled)
		{
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(skillDef).Level >= skill && (!careIfDisabled || pawn.workSettings.WorkIsActive(WorkTypeDefOf.Construction)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B9A7 RID: 47527 RVA: 0x004180D4 File Offset: 0x004162D4
		public bool AnyMechWithSkillsRequired(BuildableDef def, out Pawn pawn)
		{
			return MechanitorUtility.AnyPlayerMechCanDoWork(WorkTypeDefOf.Construction, def.constructionSkillPrerequisite, out pawn);
		}

		// Token: 0x0600B9A8 RID: 47528 RVA: 0x004180E8 File Offset: 0x004162E8
		private void DrawSkillRequirement(SkillDef skillDef, int requirement, float width, ref float curY)
		{
			Rect rect = new Rect(0f, curY + 2f, width, 24f);
			if (!this.AnyColonistWithSkill(requirement, skillDef, false))
			{
				GUI.color = Color.red;
				TooltipHandler.TipRegionByKey(rect, "NoColonistWithSkillTip", Faction.OfPlayer.def.pawnsPlural);
			}
			else if (!this.AnyColonistWithSkill(requirement, skillDef, true))
			{
				GUI.color = Color.yellow;
				TooltipHandler.TipRegionByKey(rect, "AllColonistsWithSkillHaveDisabledConstructingTip", Faction.OfPlayer.def.pawnsPlural, WorkTypeDefOf.Construction.gerundLabel);
			}
			else
			{
				GUI.color = new Color(0.72f, 0.87f, 0.72f);
			}
			Rect rect2 = rect;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("SkillNeededForConstructing".Translate(skillDef.LabelCap));
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(requirement);
			Widgets.Label(rect2, defaultInterpolatedStringHandler.ToStringAndClear());
			GUI.color = Color.white;
			curY += 18f;
		}

		// Token: 0x0600B9A9 RID: 47529 RVA: 0x00418200 File Offset: 0x00416400
		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
			this.UpdateIcon();
		}

		// Token: 0x0600B9AA RID: 47530 RVA: 0x0041820F File Offset: 0x0041640F
		public void SetTemporaryVars(ThingDef stuffDef, bool styleOverridden)
		{
			this.resetStuffAfterDeselect = this.stuffDef;
			this.stuffDef = stuffDef;
			this.styleOverridden = styleOverridden;
			this.UpdateIcon();
		}

		// Token: 0x040069C8 RID: 27080
		protected BuildableDef entDef;

		// Token: 0x040069C9 RID: 27081
		public Precept_Building sourcePrecept;

		// Token: 0x040069CA RID: 27082
		public ThingStyleDef styleDef;

		// Token: 0x040069CB RID: 27083
		public bool styleOverridden;

		// Token: 0x040069CC RID: 27084
		public ColorInt? glowerColorOverride;

		// Token: 0x040069CD RID: 27085
		private ThingDef stuffDef;

		// Token: 0x040069CE RID: 27086
		private ThingDef resetStuffAfterDeselect;

		// Token: 0x040069CF RID: 27087
		private bool writeStuff;

		// Token: 0x040069D0 RID: 27088
		public const float DragPriceDrawNumberX = 29f;

		// Token: 0x040069D1 RID: 27089
		public const float ResLineSpacing = 29f;

		// Token: 0x040069D2 RID: 27090
		private static List<ThingDefCountClass> tmpcostList = new List<ThingDefCountClass>();
	}
}
