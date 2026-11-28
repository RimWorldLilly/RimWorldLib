using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200059A RID: 1434
	public class Thing : Entity, ISelectable, ILoadReferenceable, ISignalReceiver, IExposable, IEquatable<Thing>
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002A43 RID: 10819 RVA: 0x001027EC File Offset: 0x001009EC
		// (set) Token: 0x06002A44 RID: 10820 RVA: 0x001027F4 File Offset: 0x001009F4
		public virtual int HitPoints
		{
			get
			{
				return this.hitPointsInt;
			}
			set
			{
				this.hitPointsInt = value;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002A45 RID: 10821 RVA: 0x001027FD File Offset: 0x001009FD
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.GetStatValue(StatDefOf.MaxHitPoints, true, 10));
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x00102812 File Offset: 0x00100A12
		public virtual float MarketValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.MarketValue, true, -1);
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002A47 RID: 10823 RVA: 0x00102821 File Offset: 0x00100A21
		public virtual float RoyalFavorValue
		{
			get
			{
				return this.GetStatValue(StatDefOf.RoyalFavorValue, true, -1);
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x00102830 File Offset: 0x00100A30
		public virtual int? OverrideGraphicIndex
		{
			get
			{
				return this.overrideGraphicIndex;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002A49 RID: 10825 RVA: 0x00102838 File Offset: 0x00100A38
		public virtual Texture UIIconOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x0010283B File Offset: 0x00100A3B
		// (set) Token: 0x06002A4B RID: 10827 RVA: 0x00102843 File Offset: 0x00100A43
		public bool EverSeenByPlayer
		{
			get
			{
				return this.GetEverSeenByPlayer();
			}
			set
			{
				this.SetEverSeenByPlayer(value);
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x0010284C File Offset: 0x00100A4C
		// (set) Token: 0x06002A4D RID: 10829 RVA: 0x00102854 File Offset: 0x00100A54
		public virtual ThingStyleDef StyleDef
		{
			get
			{
				return this.GetStyleDef();
			}
			set
			{
				this.styleGraphicInt = null;
				this.SetStyleDef(value);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002A4E RID: 10830 RVA: 0x00102864 File Offset: 0x00100A64
		// (set) Token: 0x06002A4F RID: 10831 RVA: 0x0010286C File Offset: 0x00100A6C
		public Precept_ThingStyle StyleSourcePrecept
		{
			get
			{
				return this.GetStyleSourcePrecept();
			}
			set
			{
				this.SetStyleSourcePrecept(value);
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x00102878 File Offset: 0x00100A78
		public bool FlammableNow
		{
			get
			{
				if (this.GetStatValue(StatDefOf.Flammability, true, -1) < 0.01f)
				{
					return false;
				}
				if (this.Spawned && !this.FireBulwark)
				{
					List<Thing> thingList = this.Position.GetThingList(this.Map);
					if (thingList != null)
					{
						for (int i = 0; i < thingList.Count; i++)
						{
							if (thingList[i].FireBulwark)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06002A51 RID: 10833 RVA: 0x001028E2 File Offset: 0x00100AE2
		public virtual bool FireBulwark
		{
			get
			{
				return this.def.Fillage == FillCategory.Full;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x001028F2 File Offset: 0x00100AF2
		public bool Destroyed
		{
			get
			{
				return this.mapIndexOrState == -2 || this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06002A53 RID: 10835 RVA: 0x0010290A File Offset: 0x00100B0A
		public bool Discarded
		{
			get
			{
				return this.mapIndexOrState == -3;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x00102918 File Offset: 0x00100B18
		public bool Spawned
		{
			get
			{
				if (this.mapIndexOrState < 0 || Find.Maps == null)
				{
					return false;
				}
				if ((int)this.mapIndexOrState < Find.Maps.Count)
				{
					return true;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Thing ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ThingID);
				defaultInterpolatedStringHandler.AppendLiteral(" is associated with invalid map index ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.mapIndexOrState);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 64664487);
				return false;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002A55 RID: 10837 RVA: 0x00102999 File Offset: 0x00100B99
		public bool SpawnedOrAnyParentSpawned
		{
			get
			{
				return this.SpawnedParentOrMe != null;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x001029A4 File Offset: 0x00100BA4
		public Thing SpawnedParentOrMe
		{
			get
			{
				if (this.Spawned)
				{
					return this;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				}
				return null;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x001029C5 File Offset: 0x00100BC5
		public int TickSpawned
		{
			get
			{
				return this.spawnedTick;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x001029CD File Offset: 0x00100BCD
		public int TickDeSpawned
		{
			get
			{
				return this.despawnedTick;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002A59 RID: 10841 RVA: 0x001029D5 File Offset: 0x00100BD5
		public Map Map
		{
			get
			{
				if (this.mapIndexOrState < 0)
				{
					return null;
				}
				List<Map> maps = Find.Maps;
				if (maps == null)
				{
					return null;
				}
				return maps[(int)this.mapIndexOrState];
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002A5A RID: 10842 RVA: 0x001029F8 File Offset: 0x00100BF8
		public Map MapHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map;
				}
				if (this.ParentHolder == null)
				{
					return null;
				}
				return ThingOwnerUtility.GetRootMap(this.ParentHolder);
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x00102A1E File Offset: 0x00100C1E
		// (set) Token: 0x06002A5C RID: 10844 RVA: 0x00102A28 File Offset: 0x00100C28
		public IntVec3 Position
		{
			get
			{
				return this.positionInt;
			}
			set
			{
				if (value == this.positionInt)
				{
					return;
				}
				if (this.Spawned)
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed position of a spawned thing which affects regions. This is not supported.");
					}
					this.DirtyMapMesh(this.Map);
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
					this.Map.coverGrid.DeRegister(this);
				}
				this.positionInt = value;
				if (this.Spawned)
				{
					this.Map.thingGrid.Register(this);
					this.Map.coverGrid.Register(this);
					this.Map.gasGrid.Notify_ThingSpawned(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					this.DirtyMapMesh(this.Map);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x00102B18 File Offset: 0x00100D18
		public IntVec3 PositionHeld
		{
			get
			{
				if (this.Spawned)
				{
					return this.Position;
				}
				IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.ParentHolder);
				if (rootPosition.IsValid)
				{
					return rootPosition;
				}
				return this.Position;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002A5E RID: 10846 RVA: 0x00102B51 File Offset: 0x00100D51
		// (set) Token: 0x06002A5F RID: 10847 RVA: 0x00102B5C File Offset: 0x00100D5C
		public Rot4 Rotation
		{
			get
			{
				return this.rotationInt;
			}
			set
			{
				if (value == this.rotationInt || this.debugRotLocked)
				{
					return;
				}
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					if (this.def.AffectsRegions)
					{
						Log.Warning("Changed rotation of a spawned non-single-cell thing which affects regions. This is not supported.");
					}
					RegionListersUpdater.DeregisterInRegions(this, this.Map);
					this.Map.thingGrid.Deregister(this, false);
				}
				this.rotationInt = value;
				if (this.Spawned && (this.def.size.x != 1 || this.def.size.z != 1))
				{
					this.Map.thingGrid.Register(this);
					RegionListersUpdater.RegisterInRegions(this, this.Map);
					this.Map.gasGrid.Notify_ThingSpawned(this);
					if (this.def.AffectsReachability)
					{
						this.Map.reachability.ClearCache();
					}
				}
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002A60 RID: 10848 RVA: 0x00102C63 File Offset: 0x00100E63
		public bool Smeltable
		{
			get
			{
				return !this.IsRelic() && this.def.smeltable && (!this.def.MadeFromStuff || this.Stuff.smeltable);
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x00102C98 File Offset: 0x00100E98
		public bool BurnableByRecipe
		{
			get
			{
				return this.def.burnableByRecipe && (!this.def.MadeFromStuff || this.Stuff.burnableByRecipe);
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x00102CC3 File Offset: 0x00100EC3
		public IThingHolder ParentHolder
		{
			get
			{
				ThingOwner thingOwner = this.holdingOwner;
				if (thingOwner == null)
				{
					return null;
				}
				return thingOwner.Owner;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002A63 RID: 10851 RVA: 0x00102CD6 File Offset: 0x00100ED6
		public Faction Faction
		{
			get
			{
				return this.factionInt;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x00102CDE File Offset: 0x00100EDE
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x00102D14 File Offset: 0x00100F14
		public string ThingID
		{
			get
			{
				if (this.def.HasThingIDNumber)
				{
					return this.def.defName + this.thingIDNumber.ToString();
				}
				return this.def.defName;
			}
			set
			{
				this.thingIDNumber = Thing.IDNumberFromThingID(value);
			}
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x00102D24 File Offset: 0x00100F24
		public static int IDNumberFromThingID(string thingID)
		{
			string value = Regex.Match(thingID, "\\d+$").Value;
			int result = 0;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				result = Convert.ToInt32(value, invariantCulture);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Could not convert id number from thingID=";
				array[1] = thingID;
				array[2] = ", numString=";
				array[3] = value;
				array[4] = " Exception=";
				int num = 5;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
			return result;
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x00102DA8 File Offset: 0x00100FA8
		public IntVec2 RotatedSize
		{
			get
			{
				if (!this.rotationInt.IsHorizontal)
				{
					return this.def.size;
				}
				return new IntVec2(this.def.size.z, this.def.size.x);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x00102DE8 File Offset: 0x00100FE8
		public virtual CellRect? CustomRectForSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x00102DFE File Offset: 0x00100FFE
		public override string Label
		{
			get
			{
				if (this.stackCount > 1)
				{
					return this.LabelNoCount + " x" + this.stackCount.ToStringCached();
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x00102E2B File Offset: 0x0010102B
		public virtual string LabelNoCount
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, true, true);
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x00102E36 File Offset: 0x00101036
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x00102E49 File Offset: 0x00101049
		public virtual string LabelCapNoCount
		{
			get
			{
				return this.LabelNoCount.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x00102E5C File Offset: 0x0010105C
		public override string LabelShort
		{
			get
			{
				return this.LabelNoCount;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002A6E RID: 10862 RVA: 0x00102E64 File Offset: 0x00101064
		public virtual string LabelNoParenthesis
		{
			get
			{
				return GenLabel.ThingLabel(this, 1, false, false);
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x00102E6F File Offset: 0x0010106F
		public string LabelNoParenthesisCap
		{
			get
			{
				return this.LabelNoParenthesis.CapitalizeFirst();
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x00102E7C File Offset: 0x0010107C
		public virtual ModContentPack ContentSource
		{
			get
			{
				return this.def.modContentPack;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x00102E89 File Offset: 0x00101089
		public virtual bool IngestibleNow
		{
			get
			{
				return !this.IsBurning() && this.def.IsIngestible;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x00102EA0 File Offset: 0x001010A0
		public ThingDef Stuff
		{
			get
			{
				return this.stuffInt;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002A73 RID: 10867 RVA: 0x00102EA8 File Offset: 0x001010A8
		public Graphic DefaultGraphic
		{
			get
			{
				if (this.graphicInt == null)
				{
					if (this.def.graphicData == null)
					{
						return BaseContent.BadGraphic;
					}
					this.graphicInt = this.def.graphicData.GraphicColoredFor(this);
				}
				return this.graphicInt;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06002A74 RID: 10868 RVA: 0x00102EE4 File Offset: 0x001010E4
		public virtual Graphic Graphic
		{
			get
			{
				ThingStyleDef styleDef = this.StyleDef;
				if (((styleDef != null) ? styleDef.Graphic : null) != null)
				{
					if (this.styleGraphicInt == null)
					{
						if (styleDef.graphicData != null)
						{
							this.styleGraphicInt = styleDef.graphicData.GraphicColoredFor(this);
						}
						else
						{
							this.styleGraphicInt = styleDef.Graphic;
						}
					}
					return this.styleGraphicInt;
				}
				return this.DefaultGraphic;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002A75 RID: 10869 RVA: 0x00102F43 File Offset: 0x00101143
		public virtual List<IntVec3> InteractionCells
		{
			get
			{
				return ThingUtility.InteractionCellsWhenAt(this.def, this.Position, this.Rotation, this.Map, true);
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06002A76 RID: 10870 RVA: 0x00102F63 File Offset: 0x00101163
		public virtual IntVec3 InteractionCell
		{
			get
			{
				return ThingUtility.InteractionCellWhenAt(this.def, this.Position, this.Rotation, this.Map);
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002A77 RID: 10871 RVA: 0x00102F84 File Offset: 0x00101184
		public float AmbientTemperature
		{
			get
			{
				if (this.Spawned)
				{
					return GenTemperature.GetTemperatureForCell(this.Position, this.Map);
				}
				if (this.ParentHolder != null)
				{
					for (IThingHolder parentHolder = this.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
					{
						float result;
						if (ThingOwnerUtility.TryGetFixedTemperature(parentHolder, this, out result))
						{
							return result;
						}
					}
				}
				if (this.SpawnedOrAnyParentSpawned)
				{
					return GenTemperature.GetTemperatureForCell(this.PositionHeld, this.MapHeld);
				}
				if (this.Tile.Valid)
				{
					return GenTemperature.GetTemperatureAtTile(this.Tile);
				}
				return 21f;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06002A78 RID: 10872 RVA: 0x0010300E File Offset: 0x0010120E
		public PlanetTile Tile
		{
			get
			{
				if (this.Spawned)
				{
					return this.Map.Tile;
				}
				if (this.ParentHolder != null)
				{
					return ThingOwnerUtility.GetRootTile(this.ParentHolder);
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002A79 RID: 10873 RVA: 0x0010303D File Offset: 0x0010123D
		public virtual bool Suspended
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsSuspended(this.ParentHolder);
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x0010305E File Offset: 0x0010125E
		public bool InCryptosleep
		{
			get
			{
				return !this.Spawned && this.ParentHolder != null && ThingOwnerUtility.ContentsInCryptosleep(this.ParentHolder);
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06002A7B RID: 10875 RVA: 0x0010307F File Offset: 0x0010127F
		public virtual string DescriptionDetailed
		{
			get
			{
				return this.def.DescriptionDetailed;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x0010308C File Offset: 0x0010128C
		public virtual string DescriptionFlavor
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002A7D RID: 10877 RVA: 0x00103099 File Offset: 0x00101299
		public bool IsOnHoldingPlatform
		{
			get
			{
				return ModsConfig.AnomalyActive && this.ParentHolder is Building_HoldingPlatform;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x001030B2 File Offset: 0x001012B2
		public TerrainAffordanceDef TerrainAffordanceNeeded
		{
			get
			{
				return this.def.GetTerrainAffordanceNeed(this.stuffInt);
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x001030C5 File Offset: 0x001012C5
		public bool BeingTransportedOnGravship
		{
			get
			{
				return this.beingTransportedOnGravship;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x001030CD File Offset: 0x001012CD
		protected virtual int MinTickIntervalRate
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002A81 RID: 10881 RVA: 0x001030D0 File Offset: 0x001012D0
		protected virtual int MaxTickIntervalRate
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002A82 RID: 10882 RVA: 0x001030D4 File Offset: 0x001012D4
		protected virtual int UpdateRateTickOffset
		{
			get
			{
				return this.HashOffset();
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x001030DC File Offset: 0x001012DC
		public virtual int UpdateRateTicks
		{
			get
			{
				return GenTicks.GetCameraUpdateRate(this);
			}
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x001030E4 File Offset: 0x001012E4
		public void DoTick()
		{
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
			bool flag = this.holdingOwner != null && !this.Spawned;
			if (this.def.tickerType == TickerType.Normal)
			{
				using (ProfilerBlock.Scope("DoTick()"))
				{
					using (ProfilerBlock.Scope("Tick()"))
					{
						this.Tick();
					}
					if (this.Destroyed)
					{
						return;
					}
					this.tickDelta++;
					int num = Mathf.Min(Mathf.Max(this.UpdateRateTicks, this.MinTickIntervalRate), this.MaxTickIntervalRate);
					if (this.tickDelta >= num || GenTicks.IsTickInterval(this.UpdateRateTickOffset, num))
					{
						using (ProfilerBlock.Scope("TickInterval()"))
						{
							this.TickInterval(this.tickDelta);
						}
						this.tickDelta = 0;
					}
					goto IL_1AF;
				}
			}
			if (this.def.tickerType == TickerType.Rare && ((!this.cachedIsHolder && !flag) || this.IsHashIntervalTick(250)))
			{
				using (ProfilerBlock.Scope("TickRare()"))
				{
					this.TickRare();
					goto IL_1AF;
				}
			}
			if (this.def.tickerType == TickerType.Long && ((!this.cachedIsHolder && !flag) || this.IsHashIntervalTick(2000)))
			{
				using (ProfilerBlock.Scope("TickLong()"))
				{
					this.TickLong();
				}
			}
			IL_1AF:
			if (this.Destroyed)
			{
				return;
			}
			if (!this.cachedIsHolder)
			{
				return;
			}
			if ((this.cachedTickable == null || this.cachedTickable.ShouldTickContents) && !ThingOwnerUtility.ContentsSuspended(this.cachedHolder))
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
						directlyHeldThings.DoTick();
						if (this.Destroyed)
						{
							break;
						}
					}
				}
				this.tmpHolders.Clear();
			}
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x0010339C File Offset: 0x0010159C
		public virtual void PostMake()
		{
			ThingIDMaker.GiveIDTo(this);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.RoundToInt((float)this.MaxHitPoints * Mathf.Clamp01(this.def.startingHpRange.RandomInRange));
			}
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x001033DC File Offset: 0x001015DC
		public virtual void PostPostMake()
		{
			if (!this.def.randomStyle.NullOrEmpty<ThingStyleChance>() && Rand.Chance(this.def.randomStyleChance))
			{
				this.StyleDef = this.def.randomStyle.RandomElementByWeight((ThingStyleChance x) => x.Chance).StyleDef;
			}
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x00103447 File Offset: 0x00101647
		public virtual void PostQualitySet()
		{
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x00103449 File Offset: 0x00101649
		public string GetUniqueLoadID()
		{
			return "Thing_" + this.ThingID;
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x0010345C File Offset: 0x0010165C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Destroyed)
			{
				Log.Error(string.Concat(new string[]
				{
					"Spawning destroyed thing ",
					(this != null) ? this.ToString() : null,
					" at ",
					this.Position.ToString(),
					". Correcting."
				}));
				this.mapIndexOrState = -1;
				if (this.HitPoints <= 0 && this.def.useHitPoints)
				{
					this.HitPoints = 1;
				}
			}
			if (this.Spawned)
			{
				Log.Error("Tried to spawn already-spawned thing " + ((this != null) ? this.ToString() : null) + " at " + this.Position.ToString());
				return;
			}
			int num = Find.Maps.IndexOf(map);
			if (num < 0)
			{
				Log.Error("Tried to spawn thing " + ((this != null) ? this.ToString() : null) + ", but the map provided does not exist.");
				return;
			}
			if (this.stackCount > this.def.stackLimit)
			{
				Log.Error(string.Concat(new string[]
				{
					"Spawned ",
					(this != null) ? this.ToString() : null,
					" with stackCount ",
					this.stackCount.ToString(),
					" but stackLimit is ",
					this.def.stackLimit.ToString(),
					". Truncating."
				}));
				this.stackCount = this.def.stackLimit;
			}
			this.mapIndexOrState = (sbyte)num;
			RegionListersUpdater.RegisterInRegions(this, map);
			if (!map.spawnedThings.TryAdd(this, false))
			{
				Log.Error("Couldn't add thing " + ((this != null) ? this.ToString() : null) + " to spawned things.");
			}
			map.listerThings.Add(this);
			map.thingGrid.Register(this);
			map.gasGrid.Notify_ThingSpawned(this);
			map.mapTemperature.Notify_ThingSpawned(this);
			if (map.IsPlayerHome)
			{
				this.EverSeenByPlayer = true;
			}
			if (Find.TickManager != null)
			{
				Find.TickManager.RegisterAllTickabilityFor(this);
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.RegisterDrawable(this);
			}
			map.tooltipGiverList.Notify_ThingSpawned(this);
			if (this.def.CanAffectLinker)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlagDefOf.Things, true, false);
			}
			if (!this.def.CanOverlapZones)
			{
				map.zoneManager.Notify_NoZoneOverlapThingSpawned(this);
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsSpawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathing.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			map.coverGrid.Register(this);
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_Spawned(this);
				map.listerMergeables.Notify_Spawned(this);
			}
			map.attackTargetsCache.Notify_ThingSpawned(this);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			if (validRegionAt_NoRebuild != null)
			{
				Room room = validRegionAt_NoRebuild.Room;
				if (room != null)
				{
					room.Notify_ContainedThingSpawnedOrDespawned(this);
				}
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
			IHaulSource haulSource = this as IHaulSource;
			if (haulSource != null)
			{
				map.haulDestinationManager.AddHaulSource(haulSource);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				ISlotGroupParent slotGroupParent = (slotGroup != null) ? slotGroup.parent : null;
				if (slotGroupParent != null)
				{
					slotGroupParent.Notify_ReceivedThing(this);
					GenThing.TryDirtyAdjacentGroupContainers(slotGroupParent, map);
				}
			}
			if (this.def.receivesSignals)
			{
				Find.SignalManager.RegisterReceiver(this);
			}
			if (!this.BeingTransportedOnGravship)
			{
				SoundDef soundSpawned = this.def.soundSpawned;
				if (soundSpawned != null)
				{
					soundSpawned.PlayOneShot(this);
				}
				if (!respawningAfterLoad)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "Spawned", this.Named("SUBJECT"));
					this.spawnedTick = Find.TickManager.TicksGame;
					this.despawnedTick = -1;
					List<EntityCodexEntryDef> entries;
					if (AnomalyUtility.ShouldNotifyCodex(this, EntityDiscoveryType.Spawn, out entries))
					{
						Find.EntityCodex.SetDiscovered(entries, this.def, this);
					}
					else
					{
						Find.HiddenItemsManager.SetDiscovered(this.def);
					}
				}
			}
			map.events.Notify_ThingSpawned(this);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x001038DC File Offset: 0x00101ADC
		public bool DeSpawnOrDeselect(DestroyMode mode = DestroyMode.Vanish)
		{
			bool flag = Current.ProgramState == ProgramState.Playing && Find.Selector.IsSelected(this);
			if (this.Spawned)
			{
				this.DeSpawn(mode);
			}
			else if (flag)
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			return flag;
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x00103930 File Offset: 0x00101B30
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Destroyed)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is already destroyed.");
				return;
			}
			if (!this.Spawned)
			{
				Log.Error("Tried to despawn " + this.ToStringSafe<Thing>() + " which is not spawned.");
				return;
			}
			Map map = this.Map;
			map.overlayDrawer.DisposeHandle(this);
			RegionListersUpdater.DeregisterInRegions(this, map);
			map.spawnedThings.Remove(this);
			map.listerThings.Remove(this);
			map.thingGrid.Deregister(this, false);
			map.coverGrid.DeRegister(this);
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			map.tooltipGiverList.Notify_ThingDespawned(this);
			if (this.def.CanAffectLinker)
			{
				map.linkGrid.Notify_LinkerCreatedOrDestroyed(this);
				map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlagDefOf.Things, true, false);
			}
			if (Find.Selector.IsSelected(this))
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			this.DirtyMapMesh(map);
			if (this.def.drawerType != DrawerType.MapMeshOnly)
			{
				map.dynamicDrawManager.DeRegisterDrawable(this);
			}
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(this.Position);
			if (validRegionAt_NoRebuild != null)
			{
				Room room = validRegionAt_NoRebuild.Room;
				if (room != null)
				{
					room.Notify_ContainedThingSpawnedOrDespawned(this);
				}
			}
			if (this.def.AffectsRegions)
			{
				map.regionDirtyer.Notify_ThingAffectingRegionsDespawned(this);
			}
			if (this.def.pathCost != 0 || this.def.passability == Traversability.Impassable)
			{
				map.pathing.RecalculatePerceivedPathCostUnderThing(this);
			}
			if (this.def.AffectsReachability)
			{
				map.reachability.ClearCache();
			}
			Find.TickManager.DeRegisterAllTickabilityFor(this);
			this.mapIndexOrState = -1;
			if (this.def.category == ThingCategory.Item)
			{
				map.listerHaulables.Notify_DeSpawned(this);
				map.listerMergeables.Notify_DeSpawned(this);
			}
			map.attackTargetsCache.Notify_ThingDespawned(this);
			map.physicalInteractionReservationManager.ReleaseAllForTarget(this);
			IHaulEnroute haulEnroute = this as IHaulEnroute;
			if (haulEnroute != null)
			{
				map.enrouteManager.Notify_ContainerDespawned(haulEnroute);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
			IHaulSource haulSource = this as IHaulSource;
			if (haulSource != null)
			{
				map.haulDestinationManager.RemoveHaulSource(haulSource);
			}
			if (this is IThingHolder && Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.category == ThingCategory.Item)
			{
				SlotGroup slotGroup = this.Position.GetSlotGroup(map);
				ISlotGroupParent slotGroupParent = (slotGroup != null) ? slotGroup.parent : null;
				if (slotGroupParent != null)
				{
					slotGroupParent.Notify_LostThing(this);
					GenThing.TryDirtyAdjacentGroupContainers(slotGroupParent, map);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Despawned", this.Named("SUBJECT"));
			this.spawnedTick = -1;
			this.despawnedTick = Find.TickManager.TicksGame;
			map.events.Notify_ThingDespawned(this);
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x00103C21 File Offset: 0x00101E21
		public virtual void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			this.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x00103C2C File Offset: 0x00101E2C
		public virtual void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!Thing.allowDestroyNonDestroyable && !this.def.destroyable)
			{
				Log.Error("Tried to destroy non-destroyable thing " + ((this != null) ? this.ToString() : null));
				return;
			}
			if (this.Destroyed)
			{
				Log.Error("Tried to destroy already-destroyed thing " + ((this != null) ? this.ToString() : null));
				return;
			}
			bool spawned = this.Spawned;
			Map map = this.Map;
			if (this.StyleSourcePrecept != null)
			{
				this.StyleSourcePrecept.Notify_ThingLost(this, spawned);
			}
			if (this.Spawned)
			{
				this.DeSpawn(mode);
			}
			else if (Current.ProgramState == ProgramState.Playing && Find.Selector.IsSelected(this))
			{
				Find.Selector.Deselect(this);
				Find.MainButtonsRoot.tabs.Notify_SelectedObjectDespawned();
			}
			this.mapIndexOrState = -2;
			if (this.def.DiscardOnDestroyed)
			{
				this.Discard(false);
			}
			CompExplosive compExplosive = this.TryGetComp<CompExplosive>();
			if (spawned)
			{
				List<Thing> list = new List<Thing>();
				GenLeaving.DoLeavingsFor(this, map, mode, list);
				if (compExplosive != null)
				{
					compExplosive.AddThingsIgnoredByExplosion(list);
				}
				this.Notify_KilledLeavingsLeft(list);
			}
			if (this.holdingOwner != null)
			{
				this.holdingOwner.Notify_ContainedItemDestroyed(this);
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
			if (!(this is Pawn))
			{
				this.stackCount = 0;
			}
			if (mode != DestroyMode.QuestLogic)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Destroyed", this.Named("SUBJECT"));
			}
			if (mode == DestroyMode.KillFinalize)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "Killed", this.Named("SUBJECT"), map.Named("MAP"));
			}
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x00103DAA File Offset: 0x00101FAA
		public virtual void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x00103DAC File Offset: 0x00101FAC
		public virtual void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			if (this.def.colorGeneratorInTraderStock != null)
			{
				this.SetColor(this.def.colorGeneratorInTraderStock.NewRandomizedColor(), true);
			}
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x00103DD2 File Offset: 0x00101FD2
		public virtual float GetBeauty(bool outside)
		{
			if (!outside || !this.def.StatBaseDefined(StatDefOf.BeautyOutdoors))
			{
				return this.GetStatValue(StatDefOf.Beauty, true, -1);
			}
			return this.GetStatValue(StatDefOf.BeautyOutdoors, true, -1);
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x00103E04 File Offset: 0x00102004
		public virtual void Notify_MyMapRemoved()
		{
			if (this.def.receivesSignals)
			{
				Find.SignalManager.DeregisterReceiver(this);
			}
			if (this.StyleSourcePrecept != null)
			{
				this.StyleSourcePrecept.Notify_ThingLost(this, false);
			}
			if (!ThingOwnerUtility.AnyParentIs<Pawn>(this))
			{
				this.mapIndexOrState = -3;
			}
			ThingOwner thingOwner = this.holdingOwner;
			if (thingOwner != null && thingOwner.Owner is Map)
			{
				this.holdingOwner = null;
			}
			this.RemoveAllReservationsAndDesignationsOnThis();
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x00103E72 File Offset: 0x00102072
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x00103E74 File Offset: 0x00102074
		public virtual void Notify_AbandonedAtTile(PlanetTile tile)
		{
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x00103E76 File Offset: 0x00102076
		public virtual void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x00103E78 File Offset: 0x00102078
		public virtual void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
		{
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x00103E7C File Offset: 0x0010207C
		public virtual void Notify_Unfogged()
		{
			if (this.beenRevealed)
			{
				return;
			}
			this.beenRevealed = true;
			List<EntityCodexEntryDef> entries;
			if (ModsConfig.AnomalyActive && AnomalyUtility.ShouldNotifyCodex(this, EntityDiscoveryType.Unfog, out entries))
			{
				Find.EntityCodex.SetDiscovered(entries, this.def, this);
			}
			else
			{
				Find.HiddenItemsManager.SetDiscovered(this.def);
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Unfogged", this);
			CompLetterOnRevealed compLetterOnRevealed = this.TryGetComp<CompLetterOnRevealed>();
			if (compLetterOnRevealed != null)
			{
				Find.LetterStack.ReceiveLetter(compLetterOnRevealed.Props.label, compLetterOnRevealed.Props.text, compLetterOnRevealed.Props.letterDef, this, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x00103F30 File Offset: 0x00102130
		public void ForceSetStateToUnspawned()
		{
			this.mapIndexOrState = -1;
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x00103F3C File Offset: 0x0010213C
		public void DecrementMapIndex()
		{
			if (this.mapIndexOrState <= 0)
			{
				Log.Warning("Tried to decrement map index for " + ((this != null) ? this.ToString() : null) + ", but mapIndexOrState=" + this.mapIndexOrState.ToString());
				return;
			}
			this.mapIndexOrState -= 1;
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x00103F90 File Offset: 0x00102190
		private void RemoveAllReservationsAndDesignationsOnThis()
		{
			if (this.def.category == ThingCategory.Mote)
			{
				return;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].reservationManager.ReleaseAllForTarget(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllForTarget(this);
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					maps[i].attackTargetReservationManager.ReleaseAllForTarget(attackTarget);
				}
				maps[i].designationManager.RemoveAllDesignationsOn(this, false);
			}
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x0010401C File Offset: 0x0010221C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.tickDelta, "tickDelta", 0, false);
			if (this.def.HasThingIDNumber)
			{
				string thingID = this.ThingID;
				Scribe_Values.Look<string>(ref thingID, "id", null, false);
				if (Scribe.mode != LoadSaveMode.Saving)
				{
					this.ThingID = thingID;
				}
			}
			Scribe_Values.Look<sbyte>(ref this.mapIndexOrState, "map", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.mapIndexOrState >= 0)
			{
				this.mapIndexOrState = -1;
			}
			Scribe_Values.Look<IntVec3>(ref this.positionInt, "pos", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.rotationInt, "rot", Rot4.North, false);
			Scribe_Values.Look<bool>(ref this.debugRotLocked, "debugRotLocked", false, false);
			if (this.def.useHitPoints)
			{
				Scribe_Values.Look<int>(ref this.hitPointsInt, "health", -1, false);
			}
			bool flag = this.def.tradeability != Tradeability.None && this.def.category == ThingCategory.Item;
			if (this.def.stackLimit > 1 || flag)
			{
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, true);
			}
			Scribe_Defs.Look<ThingDef>(ref this.stuffInt, "stuff");
			string facID = (this.factionInt != null) ? this.factionInt.GetUniqueLoadID() : "null";
			Scribe_Values.Look<string>(ref facID, "faction", "null", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (facID == "null")
				{
					this.factionInt = null;
				}
				else if (Find.World != null && Find.FactionManager != null)
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
				else
				{
					Thing.facIDsCached.SetOrAdd(this, facID);
				}
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				if (facID == "null" && Thing.facIDsCached.TryGetValue(this, out facID))
				{
					Thing.facIDsCached.Remove(this);
				}
				if (facID != "null")
				{
					this.factionInt = Find.FactionManager.AllFactions.FirstOrDefault((Faction fa) => fa.GetUniqueLoadID() == facID);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Thing.facIDsCached.Clear();
				if (this.def.MadeFromStuff && this.Stuff == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 1);
					defaultInterpolatedStringHandler.AppendFormatted<Thing>(this);
					defaultInterpolatedStringHandler.AppendLiteral(" is made from stuff but has no stuff set. Setting default stuff.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					this.SetStuffDirect(GenStuff.DefaultStuffFor(this.def));
					if (this.Stuff == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(40, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("Failed to find stuff for ");
						defaultInterpolatedStringHandler2.AppendFormatted<Thing>(this);
						defaultInterpolatedStringHandler2.AppendLiteral(" after loading.");
						Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
				}
			}
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int?>(ref this.overrideGraphicIndex, "overrideGraphicIndex", null, false);
			Scribe_Values.Look<int>(ref this.spawnedTick, "spawnedTick", -1, false);
			Scribe_Values.Look<int>(ref this.despawnedTick, "despawnedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.beenRevealed, "beenRevealed", false, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x00104376 File Offset: 0x00102576
		public virtual void PostMapInit()
		{
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x00104378 File Offset: 0x00102578
		public Vector3? DrawPosHeld
		{
			get
			{
				if (this.Spawned)
				{
					return new Vector3?(this.DrawPos);
				}
				Thing thing = ThingOwnerUtility.SpawnedParentOrMe(this.ParentHolder);
				if (thing == null)
				{
					return null;
				}
				return new Vector3?(thing.DrawPos);
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x001043BC File Offset: 0x001025BC
		public virtual Vector3 DrawPos
		{
			get
			{
				return this.TrueCenter();
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002A9E RID: 10910 RVA: 0x001043C4 File Offset: 0x001025C4
		public virtual Vector2 DrawSize
		{
			get
			{
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.drawSize;
				}
				return Vector2.one;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x001043E9 File Offset: 0x001025E9
		// (set) Token: 0x06002AA0 RID: 10912 RVA: 0x00104428 File Offset: 0x00102628
		public virtual Color DrawColor
		{
			get
			{
				if (this.Stuff != null)
				{
					return this.def.GetColorForStuff(this.Stuff);
				}
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.color;
				}
				return Color.white;
			}
			set
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot set instance color on non-ThingWithComps ");
				defaultInterpolatedStringHandler.AppendFormatted(this.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(" at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.Position);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x00104489 File Offset: 0x00102689
		public virtual Color DrawColorTwo
		{
			get
			{
				if (this.def.graphicData != null)
				{
					return this.def.graphicData.colorTwo;
				}
				return Color.white;
			}
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x001044AE File Offset: 0x001026AE
		public void DrawNowAt(Vector3 drawLoc, bool flip = false)
		{
			this.DynamicDrawPhaseAt(DrawPhase.Draw, drawLoc, flip);
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x001044B9 File Offset: 0x001026B9
		public void DynamicDrawPhase(DrawPhase phase)
		{
			if (this.def.drawerType == DrawerType.MapMeshOnly)
			{
				return;
			}
			this.DynamicDrawPhaseAt(phase, this.DrawPos, false);
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x001044D8 File Offset: 0x001026D8
		public virtual void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			if (phase == DrawPhase.Draw)
			{
				this.DrawAt(drawLoc, flip);
			}
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x001044E8 File Offset: 0x001026E8
		protected virtual void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly || !this.Spawned)
			{
				this.Graphic.Draw(drawLoc, flip ? this.Rotation.Opposite : this.Rotation, this, 0f);
			}
			SilhouetteUtility.DrawGraphicSilhouette(this, drawLoc);
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x0010453D File Offset: 0x0010273D
		public virtual void Print(SectionLayer layer)
		{
			if (!this.def.dontPrint)
			{
				this.Graphic.Print(layer, this, 0f);
			}
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x00104560 File Offset: 0x00102760
		public void DirtyMapMesh(Map map)
		{
			if (this.def.drawerType != DrawerType.RealtimeOnly)
			{
				foreach (IntVec3 loc in this.OccupiedRect())
				{
					map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Things);
				}
			}
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x001045D4 File Offset: 0x001027D4
		public virtual void DrawGUIOverlay()
		{
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				if (this.def.stackLimit > 1)
				{
					GenMapUI.DrawThingLabel(this, this.stackCount.ToStringCached());
					return;
				}
				QualityCategory cat;
				if (this.def.drawGUIOverlayQuality && this.TryGetQuality(out cat))
				{
					GenMapUI.DrawThingLabel(this, cat.GetLabelShort());
				}
			}
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x00104630 File Offset: 0x00102830
		public virtual void DrawExtraSelectionOverlays()
		{
			if (this.def.specialDisplayRadius > 0.1f)
			{
				GenDraw.DrawRadiusRing(this.Position, this.def.specialDisplayRadius);
			}
			if (this.def.drawPlaceWorkersWhileSelected && this.def.PlaceWorkers != null)
			{
				for (int i = 0; i < this.def.PlaceWorkers.Count; i++)
				{
					this.def.PlaceWorkers[i].DrawGhost(this.def, this.Position, this.Rotation, Color.white, this);
				}
			}
			GenDraw.DrawInteractionCells(this.def, this.Position, this.rotationInt);
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x001046DF File Offset: 0x001028DF
		public virtual string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			return stringBuilder.ToString();
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x001046F4 File Offset: 0x001028F4
		public virtual string GetInspectStringLowPriority()
		{
			string result = null;
			Thing.tmpDeteriorationReasons.Clear();
			float f = SteadyEnvironmentEffects.FinalDeteriorationRate(this, Thing.tmpDeteriorationReasons);
			if (Thing.tmpDeteriorationReasons.Count != 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("DeterioratingBecauseOf".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(Thing.tmpDeteriorationReasons.ToCommaList(false, false).CapitalizeFirst());
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PerDay".Translate(f.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
				defaultInterpolatedStringHandler.AppendLiteral(")");
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return result;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x001047A2 File Offset: 0x001029A2
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			Thing.<GetGizmos>d__203 <GetGizmos>d__ = new Thing.<GetGizmos>d__203(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x001047B2 File Offset: 0x001029B2
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x001047B9 File Offset: 0x001029B9
		public virtual IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x001047C0 File Offset: 0x001029C0
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return this.def.inspectorTabsResolved;
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x001047CD File Offset: 0x001029CD
		public virtual string GetCustomLabelNoCount(bool includeHp = true)
		{
			return GenLabel.ThingLabel(this, 1, includeHp, true);
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x001047D8 File Offset: 0x001029D8
		public DamageWorker.DamageResult TakeDamage(DamageInfo dinfo)
		{
			if (this.Destroyed)
			{
				return new DamageWorker.DamageResult();
			}
			if (dinfo.Amount == 0f)
			{
				return new DamageWorker.DamageResult();
			}
			if (this.def.damageMultipliers != null)
			{
				for (int i = 0; i < this.def.damageMultipliers.Count; i++)
				{
					if (this.def.damageMultipliers[i].damageDef == dinfo.Def)
					{
						int num = Mathf.RoundToInt(dinfo.Amount * this.def.damageMultipliers[i].multiplier);
						dinfo.SetAmount((float)num);
					}
				}
			}
			bool flag;
			this.PreApplyDamage(ref dinfo, out flag);
			if (flag)
			{
				return new DamageWorker.DamageResult();
			}
			bool spawnedOrAnyParentSpawned = this.SpawnedOrAnyParentSpawned;
			Map mapHeld = this.MapHeld;
			DamageWorker.DamageResult damageResult = dinfo.Def.Worker.Apply(dinfo, this);
			if (dinfo.Def.harmsHealth && spawnedOrAnyParentSpawned)
			{
				mapHeld.damageWatcher.Notify_DamageTaken(this, damageResult.totalDamageDealt);
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null)
			{
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					hediff.Notify_PawnDamagedThing(this, dinfo, damageResult);
				}
			}
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				if (dinfo.SpawnFilth)
				{
					GenLeaving.DropFilthDueToDamage(this, damageResult.totalDamageDealt);
				}
				if (dinfo.Instigator != null)
				{
					Pawn pawn2 = dinfo.Instigator as Pawn;
					if (pawn2 != null)
					{
						pawn2.records.AddTo(RecordDefOf.DamageDealt, damageResult.totalDamageDealt);
					}
					if (dinfo.Instigator.Faction == Faction.OfPlayer)
					{
						QuestUtility.SendQuestTargetSignals(this.questTags, "TookDamageFromPlayer", this.Named("SUBJECT"), dinfo.Instigator.Named("INSTIGATOR"));
					}
				}
				QuestUtility.SendQuestTargetSignals(this.questTags, "TookDamage", this.Named("SUBJECT"), dinfo.Instigator.Named("INSTIGATOR"), mapHeld.Named("MAP"));
			}
			if (!this.Destroyed && this.FlammableNow && dinfo.Def.igniteChanceByTargetFlammability != null && Rand.Chance(dinfo.Def.igniteChanceByTargetFlammability.Evaluate(this.GetStatValue(StatDefOf.Flammability, true, -1))))
			{
				this.TryAttachFire(Rand.Range(0.55f, 0.85f), dinfo.Instigator);
			}
			this.PostApplyDamage(dinfo, damageResult.totalDamageDealt);
			return damageResult;
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00104A7C File Offset: 0x00102C7C
		public virtual void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x00104A81 File Offset: 0x00102C81
		public virtual void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x00104A84 File Offset: 0x00102C84
		public virtual bool CanStackWith(Thing other)
		{
			return !this.Destroyed && !other.Destroyed && this.def.category == ThingCategory.Item && !this.IsRelic() && !other.IsRelic() && this.def == other.def && this.Stuff == other.Stuff;
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x00104AE4 File Offset: 0x00102CE4
		public virtual bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int num = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.def.useHitPoints)
			{
				this.HitPoints = Mathf.CeilToInt((float)(this.HitPoints * this.stackCount + other.HitPoints * num) / (float)(this.stackCount + num));
			}
			this.stackCount += num;
			other.stackCount -= num;
			if (this.Map != null)
			{
				this.DirtyMapMesh(this.Map);
			}
			StealAIDebugDrawer.Notify_ThingChanged(this);
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (other.stackCount <= 0)
			{
				other.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x00104BA0 File Offset: 0x00102DA0
		public virtual Thing SplitOff(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentException("SplitOff with count <= 0", "count");
			}
			if (count >= this.stackCount)
			{
				if (count > this.stackCount)
				{
					Log.Error(string.Concat(new string[]
					{
						"Tried to split off ",
						count.ToString(),
						" of ",
						(this != null) ? this.ToString() : null,
						" but there are only ",
						this.stackCount.ToString()
					}));
				}
				this.DeSpawnOrDeselect(DestroyMode.Vanish);
				ThingOwner thingOwner = this.holdingOwner;
				if (thingOwner != null)
				{
					thingOwner.Remove(this);
				}
				return this;
			}
			Thing thing = ThingMaker.MakeThing(this.def, this.Stuff);
			thing.stackCount = count;
			this.stackCount -= count;
			if (this.Map != null)
			{
				this.DirtyMapMesh(this.Map);
			}
			if (this.Spawned)
			{
				this.Map.listerMergeables.Notify_ThingStackChanged(this);
			}
			if (this.def.useHitPoints)
			{
				thing.HitPoints = this.HitPoints;
			}
			return thing;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x00104CAE File Offset: 0x00102EAE
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Thing.<SpecialDisplayStats>d__215 <SpecialDisplayStats>d__ = new Thing.<SpecialDisplayStats>d__215(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x00104CBE File Offset: 0x00102EBE
		public virtual IEnumerable<DefHyperlink> DescriptionHyperlinks
		{
			get
			{
				Thing.<get_DescriptionHyperlinks>d__217 <get_DescriptionHyperlinks>d__ = new Thing.<get_DescriptionHyperlinks>d__217(-2);
				<get_DescriptionHyperlinks>d__.<>4__this = this;
				return <get_DescriptionHyperlinks>d__;
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x00104CD0 File Offset: 0x00102ED0
		public virtual void Notify_ColorChanged()
		{
			this.graphicInt = null;
			this.styleGraphicInt = null;
			if (this.Spawned && (this.def.drawerType == DrawerType.MapMeshOnly || this.def.drawerType == DrawerType.MapMeshAndRealTime))
			{
				this.Map.mapDrawer.MapMeshDirty(this.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x00104D2F File Offset: 0x00102F2F
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x00104D31 File Offset: 0x00102F31
		public virtual void Notify_Unequipped(Pawn pawn)
		{
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x00104D33 File Offset: 0x00102F33
		public virtual void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x00104D35 File Offset: 0x00102F35
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x00104D37 File Offset: 0x00102F37
		public virtual void Notify_DebugSpawned()
		{
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x00104D39 File Offset: 0x00102F39
		public virtual void Notify_MinifiedThingAboutToBeDestroyed(DestroyMode mode)
		{
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x00104D3B File Offset: 0x00102F3B
		public virtual void Notify_RecipeProduced(Pawn pawn)
		{
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x00104D3D File Offset: 0x00102F3D
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x00104D3F File Offset: 0x00102F3F
		public virtual void Notify_Explosion(Explosion explosion)
		{
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x00104D41 File Offset: 0x00102F41
		public virtual void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x00104D43 File Offset: 0x00102F43
		public virtual void Notify_ThingSelected()
		{
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x00104D48 File Offset: 0x00102F48
		public virtual TipSignal GetTooltip()
		{
			string text = this.LabelCap;
			if (this.def.useHitPoints)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.HitPoints.ToString(),
					" / ",
					this.MaxHitPoints.ToString()
				});
			}
			return new TipSignal(text, this.thingIDNumber * 251235);
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x00104DBD File Offset: 0x00102FBD
		public virtual bool BlocksPawn(Pawn p)
		{
			return this.def.passability == Traversability.Impassable || (this.def.IsFence && p.FenceBlocked);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x00104DE7 File Offset: 0x00102FE7
		public void SetFactionDirect(Faction newFaction)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFactionDirect on " + ((this != null) ? this.ToString() : null) + " which cannot have a faction.");
				return;
			}
			this.factionInt = newFaction;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x00104E20 File Offset: 0x00103020
		public virtual void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (!this.def.CanHaveFaction)
			{
				Log.Error("Tried to SetFaction on " + ((this != null) ? this.ToString() : null) + " which cannot have a faction.");
				return;
			}
			Faction previous = this.factionInt;
			this.factionInt = newFaction;
			if (this.Spawned)
			{
				IAttackTarget attackTarget = this as IAttackTarget;
				if (attackTarget != null)
				{
					this.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFaction", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			if (newFaction != Faction.OfPlayer)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFactionToNonPlayer", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			}
			else
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "ChangedFactionToPlayer", this.Named("SUBJECT"), newFaction.Named("FACTION"));
			}
			if (this.Spawned)
			{
				this.Map.events.Notify_ThingFactionChanged(previous, this.factionInt);
			}
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x00104F28 File Offset: 0x00103128
		public virtual AcceptanceReport ClaimableBy(Faction by)
		{
			return false;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x00104F30 File Offset: 0x00103130
		public virtual bool AdoptableBy(Faction by, StringBuilder reason = null)
		{
			return false;
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x00104F34 File Offset: 0x00103134
		public bool FactionPreventsClaimingOrAdopting(Faction faction, bool forClaim, out string reason)
		{
			reason = null;
			if (faction == null)
			{
				return false;
			}
			if (faction == Faction.OfInsects)
			{
				if (HiveUtility.AnyHivePreventsClaiming(this))
				{
					return true;
				}
			}
			else
			{
				if (faction == Faction.OfMechanoids)
				{
					if (MechClusterUtility.PartOfActiveMechCluster(this))
					{
						return true;
					}
					using (HashSet<IAttackTarget>.Enumerator enumerator = this.MapHeld.attackTargetsCache.TargetsHostileToFaction(Faction.OfPlayer).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IAttackTarget attackTarget = enumerator.Current;
							if (attackTarget.Thing != null && attackTarget.Thing.Faction == faction)
							{
								Pawn pawn = attackTarget.Thing as Pawn;
								if (pawn == null)
								{
									if (forClaim)
									{
										reason = "MessageCannotClaimWhenThreatsAreNear".Translate(this.Named("CLAIMABLE"), attackTarget.Named("THREAT"));
									}
									else
									{
										reason = "MessageCannotAdoptWhileThreatsAreNear".Translate(this.Named("CLAIMABLE"), attackTarget.Named("THREAT"));
									}
									return true;
								}
								if (GenHostility.IsActiveThreatToPlayer(pawn, false))
								{
									if (forClaim)
									{
										reason = "MessageCannotClaimWhenPawnThreatsAreNear".Translate(this.Named("CLAIMABLE"), pawn.Named("THREAT"));
									}
									else
									{
										reason = "MessageCannotAdoptWhilePawnThreatsAreNear".Translate(this.Named("CLAIMABLE"), pawn.Named("THREAT"));
									}
									return true;
								}
							}
						}
						return false;
					}
				}
				if (faction == Faction.OfAncients && this.Spawned && !this.Map.IsPlayerHome && GenHostility.AnyHostileActiveThreatToPlayer(this.Map, true, true))
				{
					return true;
				}
				if (this.Spawned && faction != Faction.OfPlayer)
				{
					List<Pawn> list = this.Map.mapPawns.SpawnedPawnsInFaction(faction);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].RaceProps.ToolUser && GenHostility.IsPotentialThreat(list[i]))
						{
							if (forClaim)
							{
								reason = "MessageCannotClaimWhenThreatsAreNear".Translate(this.Named("CLAIMABLE"), list[i].Named("THREAT"));
							}
							else
							{
								reason = "MessageCannotAdoptWhileThreatsAreNear".Translate(this.Named("CLAIMABLE"), list[i].Named("THREAT"));
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x001051C4 File Offset: 0x001033C4
		public void SetPositionDirect(IntVec3 newPos)
		{
			this.positionInt = newPos;
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x001051CD File Offset: 0x001033CD
		public void SetStuffDirect(ThingDef newStuff)
		{
			this.stuffInt = newStuff;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x001051D6 File Offset: 0x001033D6
		public override string ToString()
		{
			if (this.def != null)
			{
				return this.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x001051F2 File Offset: 0x001033F2
		public bool Equals(Thing other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.def.category == ThingCategory.Mote)
			{
				return this == other;
			}
			return this.thingIDNumber == other.thingIDNumber && this.def.Equals(other.def);
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x0010522E File Offset: 0x0010342E
		public override int GetHashCode()
		{
			if (this.thingIDNumber == -1)
			{
				return base.GetHashCode();
			}
			return this.thingIDNumber;
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x00105248 File Offset: 0x00103448
		public virtual void Discard(bool silentlyRemoveReferences = false)
		{
			if (this.mapIndexOrState != -2)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to discard ",
					(this != null) ? this.ToString() : null,
					" whose state is ",
					this.mapIndexOrState.ToString(),
					"."
				}));
				return;
			}
			this.mapIndexOrState = -3;
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x001052AE File Offset: 0x001034AE
		public virtual void Notify_DefsHotReloaded()
		{
			this.graphicInt = null;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x001052B7 File Offset: 0x001034B7
		public virtual IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			Thing.<ButcherProducts>d__244 <ButcherProducts>d__ = new Thing.<ButcherProducts>d__244(-2);
			<ButcherProducts>d__.<>4__this = this;
			<ButcherProducts>d__.<>3__efficiency = efficiency;
			return <ButcherProducts>d__;
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x001052CE File Offset: 0x001034CE
		public virtual IEnumerable<Thing> StoneBlockProducts(Pawn worker, float efficiency)
		{
			Thing.<StoneBlockProducts>d__245 <StoneBlockProducts>d__ = new Thing.<StoneBlockProducts>d__245(-2);
			<StoneBlockProducts>d__.<>4__this = this;
			<StoneBlockProducts>d__.<>3__efficiency = efficiency;
			return <StoneBlockProducts>d__;
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x001052E5 File Offset: 0x001034E5
		public virtual IEnumerable<Thing> SmeltProducts(float efficiency)
		{
			Thing.<SmeltProducts>d__247 <SmeltProducts>d__ = new Thing.<SmeltProducts>d__247(-2);
			<SmeltProducts>d__.<>4__this = this;
			return <SmeltProducts>d__;
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x001052F8 File Offset: 0x001034F8
		public float Ingested(Pawn ingester, float nutritionWanted)
		{
			if (this.Destroyed)
			{
				Log.Error(((ingester != null) ? ingester.ToString() : null) + " ingested destroyed thing " + ((this != null) ? this.ToString() : null));
				return 0f;
			}
			if (!this.IngestibleNow)
			{
				Log.Error(((ingester != null) ? ingester.ToString() : null) + " ingested IngestibleNow=false thing " + ((this != null) ? this.ToString() : null));
				return 0f;
			}
			ingester.mindState.lastIngestTick = Find.TickManager.TicksGame;
			if (ingester.needs.mood != null)
			{
				List<FoodUtility.ThoughtFromIngesting> list = FoodUtility.ThoughtsFromIngesting(ingester, this, this.def);
				for (int i = 0; i < list.Count; i++)
				{
					Thought_Memory thought_Memory = ThoughtMaker.MakeThought(list[i].thought, list[i].fromPrecept);
					Thought_FoodEaten thought_FoodEaten = thought_Memory as Thought_FoodEaten;
					if (thought_FoodEaten != null)
					{
						thought_FoodEaten.SetFood(this);
					}
					ingester.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
			Need_Chemical_Any drugsDesire = ingester.needs.drugsDesire;
			if (drugsDesire != null)
			{
				drugsDesire.Notify_IngestedDrug(this);
			}
			bool flag = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeat(this, this.def);
			bool flag2 = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(this);
			if (flag && ingester.IsColonist)
			{
				TaleRecorder.RecordTale(TaleDefOf.AteRawHumanlikeMeat, new object[]
				{
					ingester
				});
			}
			if (flag2)
			{
				ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				if (flag)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeatDirect, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
			}
			else if (ModsConfig.IdeologyActive && !FoodUtility.AcceptableCannibalNonHumanlikeMeatFood(this.def))
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonCannibalFood, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			if (this.def.ingestible.ateEvent != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.def.ingestible.ateEvent, ingester.Named(HistoryEventArgsNames.Doer)), false);
			}
			if (ModsConfig.IdeologyActive)
			{
				FoodKind foodKind = FoodUtility.GetFoodKind(this);
				if (foodKind != FoodKind.Any && !this.def.IsProcessedFood)
				{
					if (foodKind == FoodKind.Meat)
					{
						if (!flag2)
						{
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
						}
					}
					else if (!this.def.IsDrug && this.def.ingestible.CachedNutrition > 0f)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
				}
				if (FoodUtility.IsVeneratedAnimalMeatOrCorpseOrHasIngredients(this, ingester))
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
				if (this.def.thingCategories != null && this.def.thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
				{
					if (this.def.IsFungus)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteFungus, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
					else
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonFungusPlant, ingester.Named(HistoryEventArgsNames.Doer)), false);
					}
				}
			}
			CompIngredients compIngredients = this.TryGetComp<CompIngredients>();
			if (compIngredients != null)
			{
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				for (int j = 0; j < compIngredients.ingredients.Count; j++)
				{
					if (!flag3 && FoodUtility.GetMeatSourceCategory(compIngredients.ingredients[j]) == MeatSourceCategory.Humanlike)
					{
						ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeatAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag3 = true;
					}
					else if (!flag4 && ingester.Ideo != null && compIngredients.ingredients[j].IsMeat && ingester.Ideo.IsVeneratedAnimal(compIngredients.ingredients[j].ingestible.sourceDef))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteVeneratedAnimalMeat, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag4 = true;
					}
					if (!flag5 && FoodUtility.GetMeatSourceCategory(compIngredients.ingredients[j]) == MeatSourceCategory.Insect)
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteInsectMeatAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
						flag5 = true;
					}
					if (ModsConfig.IdeologyActive && !flag6 && compIngredients.ingredients[j].thingCategories.Contains(ThingCategoryDefOf.PlantFoodRaw))
					{
						if (compIngredients.ingredients[j].IsFungus)
						{
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteFungusAsIngredient, ingester.Named(HistoryEventArgsNames.Doer)), false);
							flag6 = true;
						}
						else
						{
							flag7 = true;
						}
					}
				}
				if (ModsConfig.IdeologyActive && !flag6 && flag7)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteNonFungusMealWithPlants, ingester.Named(HistoryEventArgsNames.Doer)), false);
				}
			}
			int num;
			float result;
			this.IngestedCalculateAmounts(ingester, nutritionWanted, out num, out result);
			if (!ingester.Dead && ingester.needs.joy != null && Mathf.Abs(this.def.ingestible.joy) > 0.0001f && num > 0)
			{
				ingester.needs.joy.GainJoy((float)num * this.def.ingestible.joy, this.def.ingestible.joyKind ?? JoyKindDefOf.Gluttonous);
			}
			float num2;
			float chance = FoodUtility.TryGetFoodPoisoningChanceOverrideFromTraits(ingester, this, out num2) ? num2 : (this.GetStatValue(StatDefOf.FoodPoisonChanceFixedHuman, true, -1) * FoodUtility.GetFoodPoisonChanceFactor(ingester));
			if (ingester.RaceProps.Humanlike && Rand.Chance(chance))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this, FoodPoisonCause.DangerousFoodType);
			}
			List<Hediff> hediffs = ingester.health.hediffSet.hediffs;
			for (int k = 0; k < hediffs.Count; k++)
			{
				hediffs[k].Notify_IngestedThing(this, num);
			}
			Pawn_GeneTracker genes = ingester.genes;
			if (genes != null)
			{
				genes.Notify_IngestedThing(this, num);
			}
			bool flag8 = false;
			if (num > 0)
			{
				if (this.stackCount == 0)
				{
					Log.Error(((this != null) ? this.ToString() : null) + " stack count is 0.");
				}
				if (num == this.stackCount)
				{
					flag8 = true;
				}
				else
				{
					this.SplitOff(num);
				}
			}
			this.PrePostIngested(ingester);
			if (flag8)
			{
				ingester.carryTracker.innerContainer.Remove(this);
			}
			if (this.def.ingestible.outcomeDoers != null)
			{
				for (int l = 0; l < this.def.ingestible.outcomeDoers.Count; l++)
				{
					this.def.ingestible.outcomeDoers[l].DoIngestionOutcome(ingester, this, num);
				}
			}
			if (flag8 && !this.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.PostIngested(ingester);
			return result;
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x001059FA File Offset: 0x00103BFA
		protected virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x001059FC File Offset: 0x00103BFC
		protected virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x00105A00 File Offset: 0x00103C00
		protected virtual void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float num = FoodUtility.NutritionForEater(ingester, this);
			numTaken = Mathf.CeilToInt(nutritionWanted / num);
			numTaken = Mathf.Min(numTaken, this.stackCount);
			if (this.def.ingestible.maxNumToIngestAtOnce > 0)
			{
				numTaken = Mathf.Min(numTaken, this.def.ingestible.maxNumToIngestAtOnce);
			}
			numTaken = Mathf.Max(numTaken, 1);
			nutritionIngested = (float)numTaken * num;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x00105A6C File Offset: 0x00103C6C
		public virtual bool PreventPlayerSellingThingsNearby(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x00105A72 File Offset: 0x00103C72
		public virtual void PreSwapMap()
		{
			this.beingTransportedOnGravship = true;
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x00105A7B File Offset: 0x00103C7B
		public virtual void PostSwapMap()
		{
			this.beingTransportedOnGravship = false;
			QuestUtility.SendQuestTargetSignals(this.questTags, "SwappedMap", this.Named("SUBJECT"));
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x00105AA0 File Offset: 0x00103CA0
		public virtual void Notify_LeftBehind()
		{
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftBehind", this.Named("SUBJECT"));
			IThingHolder thingHolder = this as IThingHolder;
			if (thingHolder != null && thingHolder.GetDirectlyHeldThings() != null)
			{
				foreach (Thing thing in thingHolder.GetDirectlyHeldThings().ToList<Thing>())
				{
					thing.Notify_LeftBehind();
				}
			}
		}

		// Token: 0x04001D43 RID: 7491
		public ThingDef def;

		// Token: 0x04001D44 RID: 7492
		public int thingIDNumber = -1;

		// Token: 0x04001D45 RID: 7493
		private sbyte mapIndexOrState = -1;

		// Token: 0x04001D46 RID: 7494
		private IntVec3 positionInt = IntVec3.Invalid;

		// Token: 0x04001D47 RID: 7495
		private Rot4 rotationInt = Rot4.North;

		// Token: 0x04001D48 RID: 7496
		public int stackCount = 1;

		// Token: 0x04001D49 RID: 7497
		protected Faction factionInt;

		// Token: 0x04001D4A RID: 7498
		private ThingDef stuffInt;

		// Token: 0x04001D4B RID: 7499
		private Graphic graphicInt;

		// Token: 0x04001D4C RID: 7500
		protected Graphic styleGraphicInt;

		// Token: 0x04001D4D RID: 7501
		private int hitPointsInt = -1;

		// Token: 0x04001D4E RID: 7502
		public ThingOwner holdingOwner;

		// Token: 0x04001D4F RID: 7503
		public List<string> questTags;

		// Token: 0x04001D50 RID: 7504
		public int spawnedTick = -1;

		// Token: 0x04001D51 RID: 7505
		public int despawnedTick = -1;

		// Token: 0x04001D52 RID: 7506
		public int? overrideGraphicIndex;

		// Token: 0x04001D53 RID: 7507
		public bool debugRotLocked;

		// Token: 0x04001D54 RID: 7508
		private bool beingTransportedOnGravship;

		// Token: 0x04001D55 RID: 7509
		private int tickDelta;

		// Token: 0x04001D56 RID: 7510
		private bool beenRevealed;

		// Token: 0x04001D57 RID: 7511
		public bool shouldHighlightCached;

		// Token: 0x04001D58 RID: 7512
		public int shouldHighlightCachedTick;

		// Token: 0x04001D59 RID: 7513
		public Color highlightColorCached;

		// Token: 0x04001D5A RID: 7514
		public int highlightColorCachedTick;

		// Token: 0x04001D5B RID: 7515
		protected const sbyte UnspawnedState = -1;

		// Token: 0x04001D5C RID: 7516
		private const sbyte MemoryState = -2;

		// Token: 0x04001D5D RID: 7517
		private const sbyte DiscardedState = -3;

		// Token: 0x04001D5E RID: 7518
		private List<IThingHolder> tmpHolders;

		// Token: 0x04001D5F RID: 7519
		private bool cached;

		// Token: 0x04001D60 RID: 7520
		private bool cachedIsHolder;

		// Token: 0x04001D61 RID: 7521
		private IThingHolder cachedHolder;

		// Token: 0x04001D62 RID: 7522
		private IThingHolderTickable cachedTickable;

		// Token: 0x04001D63 RID: 7523
		public static bool allowDestroyNonDestroyable = false;

		// Token: 0x04001D64 RID: 7524
		private static Dictionary<Thing, string> facIDsCached = new Dictionary<Thing, string>();

		// Token: 0x04001D65 RID: 7525
		private static List<string> tmpDeteriorationReasons = new List<string>();

		// Token: 0x04001D66 RID: 7526
		public static HashSet<RitualPatternDef> showingGizmosForRitualsTmp = new HashSet<RitualPatternDef>();

		// Token: 0x04001D67 RID: 7527
		private static List<string> tmpIdeoNames = new List<string>();

		// Token: 0x04001D68 RID: 7528
		public const float SmeltCostRecoverFraction = 0.25f;
	}
}
