using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AB7 RID: 6839
	[StaticConstructorOnStartup]
	public class Building_GravEngine : Building, IRenameable
	{
		// Token: 0x17001EFE RID: 7934
		// (get) Token: 0x0600AD54 RID: 44372 RVA: 0x003D179F File Offset: 0x003CF99F
		private static Graphic OnCooldownGraphic
		{
			get
			{
				Graphic result;
				if ((result = Building_GravEngine.onCooldownGraphic) == null)
				{
					result = (Building_GravEngine.onCooldownGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/GravEngine/GravEngine_Cooldown", ShaderDatabase.Cutout, Vector2.one * 3f, Color.white));
				}
				return result;
			}
		}

		// Token: 0x17001EFF RID: 7935
		// (get) Token: 0x0600AD55 RID: 44373 RVA: 0x003D17D3 File Offset: 0x003CF9D3
		// (set) Token: 0x0600AD56 RID: 44374 RVA: 0x003D17E5 File Offset: 0x003CF9E5
		public string RenamableLabel
		{
			get
			{
				return this.gravshipName ?? this.BaseLabel;
			}
			set
			{
				this.gravshipName = value;
				this.nameHidden = false;
			}
		}

		// Token: 0x17001F00 RID: 7936
		// (get) Token: 0x0600AD57 RID: 44375 RVA: 0x003D17F5 File Offset: 0x003CF9F5
		public string BaseLabel
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17001F01 RID: 7937
		// (get) Token: 0x0600AD58 RID: 44376 RVA: 0x003D1807 File Offset: 0x003CFA07
		public string InspectLabel
		{
			get
			{
				return this.BaseLabel;
			}
		}

		// Token: 0x17001F02 RID: 7938
		// (get) Token: 0x0600AD59 RID: 44377 RVA: 0x003D180F File Offset: 0x003CFA0F
		public override Graphic Graphic
		{
			get
			{
				if (Find.TickManager.TicksGame >= this.cooldownCompleteTick)
				{
					return base.Graphic;
				}
				return Building_GravEngine.OnCooldownGraphic;
			}
		}

		// Token: 0x17001F03 RID: 7939
		// (get) Token: 0x0600AD5A RID: 44378 RVA: 0x003D1830 File Offset: 0x003CFA30
		public CompAffectedByFacilities AffectedByFacilities
		{
			get
			{
				CompAffectedByFacilities result;
				if ((result = this.affectedByFacilities) == null)
				{
					result = (this.affectedByFacilities = base.GetComp<CompAffectedByFacilities>());
				}
				return result;
			}
		}

		// Token: 0x17001F04 RID: 7940
		// (get) Token: 0x0600AD5B RID: 44379 RVA: 0x003D1856 File Offset: 0x003CFA56
		public int MaxLaunchDistance
		{
			get
			{
				return (int)this.GetStatValue(StatDefOf.GravshipRange, true, -1);
			}
		}

		// Token: 0x17001F05 RID: 7941
		// (get) Token: 0x0600AD5C RID: 44380 RVA: 0x003D1866 File Offset: 0x003CFA66
		public HashSet<IntVec3> ValidSubstructure
		{
			get
			{
				this.UpdateSubstructureIfNeeded(true);
				return this.validSubstructure;
			}
		}

		// Token: 0x17001F06 RID: 7942
		// (get) Token: 0x0600AD5D RID: 44381 RVA: 0x003D1875 File Offset: 0x003CFA75
		public HashSet<IntVec3> AllConnectedSubstructure
		{
			get
			{
				this.UpdateSubstructureIfNeeded(true);
				return this.allConnectedSubstructure;
			}
		}

		// Token: 0x17001F07 RID: 7943
		// (get) Token: 0x0600AD5E RID: 44382 RVA: 0x003D1884 File Offset: 0x003CFA84
		public HashSet<IntVec3> ValidSubstructureNoRegen
		{
			get
			{
				this.UpdateSubstructureIfNeeded(false);
				return this.validSubstructure;
			}
		}

		// Token: 0x17001F08 RID: 7944
		// (get) Token: 0x0600AD5F RID: 44383 RVA: 0x003D1893 File Offset: 0x003CFA93
		public HashSet<IntVec3> AllConnectedSubstructureNoRegen
		{
			get
			{
				this.UpdateSubstructureIfNeeded(false);
				return this.allConnectedSubstructure;
			}
		}

		// Token: 0x17001F09 RID: 7945
		// (get) Token: 0x0600AD60 RID: 44384 RVA: 0x003D18A4 File Offset: 0x003CFAA4
		public List<CompGravshipFacility> GravshipComponents
		{
			get
			{
				if (this.gravshipComponentsDirty)
				{
					this.gravshipComponentsDirty = false;
					this.gravshipComponents.Clear();
					using (List<Thing>.Enumerator enumerator = this.AffectedByFacilities.LinkedFacilitiesListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CompGravshipFacility item;
							if (enumerator.Current.TryGetComp(out item))
							{
								this.gravshipComponents.Add(item);
							}
						}
					}
				}
				return this.gravshipComponents;
			}
		}

		// Token: 0x17001F0A RID: 7946
		// (get) Token: 0x0600AD61 RID: 44385 RVA: 0x003D192C File Offset: 0x003CFB2C
		public float TotalFuel
		{
			get
			{
				float num = 0f;
				foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
				{
					CompRefuelable compRefuelable;
					if (compGravshipFacility.parent.Spawned && compGravshipFacility.Props.providesFuel && compGravshipFacility.CanBeActive && compGravshipFacility.parent.TryGetComp(out compRefuelable))
					{
						num += compRefuelable.Fuel;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F0B RID: 7947
		// (get) Token: 0x0600AD62 RID: 44386 RVA: 0x003D19BC File Offset: 0x003CFBBC
		public float MaxFuel
		{
			get
			{
				float num = 0f;
				foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
				{
					CompRefuelable compRefuelable;
					if (compGravshipFacility.parent.Spawned && compGravshipFacility.Props.providesFuel && compGravshipFacility.CanBeActive && compGravshipFacility.parent.TryGetComp(out compRefuelable))
					{
						num += compRefuelable.Props.fuelCapacity;
					}
				}
				return num;
			}
		}

		// Token: 0x17001F0C RID: 7948
		// (get) Token: 0x0600AD63 RID: 44387 RVA: 0x003D1A50 File Offset: 0x003CFC50
		public float FuelSavingsPercent
		{
			get
			{
				float num = 0f;
				foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
				{
					if (compGravshipFacility.parent.Spawned && compGravshipFacility.Props.fuelSavingsPercent > 0f && compGravshipFacility.CanBeActive)
					{
						num += compGravshipFacility.Props.fuelSavingsPercent;
					}
				}
				return Mathf.Clamp01(num);
			}
		}

		// Token: 0x17001F0D RID: 7949
		// (get) Token: 0x0600AD64 RID: 44388 RVA: 0x003D1AE0 File Offset: 0x003CFCE0
		public float FuelPerTile
		{
			get
			{
				return 10f * this.FuelUseageFactor;
			}
		}

		// Token: 0x17001F0E RID: 7950
		// (get) Token: 0x0600AD65 RID: 44389 RVA: 0x003D1AEE File Offset: 0x003CFCEE
		public float FuelUseageFactor
		{
			get
			{
				return 1f - this.FuelSavingsPercent;
			}
		}

		// Token: 0x17001F0F RID: 7951
		// (get) Token: 0x0600AD66 RID: 44390 RVA: 0x003D1AFC File Offset: 0x003CFCFC
		public List<GravshipComponentTypeDef> MissingComponents
		{
			get
			{
				if (this.missingComponentsDirty)
				{
					this.missingComponentsDirty = false;
					this.missingComponents.Clear();
					using (List<GravshipComponentTypeDef>.Enumerator enumerator = DefDatabase<GravshipComponentTypeDef>.AllDefsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GravshipComponentTypeDef def = enumerator.Current;
							if (def.requiredForLaunch && this.GravshipComponents.Find((CompGravshipFacility c) => c.Props.componentTypeDef == def) == null)
							{
								this.missingComponents.Add(def);
							}
						}
					}
				}
				return this.missingComponents;
			}
		}

		// Token: 0x17001F10 RID: 7952
		// (get) Token: 0x0600AD67 RID: 44391 RVA: 0x003D1BAC File Offset: 0x003CFDAC
		public bool HasSignalJammer
		{
			get
			{
				return this.GravshipComponents.Any((CompGravshipFacility c) => c.Props.componentTypeDef == GravshipComponentTypeDefOf.SignalJammer);
			}
		}

		// Token: 0x0600AD68 RID: 44392 RVA: 0x003D1BD8 File Offset: 0x003CFDD8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600AD69 RID: 44393 RVA: 0x003D1BF0 File Offset: 0x003CFDF0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.missingComponents.Clear();
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600AD6A RID: 44394 RVA: 0x003D1C0A File Offset: 0x003CFE0A
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600AD6B RID: 44395 RVA: 0x003D1C18 File Offset: 0x003CFE18
		protected override void Tick()
		{
			base.Tick();
			this.hasTickedThisSession = true;
			if (this.silentlyActivate)
			{
				this.Inspect(true);
				this.silentlyActivate = false;
			}
			if (Find.TickManager.TicksGame == this.cooldownCompleteTick)
			{
				Messages.Message("GravshipReadyToLaunch".Translate(this.RenamableLabel), this, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x0600AD6C RID: 44396 RVA: 0x003D1C88 File Offset: 0x003CFE88
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (base.Spawned)
			{
				if (Find.TickManager.TicksGame >= this.cooldownCompleteTick)
				{
					drawLoc.z += 0.5f * (1f + Mathf.Sin(6.2831855f * (float)GenTicks.TicksGame / 500f)) * 0.3f;
				}
				drawLoc.y += 0.03658537f;
				Vector3 s = new Vector3(this.def.graphicData.drawSize.x, 1f, this.def.graphicData.drawSize.y);
				Graphics.DrawMesh(MeshPool.plane10Back, Matrix4x4.TRS(drawLoc, base.Rotation.AsQuat, s), Building_GravEngine.OrbMat.Material, 0, null, 0);
			}
		}

		// Token: 0x0600AD6D RID: 44397 RVA: 0x003D1D60 File Offset: 0x003CFF60
		public void AddComponent(CompGravshipFacility comp)
		{
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600AD6E RID: 44398 RVA: 0x003D1D68 File Offset: 0x003CFF68
		public void RemoveComponent(CompGravshipFacility comp)
		{
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600AD6F RID: 44399 RVA: 0x003D1D70 File Offset: 0x003CFF70
		public AcceptanceReport CanLaunch(CompPilotConsole console)
		{
			Map map = base.Map;
			bool flag;
			if (map == null)
			{
				flag = false;
			}
			else
			{
				MapGeneratorDef generatorDef = map.generatorDef;
				bool? flag2;
				if (generatorDef == null)
				{
					flag2 = null;
				}
				else
				{
					PocketMapProperties pocketMapProperties = generatorDef.pocketMapProperties;
					flag2 = ((pocketMapProperties != null) ? new bool?(pocketMapProperties.canLaunchGravship) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = false;
				flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
			}
			if (flag)
			{
				return new AcceptanceReport(base.Map.generatorDef.label);
			}
			if (this.ValidSubstructure.Count == 0)
			{
				return new AcceptanceReport("CannotLaunchNoSubstructure".Translate().CapitalizeFirst());
			}
			if (!console.CanBeActive)
			{
				return new AcceptanceReport("CannotLaunchDisconnected".Translate().CapitalizeFirst());
			}
			if (this.TotalFuel < 10f)
			{
				return new AcceptanceReport("CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst());
			}
			if (this.MaxLaunchDistance <= 0)
			{
				return new AcceptanceReport("CannotLaunchNoThrusters".Translate().CapitalizeFirst());
			}
			if (GenTicks.TicksGame < this.cooldownCompleteTick)
			{
				return new AcceptanceReport("CannotLaunchOnCooldown".Translate((this.cooldownCompleteTick - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst());
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600AD70 RID: 44400 RVA: 0x003D1ED4 File Offset: 0x003D00D4
		public void ConsumeFuel(PlanetTile tile)
		{
			float num;
			int num2;
			if (!GravshipUtility.TryGetPathFuelCost(base.Map.Tile, tile, out num, out num2, 10f, 1f))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to get the fuel cost from tile (");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(base.Map.Tile);
				defaultInterpolatedStringHandler.AppendLiteral(") to ");
				defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(tile);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			float num3 = num / this.TotalFuel;
			foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
			{
				if (compGravshipFacility.CanBeActive && compGravshipFacility.Props.providesFuel)
				{
					CompRefuelable comp = compGravshipFacility.parent.GetComp<CompRefuelable>();
					if (comp != null)
					{
						comp.ConsumeFuel(comp.Fuel * num3);
					}
				}
			}
			int ticksGame = GenTicks.TicksGame;
			LaunchInfo launchInfo = this.launchInfo;
			this.cooldownCompleteTick = ticksGame + (int)GravshipUtility.LaunchCooldownFromQuality((launchInfo != null) ? launchInfo.quality : 1f);
		}

		// Token: 0x0600AD71 RID: 44401 RVA: 0x003D2004 File Offset: 0x003D0204
		public bool ValidSubstructureAt(IntVec3 cell)
		{
			return this.ValidSubstructure.Contains(cell);
		}

		// Token: 0x0600AD72 RID: 44402 RVA: 0x003D2014 File Offset: 0x003D0214
		public bool OnValidSubstructure(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			BuildingProperties building = thing.def.building;
			if (building != null && building.isAttachment)
			{
				Thing wallAttachedTo = GenConstruct.GetWallAttachedTo(thing);
				return this.ValidSubstructureAt(wallAttachedTo.Position);
			}
			foreach (IntVec3 cell in thing.OccupiedRect())
			{
				if (!this.ValidSubstructureAt(cell))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AD73 RID: 44403 RVA: 0x003D20AC File Offset: 0x003D02AC
		public bool LooselyConnectedToGravEngine(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			foreach (IntVec3 item in thing.OccupiedRect())
			{
				if (!this.AllConnectedSubstructure.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AD74 RID: 44404 RVA: 0x003D211C File Offset: 0x003D031C
		public void Inspect(bool silent = false)
		{
			if (Find.ResearchManager.gravEngineInspected)
			{
				return;
			}
			Find.ResearchManager.gravEngineInspected = true;
			QuestUtility.SendQuestTargetSignals(this.questTags, "Inspected", this.Named("SUBJECT"));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Gravship, OpportunityType.Important);
			if (base.Faction != Faction.OfPlayer)
			{
				this.SetFaction(Faction.OfPlayer, null);
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.GravEngine, StorytellerUtility.DefaultThreatPointsNow(base.Map));
			if (!silent)
			{
				DiaNode diaNode = new DiaNode("GravEngineInspectedLetterContents".Translate());
				DiaOption diaOption = new DiaOption("ViewQuest".Translate());
				diaOption.resolveTree = true;
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
				};
				diaNode.options.Add(diaOption);
				diaNode.options.Add(new DiaOption("Close".Translate())
				{
					resolveTree = true
				});
				Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
				dialog_NodeTree.forcePause = true;
				Find.WindowStack.Add(dialog_NodeTree);
			}
		}

		// Token: 0x0600AD75 RID: 44405 RVA: 0x003D2231 File Offset: 0x003D0431
		public void ForceSubstructureDirty()
		{
			this.substructureDirty = true;
			this.missingComponentsDirty = true;
			this.gravshipComponentsDirty = true;
		}

		// Token: 0x0600AD76 RID: 44406 RVA: 0x003D2248 File Offset: 0x003D0448
		private void UpdateSubstructureIfNeeded(bool regenerateSectionLayers)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (base.Destroyed || !base.Spawned)
			{
				return;
			}
			if (this.substructureDirty)
			{
				this.substructureDirty = false;
				GravshipUtility.GetConnectedSubstructure(this, this.allConnectedSubstructure, int.MaxValue, false);
				GravshipUtility.GetConnectedSubstructure(this, this.validSubstructure, (int)this.GetStatValue(StatDefOf.SubstructureSupport, true, -1), true);
				Map map = base.Map;
				if (map != null)
				{
					SubstructureGrid substructureGrid = map.substructureGrid;
					if (substructureGrid != null)
					{
						substructureGrid.Drawer.SetDirty();
					}
				}
				if (regenerateSectionLayers && base.Map != null && base.Map.mapDrawer != null)
				{
					base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipMask));
					base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipHull));
					base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_SubstructureProps));
				}
			}
			if (this.hasTickedThisSession && this.nameHidden && !this.haveShownNameDialog && Find.ResearchManager.gravEngineInspected && base.Faction == Faction.OfPlayer && this.validSubstructure.Count > 90)
			{
				Find.WindowStack.Add(new Dialog_NamePlayerGravship(this));
				this.haveShownNameDialog = true;
			}
		}

		// Token: 0x0600AD77 RID: 44407 RVA: 0x003D238C File Offset: 0x003D058C
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			AcceptanceReport result = base.ClaimableBy(by);
			if (!result.Accepted)
			{
				return result;
			}
			if (!Find.ResearchManager.gravEngineInspected)
			{
				return "GravEngineNotInspected".Translate();
			}
			return true;
		}

		// Token: 0x0600AD78 RID: 44408 RVA: 0x003D23D0 File Offset: 0x003D05D0
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!base.Spawned)
			{
				return text;
			}
			if (text.Length > 0)
			{
				text += "\n";
			}
			if (!Find.ResearchManager.gravEngineInspected)
			{
				text += "GravEngineNotInspected".Translate() + "\n";
			}
			text = string.Concat(new string[]
			{
				text,
				"ConnectedSubstructure".Translate() + ": ",
				this.AllConnectedSubstructure.Count.ToString(),
				" / ",
				StatDefOf.SubstructureSupport.ValueToString(this.GetStatValue(StatDefOf.SubstructureSupport, true, -1), ToStringNumberSense.Absolute, true)
			});
			int num = base.Map.listerThings.ThingsOfDef(ThingDefOf.Blueprint_Substructure).Count;
			num += base.Map.listerThings.ThingsOfDef(ThingDefOf.Frame_Substructure).Count;
			if (num > 0)
			{
				text = text + ("\n" + "SubstructureBlueprintsCount".Translate() + ": ") + num.ToString();
			}
			if (this.MissingComponents.Count > 0)
			{
				string[] array = new string[5];
				array[0] = text;
				array[1] = "\n";
				array[2] = "Requires".Translate().ToString();
				array[3] = ": ";
				array[4] = (from d in this.MissingComponents
				select d.label).ToCommaList(false, false).Colorize(ColorLibrary.RedReadable).CapitalizeFirst();
				text = string.Concat(array);
			}
			return text;
		}

		// Token: 0x0600AD79 RID: 44409 RVA: 0x003D258F File Offset: 0x003D078F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_GravEngine.<GetGizmos>d__79 <GetGizmos>d__ = new Building_GravEngine.<GetGizmos>d__79(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AD7A RID: 44410 RVA: 0x003D259F File Offset: 0x003D079F
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_GravEngine.<GetFloatMenuOptions>d__80 <GetFloatMenuOptions>d__ = new Building_GravEngine.<GetFloatMenuOptions>d__80(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AD7B RID: 44411 RVA: 0x003D25B6 File Offset: 0x003D07B6
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Building_GravEngine.<SpecialDisplayStats>d__81 <SpecialDisplayStats>d__ = new Building_GravEngine.<SpecialDisplayStats>d__81(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600AD7C RID: 44412 RVA: 0x003D25C6 File Offset: 0x003D07C6
		public IEnumerable<TaggedString> GetOrbitalWarnings()
		{
			Building_GravEngine.<GetOrbitalWarnings>d__83 <GetOrbitalWarnings>d__ = new Building_GravEngine.<GetOrbitalWarnings>d__83(-2);
			<GetOrbitalWarnings>d__.<>4__this = this;
			return <GetOrbitalWarnings>d__;
		}

		// Token: 0x0600AD7D RID: 44413 RVA: 0x003D25D8 File Offset: 0x003D07D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.cooldownCompleteTick, "cooldownCompleteTick", -1, false);
			Scribe_Values.Look<bool>(ref this.silentlyActivate, "silentlyActivate", false, false);
			Scribe_Values.Look<bool>(ref this.nameHidden, "nameHidden", true, false);
			Scribe_Values.Look<string>(ref this.gravshipName, "gravshipName", null, false);
			Scribe_Deep.Look<LaunchInfo>(ref this.launchInfo, "launchInfo", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsToBoard, "pawnsToBoard", LookMode.Reference);
			Scribe_Collections.Look<Pawn>(ref this.pawnsToLeave, "pawnsToLeave", LookMode.Reference);
		}

		// Token: 0x0600AD84 RID: 44420 RVA: 0x003D27FC File Offset: 0x003D09FC
		[CompilerGenerated]
		internal static bool <GetGizmos>g__ValidateInspectionTarget|79_0(LocalTargetInfo targ)
		{
			Pawn pawn = targ.Thing as Pawn;
			return pawn != null && pawn.IsColonistPlayerControlled && pawn.CanReach(pawn.Position, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !pawn.Downed && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving);
		}

		// Token: 0x0400647C RID: 25724
		public int cooldownCompleteTick = -1;

		// Token: 0x0400647D RID: 25725
		public bool silentlyActivate;

		// Token: 0x0400647E RID: 25726
		public bool nameHidden = true;

		// Token: 0x0400647F RID: 25727
		private string gravshipName = "Gravship".Translate();

		// Token: 0x04006480 RID: 25728
		public HashSet<Pawn> pawnsToBoard;

		// Token: 0x04006481 RID: 25729
		public HashSet<Pawn> pawnsToLeave;

		// Token: 0x04006482 RID: 25730
		public LaunchInfo launchInfo;

		// Token: 0x04006483 RID: 25731
		[Unsaved(false)]
		private CompAffectedByFacilities affectedByFacilities;

		// Token: 0x04006484 RID: 25732
		[Unsaved(false)]
		private List<CompGravshipFacility> gravshipComponents = new List<CompGravshipFacility>();

		// Token: 0x04006485 RID: 25733
		[Unsaved(false)]
		private HashSet<IntVec3> validSubstructure = new HashSet<IntVec3>();

		// Token: 0x04006486 RID: 25734
		[Unsaved(false)]
		private HashSet<IntVec3> allConnectedSubstructure = new HashSet<IntVec3>();

		// Token: 0x04006487 RID: 25735
		[Unsaved(false)]
		private List<GravshipComponentTypeDef> missingComponents = new List<GravshipComponentTypeDef>();

		// Token: 0x04006488 RID: 25736
		[Unsaved(false)]
		public bool substructureDirty = true;

		// Token: 0x04006489 RID: 25737
		[Unsaved(false)]
		private bool missingComponentsDirty = true;

		// Token: 0x0400648A RID: 25738
		[Unsaved(false)]
		private bool gravshipComponentsDirty = true;

		// Token: 0x0400648B RID: 25739
		[Unsaved(false)]
		private bool haveShownNameDialog;

		// Token: 0x0400648C RID: 25740
		[Unsaved(false)]
		private bool hasTickedThisSession;

		// Token: 0x0400648D RID: 25741
		private static readonly Texture2D InspectCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Inspect", true);

		// Token: 0x0400648E RID: 25742
		private static readonly CachedMaterial OrbMat = new CachedMaterial("Things/Building/GravEngine/GravEngine_Orb", ShaderDatabase.Cutout);

		// Token: 0x0400648F RID: 25743
		private static Graphic onCooldownGraphic;

		// Token: 0x04006490 RID: 25744
		private const float BaseFuelPerTile = 10f;

		// Token: 0x04006491 RID: 25745
		private const float BobHeight = 0.3f;

		// Token: 0x04006492 RID: 25746
		private const int NameDialogSubstructureThreshold = 90;

		// Token: 0x04006493 RID: 25747
		private HashSet<Room> tmpGravshipRooms = new HashSet<Room>();
	}
}
