using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023E0 RID: 9184
	[StaticConstructorOnStartup]
	public class WorldObject : IExposable, ILoadReferenceable, ISelectable
	{
		// Token: 0x17002851 RID: 10321
		// (get) Token: 0x0600E4FE RID: 58622 RVA: 0x00550D1E File Offset: 0x0054EF1E
		public List<WorldObjectComp> AllComps
		{
			get
			{
				return this.comps;
			}
		}

		// Token: 0x17002852 RID: 10322
		// (get) Token: 0x0600E4FF RID: 58623 RVA: 0x00550D26 File Offset: 0x0054EF26
		public virtual bool ShowRelatedQuests
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002853 RID: 10323
		// (get) Token: 0x0600E500 RID: 58624 RVA: 0x00550D29 File Offset: 0x0054EF29
		public virtual bool CanReformFoggedEnemies
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002854 RID: 10324
		// (get) Token: 0x0600E501 RID: 58625 RVA: 0x00550D2C File Offset: 0x0054EF2C
		public bool Destroyed
		{
			get
			{
				return this.destroyed;
			}
		}

		// Token: 0x17002855 RID: 10325
		// (get) Token: 0x0600E502 RID: 58626 RVA: 0x00550D34 File Offset: 0x0054EF34
		public virtual bool VisibleInBackground
		{
			get
			{
				return this.Tile.LayerDef.isSpace && Find.WorldSelector.SelectedLayer.Def.isSpace;
			}
		}

		// Token: 0x17002856 RID: 10326
		// (get) Token: 0x0600E503 RID: 58627 RVA: 0x00550D6C File Offset: 0x0054EF6C
		public virtual bool RequiresSignalJammerToReach
		{
			get
			{
				if (this.def.requiresSignalJammerToReach)
				{
					MapParent mapParent = this as MapParent;
					return mapParent == null || !mapParent.HasMap;
				}
				return false;
			}
		}

		// Token: 0x17002857 RID: 10327
		// (get) Token: 0x0600E504 RID: 58628 RVA: 0x00550D9E File Offset: 0x0054EF9E
		public virtual float DrawAltitude
		{
			get
			{
				return 0.03f;
			}
		}

		// Token: 0x17002858 RID: 10328
		// (get) Token: 0x0600E505 RID: 58629 RVA: 0x00550DA5 File Offset: 0x0054EFA5
		// (set) Token: 0x0600E506 RID: 58630 RVA: 0x00550DB0 File Offset: 0x0054EFB0
		public PlanetTile Tile
		{
			get
			{
				return this.tile;
			}
			set
			{
				if (this.tile == value)
				{
					return;
				}
				PlanetTile previous = this.tile;
				this.tile = value;
				if (this.Spawned && !this.def.useDynamicDrawer)
				{
					Find.World.renderer.Notify_StaticWorldObjectPosChanged();
				}
				if (previous.Valid)
				{
					previous.Tile.Layer.FastTileFinder.DirtyTile(previous);
				}
				this.tile.Tile.Layer.FastTileFinder.DirtyTile(this.tile);
				this.PositionChanged(previous, this.tile);
			}
		}

		// Token: 0x17002859 RID: 10329
		// (get) Token: 0x0600E507 RID: 58631 RVA: 0x00550E4B File Offset: 0x0054F04B
		public bool Spawned
		{
			get
			{
				return Find.WorldObjects.Contains(this);
			}
		}

		// Token: 0x1700285A RID: 10330
		// (get) Token: 0x0600E508 RID: 58632 RVA: 0x00550E58 File Offset: 0x0054F058
		public virtual Vector3 DrawPos
		{
			get
			{
				if (Find.TickManager.TicksGame != this.drawPosCacheTick)
				{
					this.drawPosCached = this.Tile.Layer.Origin + Find.WorldGrid.GetTileCenter(this.Tile);
					this.drawPosCacheTick = Find.TickManager.TicksGame;
				}
				return this.drawPosCached;
			}
		}

		// Token: 0x1700285B RID: 10331
		// (get) Token: 0x0600E509 RID: 58633 RVA: 0x00550EBB File Offset: 0x0054F0BB
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x1700285C RID: 10332
		// (get) Token: 0x0600E50A RID: 58634 RVA: 0x00550EC3 File Offset: 0x0054F0C3
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x1700285D RID: 10333
		// (get) Token: 0x0600E50B RID: 58635 RVA: 0x00550ED0 File Offset: 0x0054F0D0
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700285E RID: 10334
		// (get) Token: 0x0600E50C RID: 58636 RVA: 0x00550EE3 File Offset: 0x0054F0E3
		public virtual string LabelShort
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x1700285F RID: 10335
		// (get) Token: 0x0600E50D RID: 58637 RVA: 0x00550EEB File Offset: 0x0054F0EB
		public virtual string LabelShortCap
		{
			get
			{
				return this.LabelShort.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17002860 RID: 10336
		// (get) Token: 0x0600E50E RID: 58638 RVA: 0x00550EFE File Offset: 0x0054F0FE
		public virtual bool HasName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002861 RID: 10337
		// (get) Token: 0x0600E50F RID: 58639 RVA: 0x00550F01 File Offset: 0x0054F101
		public virtual Material Material
		{
			get
			{
				return this.def.Material;
			}
		}

		// Token: 0x17002862 RID: 10338
		// (get) Token: 0x0600E510 RID: 58640 RVA: 0x00550F10 File Offset: 0x0054F110
		public virtual Material ExpandingMaterial
		{
			get
			{
				if (this.def.expandingShader == null)
				{
					return null;
				}
				if (this.expandingMaterial != null)
				{
					return this.expandingMaterial;
				}
				MaterialRequest req = new MaterialRequest
				{
					mainTex = this.def.ExpandingIconTexture,
					shader = this.def.expandingShader.Shader,
					color = Color.white,
					maskTex = this.def.ExpandingIconTextureMask
				};
				return this.expandingMaterial = MaterialPool.MatFrom(req);
			}
		}

		// Token: 0x17002863 RID: 10339
		// (get) Token: 0x0600E511 RID: 58641 RVA: 0x00550FA1 File Offset: 0x0054F1A1
		public virtual bool SelectableNow
		{
			get
			{
				return this.def.selectable;
			}
		}

		// Token: 0x17002864 RID: 10340
		// (get) Token: 0x0600E512 RID: 58642 RVA: 0x00550FAE File Offset: 0x0054F1AE
		public virtual bool NeverMultiSelect
		{
			get
			{
				return this.def.neverMultiSelect;
			}
		}

		// Token: 0x17002865 RID: 10341
		// (get) Token: 0x0600E513 RID: 58643 RVA: 0x00550FBC File Offset: 0x0054F1BC
		public virtual Texture2D ExpandingIcon
		{
			get
			{
				if (this.def.ExpandingIconTexture != null)
				{
					return this.def.ExpandingIconTexture;
				}
				if (this.Material != null)
				{
					return (Texture2D)this.Material.mainTexture;
				}
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17002866 RID: 10342
		// (get) Token: 0x0600E514 RID: 58644 RVA: 0x0055100C File Offset: 0x0054F20C
		public virtual Color ExpandingIconColor
		{
			get
			{
				Color? expandingIconColor = this.def.expandingIconColor;
				if (expandingIconColor == null)
				{
					return this.Material.color;
				}
				return expandingIconColor.GetValueOrDefault();
			}
		}

		// Token: 0x17002867 RID: 10343
		// (get) Token: 0x0600E515 RID: 58645 RVA: 0x00551041 File Offset: 0x0054F241
		public virtual float ExpandingIconPriority
		{
			get
			{
				return this.def.expandingIconPriority;
			}
		}

		// Token: 0x17002868 RID: 10344
		// (get) Token: 0x0600E516 RID: 58646 RVA: 0x0055104E File Offset: 0x0054F24E
		public virtual bool ExpandMore
		{
			get
			{
				return this.def.expandMore;
			}
		}

		// Token: 0x17002869 RID: 10345
		// (get) Token: 0x0600E517 RID: 58647 RVA: 0x0055105B File Offset: 0x0054F25B
		public virtual bool AppendFactionToInspectString
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700286A RID: 10346
		// (get) Token: 0x0600E518 RID: 58648 RVA: 0x0055105E File Offset: 0x0054F25E
		public IThingHolder ParentHolder
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.World;
			}
		}

		// Token: 0x1700286B RID: 10347
		// (get) Token: 0x0600E519 RID: 58649 RVA: 0x0055106F File Offset: 0x0054F26F
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats
		{
			get
			{
				return Enumerable.Empty<StatDrawEntry>();
			}
		}

		// Token: 0x1700286C RID: 10348
		// (get) Token: 0x0600E51A RID: 58650 RVA: 0x00551076 File Offset: 0x0054F276
		public virtual BiomeDef Biome
		{
			get
			{
				if (!this.Spawned)
				{
					return null;
				}
				return Find.WorldGrid[this.Tile].PrimaryBiome;
			}
		}

		// Token: 0x1700286D RID: 10349
		// (get) Token: 0x0600E51B RID: 58651 RVA: 0x00551097 File Offset: 0x0054F297
		public virtual float ExpandingIconRotation
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700286E RID: 10350
		// (get) Token: 0x0600E51C RID: 58652 RVA: 0x0055109E File Offset: 0x0054F29E
		public virtual bool ExpandingIconFlipHorizontal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700286F RID: 10351
		// (get) Token: 0x0600E51D RID: 58653 RVA: 0x005510A1 File Offset: 0x0054F2A1
		public virtual bool GravShipCanLandOn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002870 RID: 10352
		// (get) Token: 0x0600E51E RID: 58654 RVA: 0x005510A4 File Offset: 0x0054F2A4
		protected virtual int UpdateRateTicks
		{
			get
			{
				if (!WorldRendererUtility.WorldSelected)
				{
					return 15;
				}
				return 1;
			}
		}

		// Token: 0x17002871 RID: 10353
		// (get) Token: 0x0600E51F RID: 58655 RVA: 0x005510B1 File Offset: 0x0054F2B1
		protected virtual int UpdateRateTickOffset
		{
			get
			{
				return this.HashOffset();
			}
		}

		// Token: 0x0600E520 RID: 58656 RVA: 0x005510B9 File Offset: 0x0054F2B9
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			WorldObject.<IncidentTargetTags>d__85 <IncidentTargetTags>d__ = new WorldObject.<IncidentTargetTags>d__85(-2);
			<IncidentTargetTags>d__.<>4__this = this;
			return <IncidentTargetTags>d__;
		}

		// Token: 0x0600E521 RID: 58657 RVA: 0x005510CC File Offset: 0x0054F2CC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<WorldObjectDef>(ref this.def, "def");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			Scribe_Values.Look<PlanetTile>(ref this.tile, "tile", default(PlanetTile), false);
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.creationGameTicks, "creationGameTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyed, "destroyed", false, false);
			Scribe_Values.Look<int>(ref this.tickDelta, "tickDelta", 0, false);
			Scribe_Values.Look<bool>(ref this.isGeneratedLocation, "isGeneratedLocation", false, false);
			Scribe_References.Look<Faction>(ref this.factionInt, "faction", false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Deep.Look<ThingOwner<Thing>>(ref this.rewards, "rewards", Array.Empty<object>());
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostExposeData();
			}
		}

		// Token: 0x0600E522 RID: 58658 RVA: 0x005511D8 File Offset: 0x0054F3D8
		private void InitializeComps()
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				WorldObjectComp worldObjectComp = null;
				try
				{
					worldObjectComp = (WorldObjectComp)Activator.CreateInstance(this.def.comps[i].compClass);
					worldObjectComp.parent = this;
					this.comps.Add(worldObjectComp);
					worldObjectComp.Initialize(this.def.comps[i]);
				}
				catch (Exception ex)
				{
					string str = "Could not instantiate or initialize a WorldObjectComp: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					this.comps.Remove(worldObjectComp);
				}
			}
		}

		// Token: 0x0600E523 RID: 58659 RVA: 0x00551290 File Offset: 0x0054F490
		public virtual void SetFaction(Faction newFaction)
		{
			if (!this.def.canHaveFaction && newFaction != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to set faction to ",
					(newFaction != null) ? newFaction.ToString() : null,
					" but this world object (",
					(this != null) ? this.ToString() : null,
					") cannot have faction."
				}));
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x0600E524 RID: 58660 RVA: 0x00551300 File Offset: 0x0054F500
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (ModsConfig.OdysseyActive && this.def.requiresSignalJammerToReach)
			{
				stringBuilder.Append("RequiresSignalJammer".Translate());
			}
			if (this.Faction != null && this.AppendFactionToInspectString)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Faction".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(this.Faction.Name);
				stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode)
					{
						string text2 = text;
						if (char.IsWhiteSpace(text2[text2.Length - 1]))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(47, 2);
							defaultInterpolatedStringHandler2.AppendFormatted<Type>(this.comps[i].GetType());
							defaultInterpolatedStringHandler2.AppendLiteral(" CompInspectStringExtra ended with whitespace: ");
							defaultInterpolatedStringHandler2.AppendFormatted(text);
							Log.ErrorOnce(defaultInterpolatedStringHandler2.ToStringAndClear(), 25612);
							text = text.TrimEndNewlines();
						}
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x0600E525 RID: 58661 RVA: 0x0055146C File Offset: 0x0054F66C
		public void DoTick()
		{
			using (ProfilerBlock.Scope("DoTick()"))
			{
				using (ProfilerBlock.Scope("Tick()"))
				{
					this.Tick();
				}
				this.tickDelta++;
				if (this.tickDelta > this.UpdateRateTicks || GenTicks.IsTickInterval(this.UpdateRateTickOffset, this.UpdateRateTicks))
				{
					using (ProfilerBlock.Scope("TickInterval()"))
					{
						this.TickInterval(this.tickDelta);
					}
					this.tickDelta = 0;
				}
			}
			if (this.Destroyed)
			{
				return;
			}
			if (!this.cached)
			{
				this.cached = true;
				this.cachedHolder = (this as IThingHolder);
				this.cachedTickable = (this as IThingHolderTickable);
				this.cachedIsHolder = (this.cachedHolder != null);
			}
			if (!this.cachedIsHolder)
			{
				return;
			}
			if (this.cachedTickable == null || this.cachedTickable.ShouldTickContents)
			{
				if (this.tmpHolders == null)
				{
					this.tmpHolders = new List<IThingHolder>(8);
				}
				this.tmpHolders.Add(this.cachedHolder);
				this.cachedHolder.GetChildHolders(this.tmpHolders);
				for (int i = 0; i < this.tmpHolders.Count; i++)
				{
					ThingOwner directlyHeldThings = this.tmpHolders[i].GetDirectlyHeldThings();
					if (directlyHeldThings != null)
					{
						IThingHolder owner = directlyHeldThings.Owner;
						if (!(owner is Map) && !(owner is Caravan))
						{
							directlyHeldThings.DoTick();
							if (this.Destroyed)
							{
								break;
							}
						}
					}
				}
				this.tmpHolders.Clear();
			}
		}

		// Token: 0x0600E526 RID: 58662 RVA: 0x00551630 File Offset: 0x0054F830
		protected virtual void Tick()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTick();
			}
		}

		// Token: 0x0600E527 RID: 58663 RVA: 0x00551664 File Offset: 0x0054F864
		protected virtual void TickInterval(int delta)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTickInterval(delta);
			}
		}

		// Token: 0x0600E528 RID: 58664 RVA: 0x00551699 File Offset: 0x0054F899
		public virtual void ExtraSelectionOverlaysOnGUI()
		{
		}

		// Token: 0x0600E529 RID: 58665 RVA: 0x0055169C File Offset: 0x0054F89C
		public virtual void DrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDrawExtraSelectionOverlays();
			}
		}

		// Token: 0x0600E52A RID: 58666 RVA: 0x005516D0 File Offset: 0x0054F8D0
		public virtual void PostMake()
		{
			this.InitializeComps();
		}

		// Token: 0x0600E52B RID: 58667 RVA: 0x005516D8 File Offset: 0x0054F8D8
		public virtual void PostAdd()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
		}

		// Token: 0x0600E52C RID: 58668 RVA: 0x005516F5 File Offset: 0x0054F8F5
		protected virtual void PositionChanged(PlanetTile previous, PlanetTile current)
		{
		}

		// Token: 0x0600E52D RID: 58669 RVA: 0x005516F8 File Offset: 0x0054F8F8
		public virtual void SpawnSetup()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.RegisterDrawable(this);
			}
			this.Tile.Layer.FastTileFinder.DirtyTile(this.Tile);
		}

		// Token: 0x0600E52E RID: 58670 RVA: 0x00551758 File Offset: 0x0054F958
		public virtual void PostRemove()
		{
			if (!this.def.useDynamicDrawer)
			{
				Find.World.renderer.Notify_StaticWorldObjectPosChanged();
			}
			if (this.def.useDynamicDrawer)
			{
				Find.WorldDynamicDrawManager.DeRegisterDrawable(this);
			}
			Find.WorldSelector.Deselect(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostPostRemove();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
			this.Tile.Layer.FastTileFinder.DirtyTile(this.Tile);
		}

		// Token: 0x0600E52F RID: 58671 RVA: 0x00551804 File Offset: 0x0054FA04
		public virtual void Destroy()
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed world object " + ((this != null) ? this.ToString() : null));
				return;
			}
			if (this.Spawned)
			{
				Find.WorldObjects.Remove(this);
			}
			this.destroyed = true;
			Find.FactionManager.Notify_WorldObjectDestroyed(this);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].PostDestroy();
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
		}

		// Token: 0x0600E530 RID: 58672 RVA: 0x005518A0 File Offset: 0x0054FAA0
		public virtual void Print(LayerSubMesh subMesh)
		{
			float averageTileSize = this.Tile.Layer.AverageTileSize;
			float num = Rand.RangeSeeded(0f, 0.01f, this.ID) + this.def.drawAltitudeOffset;
			WorldRendererUtility.PrintQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, 0.03f + num, subMesh, false, Rand.Range(0f, 360f), true);
		}

		// Token: 0x0600E531 RID: 58673 RVA: 0x00551910 File Offset: 0x0054FB10
		public virtual void Draw()
		{
			float averageTileSize = this.Tile.Layer.AverageTileSize;
			float rawTransitionPct = ExpandableWorldObjectsUtility.RawTransitionPct;
			if (this.Tile.LayerDef.isSpace)
			{
				return;
			}
			if (!this.Material)
			{
				return;
			}
			float num = Rand.RangeSeeded(0f, 0.01f, this.ID) + this.def.drawAltitudeOffset;
			if (this.def.expandingIcon && rawTransitionPct > 0f && !ExpandableWorldObjectsUtility.HiddenByRules(this))
			{
				Color color = this.Material.color;
				float num2 = 1f - rawTransitionPct;
				WorldObject.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * num2));
				WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, this.DrawAltitude + num, this.Material, 0f, false, false, WorldObject.propertyBlock);
				return;
			}
			WorldRendererUtility.DrawQuadTangentialToPlanet(this.DrawPos, 0.7f * averageTileSize, this.DrawAltitude + num, this.Material, 0f, false, false, null);
		}

		// Token: 0x0600E532 RID: 58674 RVA: 0x00551A3C File Offset: 0x0054FC3C
		public T GetComponent<T>() where T : WorldObjectComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				T t = this.comps[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600E533 RID: 58675 RVA: 0x00551A89 File Offset: 0x0054FC89
		public bool TryGetComponent<T>(out T comp) where T : WorldObjectComp
		{
			comp = this.GetComponent<T>();
			return comp != null;
		}

		// Token: 0x0600E534 RID: 58676 RVA: 0x00551AA8 File Offset: 0x0054FCA8
		public WorldObjectComp GetComponent(Type type)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (type.IsInstanceOfType(this.comps[i]))
				{
					return this.comps[i];
				}
			}
			return null;
		}

		// Token: 0x0600E535 RID: 58677 RVA: 0x00551AED File Offset: 0x0054FCED
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			WorldObject.<GetGizmos>d__111 <GetGizmos>d__ = new WorldObject.<GetGizmos>d__111(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E536 RID: 58678 RVA: 0x00551AFD File Offset: 0x0054FCFD
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			WorldObject.<GetCaravanGizmos>d__112 <GetCaravanGizmos>d__ = new WorldObject.<GetCaravanGizmos>d__112(-2);
			<GetCaravanGizmos>d__.<>4__this = this;
			<GetCaravanGizmos>d__.<>3__caravan = caravan;
			return <GetCaravanGizmos>d__;
		}

		// Token: 0x0600E537 RID: 58679 RVA: 0x00551B14 File Offset: 0x0054FD14
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			WorldObject.<GetFloatMenuOptions>d__113 <GetFloatMenuOptions>d__ = new WorldObject.<GetFloatMenuOptions>d__113(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__caravan = caravan;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600E538 RID: 58680 RVA: 0x00551B2B File Offset: 0x0054FD2B
		public virtual IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x0600E539 RID: 58681 RVA: 0x00551B32 File Offset: 0x0054FD32
		public virtual IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x0600E53A RID: 58682 RVA: 0x00551B39 File Offset: 0x0054FD39
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x0600E53B RID: 58683 RVA: 0x00551B46 File Offset: 0x0054FD46
		public virtual bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			return this.Faction == other.Faction;
		}

		// Token: 0x0600E53C RID: 58684 RVA: 0x00551B58 File Offset: 0x0054FD58
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" ",
				this.LabelCap,
				" (tile=",
				this.Tile.ToString(),
				")"
			});
		}

		// Token: 0x0600E53D RID: 58685 RVA: 0x00551BB6 File Offset: 0x0054FDB6
		public override int GetHashCode()
		{
			return this.ID;
		}

		// Token: 0x0600E53E RID: 58686 RVA: 0x00551BBE File Offset: 0x0054FDBE
		public string GetUniqueLoadID()
		{
			return "WorldObject_" + this.ID.ToString();
		}

		// Token: 0x0600E53F RID: 58687 RVA: 0x00551BD8 File Offset: 0x0054FDD8
		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.def.description);
			for (int i = 0; i < this.comps.Count; i++)
			{
				string descriptionPart = this.comps[i].GetDescriptionPart();
				if (!descriptionPart.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(descriptionPart);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400917C RID: 37244
		public WorldObjectDef def;

		// Token: 0x0400917D RID: 37245
		public int ID = -1;

		// Token: 0x0400917E RID: 37246
		private PlanetTile tile = PlanetTile.Invalid;

		// Token: 0x0400917F RID: 37247
		private Faction factionInt;

		// Token: 0x04009180 RID: 37248
		public int creationGameTicks = -1;

		// Token: 0x04009181 RID: 37249
		public List<string> questTags;

		// Token: 0x04009182 RID: 37250
		private bool destroyed;

		// Token: 0x04009183 RID: 37251
		public ThingOwner<Thing> rewards;

		// Token: 0x04009184 RID: 37252
		private int tickDelta;

		// Token: 0x04009185 RID: 37253
		public bool isGeneratedLocation;

		// Token: 0x04009186 RID: 37254
		private List<WorldObjectComp> comps = new List<WorldObjectComp>();

		// Token: 0x04009187 RID: 37255
		private Material expandingMaterial;

		// Token: 0x04009188 RID: 37256
		private int drawPosCacheTick = -1;

		// Token: 0x04009189 RID: 37257
		private Vector3 drawPosCached;

		// Token: 0x0400918A RID: 37258
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400918B RID: 37259
		private const float BaseDrawSize = 0.7f;

		// Token: 0x0400918C RID: 37260
		private const float DrawOffsetRange = 0.01f;

		// Token: 0x0400918D RID: 37261
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);

		// Token: 0x0400918E RID: 37262
		private List<IThingHolder> tmpHolders;

		// Token: 0x0400918F RID: 37263
		private bool cached;

		// Token: 0x04009190 RID: 37264
		private bool cachedIsHolder;

		// Token: 0x04009191 RID: 37265
		private IThingHolder cachedHolder;

		// Token: 0x04009192 RID: 37266
		private IThingHolderTickable cachedTickable;
	}
}
