using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002CB RID: 715
	public sealed class MapInfo : IExposable
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x000823C4 File Offset: 0x000805C4
		public PlanetTile Tile
		{
			get
			{
				MapParent mapParent = this.parent;
				if (mapParent == null)
				{
					return PlanetTile.Invalid;
				}
				return mapParent.Tile;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x000823DB File Offset: 0x000805DB
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x00082400 File Offset: 0x00080600
		// (set) Token: 0x060015EE RID: 5614 RVA: 0x00082408 File Offset: 0x00080608
		public IntVec3 Size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00082414 File Offset: 0x00080614
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.isPocketMap, "isPocketMap", false, false);
			Scribe_Values.Look<bool>(ref this.disableSunShadows, "disableSunShadows", false, false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x040012E5 RID: 4837
		private IntVec3 sizeInt;

		// Token: 0x040012E6 RID: 4838
		public MapParent parent;

		// Token: 0x040012E7 RID: 4839
		public bool isPocketMap;

		// Token: 0x040012E8 RID: 4840
		public bool disableSunShadows;
	}
}
