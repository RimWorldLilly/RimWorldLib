using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006C7 RID: 1735
	public class SubEffecter
	{
		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x00160944 File Offset: 0x0015EB44
		public Color EffectiveColor
		{
			get
			{
				Color? color = this.colorOverride;
				if (color == null)
				{
					return this.def.color;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060036FB RID: 14075 RVA: 0x00160974 File Offset: 0x0015EB74
		public MoteSpawnLocType EffectiveSpawnLocType
		{
			get
			{
				MoteSpawnLocType? moteSpawnLocType = this.spawnLocOverride;
				if (moteSpawnLocType == null)
				{
					return this.def.spawnLocType;
				}
				return moteSpawnLocType.GetValueOrDefault();
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x001609A4 File Offset: 0x0015EBA4
		public Vector3? EffectiveDimensions
		{
			get
			{
				Vector3? result = this.dimensionsOverride;
				if (result == null)
				{
					return this.def.positionDimensions;
				}
				return result;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x001609D0 File Offset: 0x0015EBD0
		public Vector3 EffectiveOffset
		{
			get
			{
				Vector3? vector = this.offsetOverride;
				if (vector == null)
				{
					return this.def.positionOffset;
				}
				return vector.GetValueOrDefault();
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060036FE RID: 14078 RVA: 0x00160A00 File Offset: 0x0015EC00
		public float EffectiveChancePerTick
		{
			get
			{
				float? num = this.chanceOverride;
				if (num == null)
				{
					return this.def.chancePerTick;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x00160A30 File Offset: 0x0015EC30
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x00160A46 File Offset: 0x0015EC46
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x00160A48 File Offset: 0x0015EC48
		public virtual void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x00160A4A File Offset: 0x0015EC4A
		public virtual void SubCleanup()
		{
		}

		// Token: 0x04002427 RID: 9255
		public Effecter parent;

		// Token: 0x04002428 RID: 9256
		public SubEffecterDef def;

		// Token: 0x04002429 RID: 9257
		public Color? colorOverride;

		// Token: 0x0400242A RID: 9258
		public MoteSpawnLocType? spawnLocOverride;

		// Token: 0x0400242B RID: 9259
		public Vector3? dimensionsOverride;

		// Token: 0x0400242C RID: 9260
		public Vector3? offsetOverride;

		// Token: 0x0400242D RID: 9261
		public float? chanceOverride;
	}
}
