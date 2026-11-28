using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002300 RID: 8960
	public readonly struct PlanetTile : IEquatable<PlanetTile>
	{
		// Token: 0x170026A8 RID: 9896
		// (get) Token: 0x0600DD56 RID: 56662 RVA: 0x00526C7B File Offset: 0x00524E7B
		public Tile Tile
		{
			get
			{
				return this.Layer[this.tileId];
			}
		}

		// Token: 0x170026A9 RID: 9897
		// (get) Token: 0x0600DD57 RID: 56663 RVA: 0x00526C8E File Offset: 0x00524E8E
		public PlanetLayer Layer
		{
			get
			{
				if (this.layerId < 0)
				{
					return Find.WorldGrid.Surface;
				}
				return Find.WorldGrid.PlanetLayers[this.layerId];
			}
		}

		// Token: 0x170026AA RID: 9898
		// (get) Token: 0x0600DD58 RID: 56664 RVA: 0x00526CB9 File Offset: 0x00524EB9
		public PlanetLayerDef LayerDef
		{
			get
			{
				return this.Layer.Def;
			}
		}

		// Token: 0x170026AB RID: 9899
		// (get) Token: 0x0600DD59 RID: 56665 RVA: 0x00526CC6 File Offset: 0x00524EC6
		public bool Valid
		{
			get
			{
				return this.tileId >= 0;
			}
		}

		// Token: 0x0600DD5A RID: 56666 RVA: 0x00526CD4 File Offset: 0x00524ED4
		public PlanetTile(int tileId, PlanetLayer layer)
		{
			this = new PlanetTile(tileId, layer.LayerID);
		}

		// Token: 0x0600DD5B RID: 56667 RVA: 0x00526CE3 File Offset: 0x00524EE3
		public PlanetTile(int tileId)
		{
			this.tileId = tileId;
			this.layerId = 0;
		}

		// Token: 0x0600DD5C RID: 56668 RVA: 0x00526CF3 File Offset: 0x00524EF3
		public PlanetTile(int tileId, int layerId)
		{
			this.tileId = tileId;
			this.layerId = layerId;
		}

		// Token: 0x0600DD5D RID: 56669 RVA: 0x00526D04 File Offset: 0x00524F04
		public bool Equals(PlanetTile other)
		{
			if (this.tileId != other.tileId)
			{
				return false;
			}
			if (this.layerId == other.layerId)
			{
				return true;
			}
			bool flag = this.layerId < 0 || this.Layer.IsRootSurface;
			bool flag2 = other.layerId < 0 || other.Layer.IsRootSurface;
			return flag && flag2;
		}

		// Token: 0x0600DD5E RID: 56670 RVA: 0x00526D64 File Offset: 0x00524F64
		public override bool Equals(object obj)
		{
			if (obj is PlanetTile)
			{
				PlanetTile other = (PlanetTile)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600DD5F RID: 56671 RVA: 0x00526D8C File Offset: 0x00524F8C
		public override int GetHashCode()
		{
			bool flag = this.layerId < 0 || this.Layer.IsRootSurface;
			return this.tileId * 397 ^ (flag ? 0 : this.layerId);
		}

		// Token: 0x0600DD60 RID: 56672 RVA: 0x00526DCA File Offset: 0x00524FCA
		public static bool operator ==(PlanetTile lhs, PlanetTile rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600DD61 RID: 56673 RVA: 0x00526DD4 File Offset: 0x00524FD4
		public static bool operator !=(PlanetTile lhs, PlanetTile rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600DD62 RID: 56674 RVA: 0x00526DE0 File Offset: 0x00524FE0
		public static implicit operator int(PlanetTile tile)
		{
			return tile.tileId;
		}

		// Token: 0x0600DD63 RID: 56675 RVA: 0x00526DE8 File Offset: 0x00524FE8
		public static implicit operator PlanetTile(int tileId)
		{
			return new PlanetTile(tileId);
		}

		// Token: 0x0600DD64 RID: 56676 RVA: 0x00526DF0 File Offset: 0x00524FF0
		public static bool TryParse(string str, out PlanetTile tile)
		{
			string[] array = str.Split(',', StringSplitOptions.None);
			tile = PlanetTile.Invalid;
			int num;
			int num2;
			if (array.Length == 1 && int.TryParse(array[0], out num))
			{
				tile = new PlanetTile(num);
			}
			else if (array.Length == 2 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2))
			{
				tile = new PlanetTile(num, num2);
			}
			return tile.Valid;
		}

		// Token: 0x0600DD65 RID: 56677 RVA: 0x00526E64 File Offset: 0x00525064
		public static PlanetTile FromString(string str)
		{
			PlanetTile result;
			if (PlanetTile.TryParse(str, out result))
			{
				return result;
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x0600DD66 RID: 56678 RVA: 0x00526E84 File Offset: 0x00525084
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.tileId);
			defaultInterpolatedStringHandler.AppendLiteral(",");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.layerId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04008E08 RID: 36360
		public readonly int tileId;

		// Token: 0x04008E09 RID: 36361
		private readonly int layerId;

		// Token: 0x04008E0A RID: 36362
		public static readonly PlanetTile Invalid = new PlanetTile(-1);
	}
}
