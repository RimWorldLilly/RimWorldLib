using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002BE RID: 702
	public struct CellTerrain
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x0007F7C3 File Offset: 0x0007D9C3
		public CellTerrain(TerrainDef def, bool polluted, float snowCoverage, float sandCoverage, ColorDef color)
		{
			this.def = def;
			this.polluted = polluted;
			this.snowCoverage = snowCoverage;
			this.sandCoverage = sandCoverage;
			this.color = color;
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0007F7EC File Offset: 0x0007D9EC
		public override bool Equals(object obj)
		{
			if (obj is CellTerrain)
			{
				CellTerrain terrain = (CellTerrain)obj;
				return this.Equals(terrain);
			}
			return false;
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0007F814 File Offset: 0x0007DA14
		public bool Equals(CellTerrain terrain)
		{
			return terrain.def == this.def && terrain.color == this.color && terrain.polluted == this.polluted && Mathf.Abs(terrain.snowCoverage - this.snowCoverage) < float.Epsilon && Mathf.Abs(terrain.sandCoverage - this.sandCoverage) < float.Epsilon;
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0007F87F File Offset: 0x0007DA7F
		public override int GetHashCode()
		{
			return Gen.HashCombine<ColorDef>(Gen.HashCombine<float>(Gen.HashCombine<float>(Gen.HashCombine<bool>(Gen.HashCombine<TerrainDef>(0, this.def), this.polluted), this.snowCoverage), this.sandCoverage), this.color);
		}

		// Token: 0x040012AD RID: 4781
		public TerrainDef def;

		// Token: 0x040012AE RID: 4782
		public bool polluted;

		// Token: 0x040012AF RID: 4783
		public float snowCoverage;

		// Token: 0x040012B0 RID: 4784
		public float sandCoverage;

		// Token: 0x040012B1 RID: 4785
		public ColorDef color;
	}
}
