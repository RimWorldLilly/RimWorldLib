using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002334 RID: 9012
	public class Tile : IExposable
	{
		// Token: 0x1700270C RID: 9996
		// (get) Token: 0x0600DE97 RID: 56983 RVA: 0x0052C832 File Offset: 0x0052AA32
		public PlanetLayer Layer
		{
			get
			{
				return this.tile.Layer;
			}
		}

		// Token: 0x1700270D RID: 9997
		// (get) Token: 0x0600DE98 RID: 56984 RVA: 0x0052C83F File Offset: 0x0052AA3F
		// (set) Token: 0x0600DE99 RID: 56985 RVA: 0x0052C847 File Offset: 0x0052AA47
		public BiomeDef PrimaryBiome
		{
			get
			{
				return this.biome;
			}
			set
			{
				this.biome = value;
			}
		}

		// Token: 0x1700270E RID: 9998
		// (get) Token: 0x0600DE9A RID: 56986 RVA: 0x0052C850 File Offset: 0x0052AA50
		public IEnumerable<BiomeDef> Biomes
		{
			get
			{
				Tile.<get_Biomes>d__18 <get_Biomes>d__ = new Tile.<get_Biomes>d__18(-2);
				<get_Biomes>d__.<>4__this = this;
				return <get_Biomes>d__;
			}
		}

		// Token: 0x1700270F RID: 9999
		// (get) Token: 0x0600DE9B RID: 56987 RVA: 0x0052C860 File Offset: 0x0052AA60
		public Landmark Landmark
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return null;
				}
				return Find.World.landmarks[this.tile];
			}
		}

		// Token: 0x17002710 RID: 10000
		// (get) Token: 0x0600DE9C RID: 56988 RVA: 0x0052C880 File Offset: 0x0052AA80
		public bool OnSurface
		{
			get
			{
				return this.tile.Layer.IsRootSurface;
			}
		}

		// Token: 0x17002711 RID: 10001
		// (get) Token: 0x0600DE9D RID: 56989 RVA: 0x0052C892 File Offset: 0x0052AA92
		public virtual bool WaterCovered
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002712 RID: 10002
		// (get) Token: 0x0600DE9E RID: 56990 RVA: 0x0052C895 File Offset: 0x0052AA95
		public bool IsCoastal
		{
			get
			{
				return Find.World.CoastDirectionAt(this.tile) != Rot4.Invalid;
			}
		}

		// Token: 0x17002713 RID: 10003
		// (get) Token: 0x0600DE9F RID: 56991 RVA: 0x0052C8B4 File Offset: 0x0052AAB4
		public IList<TileMutatorDef> Mutators
		{
			get
			{
				IList<TileMutatorDef> list = this.mutatorsNullable;
				return list ?? Array.Empty<TileMutatorDef>();
			}
		}

		// Token: 0x17002714 RID: 10004
		// (get) Token: 0x0600DEA0 RID: 56992 RVA: 0x0052C8D4 File Offset: 0x0052AAD4
		public Hilliness HillinessLabel
		{
			get
			{
				if (this.hillinessLabelCached != null)
				{
					return this.hillinessLabelCached.Value;
				}
				this.hillinessLabelCached = new Hilliness?(this.hilliness);
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					if (tileMutatorDef.hillinessLabel != Hilliness.Undefined)
					{
						this.hillinessLabelCached = new Hilliness?(tileMutatorDef.hillinessLabel);
					}
				}
				return this.hillinessLabelCached.Value;
			}
		}

		// Token: 0x17002715 RID: 10005
		// (get) Token: 0x0600DEA1 RID: 56993 RVA: 0x0052C968 File Offset: 0x0052AB68
		public Hilliness HillinessForElevationGen
		{
			get
			{
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					if (tileMutatorDef.hillinessForElevationGen != Hilliness.Undefined)
					{
						return tileMutatorDef.hillinessForElevationGen;
					}
				}
				return this.hilliness;
			}
		}

		// Token: 0x17002716 RID: 10006
		// (get) Token: 0x0600DEA2 RID: 56994 RVA: 0x0052C9C8 File Offset: 0x0052ABC8
		public Hilliness HillinessForOreGeneration
		{
			get
			{
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					if (tileMutatorDef.hillinessForOreGeneration != Hilliness.Undefined)
					{
						return tileMutatorDef.hillinessForOreGeneration;
					}
				}
				return this.HillinessForElevationGen;
			}
		}

		// Token: 0x17002717 RID: 10007
		// (get) Token: 0x0600DEA3 RID: 56995 RVA: 0x0052CA28 File Offset: 0x0052AC28
		public float AnimalDensity
		{
			get
			{
				float num = this.biome.animalDensity;
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					num *= tileMutatorDef.animalDensityFactor;
				}
				return num;
			}
		}

		// Token: 0x17002718 RID: 10008
		// (get) Token: 0x0600DEA4 RID: 56996 RVA: 0x0052CA84 File Offset: 0x0052AC84
		public float PlantDensityFactor
		{
			get
			{
				float num = 1f;
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					num *= tileMutatorDef.plantDensityFactor;
				}
				return num;
			}
		}

		// Token: 0x17002719 RID: 10009
		// (get) Token: 0x0600DEA5 RID: 56997 RVA: 0x0052CADC File Offset: 0x0052ACDC
		public float FishPopulationFactor
		{
			get
			{
				float num = 1f;
				foreach (TileMutatorDef tileMutatorDef in this.Mutators)
				{
					num *= tileMutatorDef.fishPopulationFactor;
				}
				return num;
			}
		}

		// Token: 0x1700271A RID: 10010
		// (get) Token: 0x0600DEA6 RID: 56998 RVA: 0x0052CB34 File Offset: 0x0052AD34
		public bool AllowRoofedEdgeWalkIn
		{
			get
			{
				using (IEnumerator<TileMutatorDef> enumerator = this.Mutators.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.allowRoofedEdgeWalkIn)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700271B RID: 10011
		// (get) Token: 0x0600DEA7 RID: 56999 RVA: 0x0052CB88 File Offset: 0x0052AD88
		public float MaxTemperature
		{
			get
			{
				if (this.cachedMaxTemp == null)
				{
					this.cachedMaxTemp = new float?(GenTemperature.MaxTemperatureAtTile(this.tile));
				}
				return this.cachedMaxTemp.Value;
			}
		}

		// Token: 0x1700271C RID: 10012
		// (get) Token: 0x0600DEA8 RID: 57000 RVA: 0x0052CBB8 File Offset: 0x0052ADB8
		public float MinTemperature
		{
			get
			{
				if (this.cachedMinTemp == null)
				{
					this.cachedMinTemp = new float?(GenTemperature.MinTemperatureAtTile(this.tile));
				}
				return this.cachedMinTemp.Value;
			}
		}

		// Token: 0x1700271D RID: 10013
		// (get) Token: 0x0600DEA9 RID: 57001 RVA: 0x0052CBE8 File Offset: 0x0052ADE8
		public float MaxFishPopulation
		{
			get
			{
				return this.PrimaryBiome.maxFishPopulation * this.FishPopulationFactor;
			}
		}

		// Token: 0x0600DEAA RID: 57002 RVA: 0x0052CBFC File Offset: 0x0052ADFC
		public Tile()
		{
		}

		// Token: 0x0600DEAB RID: 57003 RVA: 0x0052CC1A File Offset: 0x0052AE1A
		public Tile(PlanetTile tile)
		{
			this.tile = tile;
		}

		// Token: 0x0600DEAC RID: 57004 RVA: 0x0052CC40 File Offset: 0x0052AE40
		public void AddMutator(TileMutatorDef mutator)
		{
			if (this.mutatorsNullable == null)
			{
				this.mutatorsNullable = new List<TileMutatorDef>();
			}
			for (int i = this.mutatorsNullable.Count - 1; i >= 0; i--)
			{
				TileMutatorDef tileMutatorDef = this.mutatorsNullable[i];
				foreach (string item in mutator.categories)
				{
					if (tileMutatorDef.categories.Contains(item))
					{
						if (mutator.priority >= tileMutatorDef.priority)
						{
							this.mutatorsNullable.Remove(tileMutatorDef);
						}
						else
						{
							string str = "Detected mutator conflict: ";
							TileMutatorDef tileMutatorDef2 = tileMutatorDef;
							Log.Error(str + ((tileMutatorDef2 != null) ? tileMutatorDef2.ToString() : null) + " and " + ((mutator != null) ? mutator.ToString() : null));
						}
					}
				}
				foreach (string item2 in mutator.overrideCategories)
				{
					if (tileMutatorDef.categories.Contains(item2))
					{
						this.mutatorsNullable.Remove(tileMutatorDef);
					}
				}
			}
			this.mutatorsNullable.Add(mutator);
			this.mutatorsNullable.SortBy((TileMutatorDef m) => m.genOrder);
			TileMutatorWorker worker = mutator.Worker;
			if (worker == null)
			{
				return;
			}
			worker.OnAddedToTile(this.tile);
		}

		// Token: 0x0600DEAD RID: 57005 RVA: 0x0052CDC8 File Offset: 0x0052AFC8
		public void RemoveMutator(TileMutatorDef mutator)
		{
			List<TileMutatorDef> list = this.mutatorsNullable;
			if (list == null)
			{
				return;
			}
			list.Remove(mutator);
		}

		// Token: 0x0600DEAE RID: 57006 RVA: 0x0052CDDC File Offset: 0x0052AFDC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 8);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted<BiomeDef>(this.biome);
			defaultInterpolatedStringHandler.AppendLiteral(" elev=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.elevation);
			defaultInterpolatedStringHandler.AppendLiteral("m hill=");
			defaultInterpolatedStringHandler.AppendFormatted<Hilliness>(this.hilliness);
			defaultInterpolatedStringHandler.AppendLiteral(" temp=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.temperature);
			defaultInterpolatedStringHandler.AppendLiteral("°C rain=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.rainfall);
			defaultInterpolatedStringHandler.AppendLiteral("mm");
			defaultInterpolatedStringHandler.AppendLiteral(" swampiness=");
			defaultInterpolatedStringHandler.AppendFormatted(this.swampiness.ToStringPercent());
			defaultInterpolatedStringHandler.AppendLiteral(" (allowed=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.biome.allowRoads);
			defaultInterpolatedStringHandler.AppendLiteral(") (allowed=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.biome.allowRivers);
			defaultInterpolatedStringHandler.AppendLiteral("))");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600DEAF RID: 57007 RVA: 0x0052CEE9 File Offset: 0x0052B0E9
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			Tile.<GetGizmos>d__57 <GetGizmos>d__ = new Tile.<GetGizmos>d__57(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600DEB0 RID: 57008 RVA: 0x0052CEFC File Offset: 0x0052B0FC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<BiomeDef>(ref this.biome, "biome");
			Scribe_Values.Look<PlanetTile>(ref this.tile, "tile", default(PlanetTile), false);
			Scribe_Values.Look<float>(ref this.elevation, "elevation", 100f, false);
			Scribe_Values.Look<Hilliness>(ref this.hilliness, "hilliness", Hilliness.Undefined, false);
			Scribe_Values.Look<float>(ref this.temperature, "temperature", 20f, false);
			Scribe_Values.Look<float>(ref this.rainfall, "rainfall", 0f, false);
			Scribe_Values.Look<float>(ref this.swampiness, "swampiness", 0f, false);
			Scribe_Values.Look<float>(ref this.pollution, "pollution", 0f, false);
			Scribe_Deep.Look<WorldFeature>(ref this.feature, "feature", Array.Empty<object>());
			Scribe_Collections.Look<TileMutatorDef>(ref this.mutatorsNullable, "mutatorDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x04008EF2 RID: 36594
		private BiomeDef biome;

		// Token: 0x04008EF3 RID: 36595
		public float elevation = 100f;

		// Token: 0x04008EF4 RID: 36596
		public Hilliness hilliness;

		// Token: 0x04008EF5 RID: 36597
		public float temperature = 20f;

		// Token: 0x04008EF6 RID: 36598
		public float rainfall;

		// Token: 0x04008EF7 RID: 36599
		public float swampiness;

		// Token: 0x04008EF8 RID: 36600
		public WorldFeature feature;

		// Token: 0x04008EF9 RID: 36601
		public float pollution;

		// Token: 0x04008EFA RID: 36602
		public PlanetTile tile;

		// Token: 0x04008EFB RID: 36603
		public List<TileMutatorDef> mutatorsNullable;

		// Token: 0x04008EFC RID: 36604
		[Unsaved(false)]
		private bool? tmpHasSecondaryBiome;

		// Token: 0x04008EFD RID: 36605
		[Unsaved(false)]
		private BiomeDef tmpSecondaryBiome;

		// Token: 0x04008EFE RID: 36606
		private Hilliness? hillinessLabelCached;

		// Token: 0x04008EFF RID: 36607
		private float? cachedMaxTemp;

		// Token: 0x04008F00 RID: 36608
		private float? cachedMinTemp;
	}
}
