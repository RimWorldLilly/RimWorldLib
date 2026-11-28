using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001731 RID: 5937
	public class CompProperties_RitualVisualEffect
	{
		// Token: 0x0600940A RID: 37898 RVA: 0x0035FAE4 File Offset: 0x0035DCE4
		public CompProperties_RitualVisualEffect()
		{
		}

		// Token: 0x0600940B RID: 37899 RVA: 0x0035FB60 File Offset: 0x0035DD60
		public CompProperties_RitualVisualEffect(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x0600940C RID: 37900 RVA: 0x0035FBE2 File Offset: 0x0035DDE2
		public RitualVisualEffectComp GetInstance()
		{
			return (RitualVisualEffectComp)Activator.CreateInstance(this.compClass);
		}

		// Token: 0x04005963 RID: 22883
		public Type compClass = typeof(RitualVisualEffectComp);

		// Token: 0x04005964 RID: 22884
		public ThingDef moteDef;

		// Token: 0x04005965 RID: 22885
		public FleckDef fleckDef;

		// Token: 0x04005966 RID: 22886
		public EffecterDef effecterDef;

		// Token: 0x04005967 RID: 22887
		public FloatRange velocity = FloatRange.Zero;

		// Token: 0x04005968 RID: 22888
		public Vector3 velocityDir = Vector3.zero;

		// Token: 0x04005969 RID: 22889
		public FloatRange rotation = FloatRange.Zero;

		// Token: 0x0400596A RID: 22890
		public FloatRange rotationRate = FloatRange.Zero;

		// Token: 0x0400596B RID: 22891
		public FloatRange scale = FloatRange.One;

		// Token: 0x0400596C RID: 22892
		public Vector3 offset = Vector3.zero;

		// Token: 0x0400596D RID: 22893
		public IntVec3 roomCheckOffset = IntVec3.Zero;

		// Token: 0x0400596E RID: 22894
		public bool scaleWithRoom;

		// Token: 0x0400596F RID: 22895
		public bool scalePositionWithRoom;

		// Token: 0x04005970 RID: 22896
		public bool onlySpawnInSameRoom;

		// Token: 0x04005971 RID: 22897
		public Color? colorOverride;

		// Token: 0x04005972 RID: 22898
		public List<Color> overrideColors = new List<Color>();
	}
}
