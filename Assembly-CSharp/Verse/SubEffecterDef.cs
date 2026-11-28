using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000169 RID: 361
	public class SubEffecterDef
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x0003BB9A File Offset: 0x00039D9A
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x0400080E RID: 2062
		public Type subEffecterClass;

		// Token: 0x0400080F RID: 2063
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x04000810 RID: 2064
		public int ticksBetweenMotes = 40;

		// Token: 0x04000811 RID: 2065
		public int maxMoteCount = int.MaxValue;

		// Token: 0x04000812 RID: 2066
		public int initialDelayTicks;

		// Token: 0x04000813 RID: 2067
		public int lifespanMaxTicks = 999999;

		// Token: 0x04000814 RID: 2068
		public float chancePerTick = 0.1f;

		// Token: 0x04000815 RID: 2069
		public int chancePeriodTicks;

		// Token: 0x04000816 RID: 2070
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x04000817 RID: 2071
		public float positionLerpFactor = 0.5f;

		// Token: 0x04000818 RID: 2072
		public Vector3 positionOffset = Vector3.zero;

		// Token: 0x04000819 RID: 2073
		public float positionRadius;

		// Token: 0x0400081A RID: 2074
		public float positionRadiusMin;

		// Token: 0x0400081B RID: 2075
		public List<Vector3> perRotationOffsets;

		// Token: 0x0400081C RID: 2076
		public Vector3? positionDimensions;

		// Token: 0x0400081D RID: 2077
		public bool attachToSpawnThing;

		// Token: 0x0400081E RID: 2078
		public float avoidLastPositionRadius;

		// Token: 0x0400081F RID: 2079
		public AttachPointType attachPoint;

		// Token: 0x04000820 RID: 2080
		public ThingDef moteDef;

		// Token: 0x04000821 RID: 2081
		public FleckDef fleckDef;

		// Token: 0x04000822 RID: 2082
		public Color color = Color.white;

		// Token: 0x04000823 RID: 2083
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x04000824 RID: 2084
		public bool absoluteAngle;

		// Token: 0x04000825 RID: 2085
		public bool useTargetAInitialRotation;

		// Token: 0x04000826 RID: 2086
		public bool useTargetBInitialRotation;

		// Token: 0x04000827 RID: 2087
		public bool fleckUsesAngleForVelocity;

		// Token: 0x04000828 RID: 2088
		public bool rotateTowardsTargetCenter;

		// Token: 0x04000829 RID: 2089
		public bool useTargetABodyAngle;

		// Token: 0x0400082A RID: 2090
		public bool useTargetBBodyAngle;

		// Token: 0x0400082B RID: 2091
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x0400082C RID: 2092
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x0400082D RID: 2093
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x0400082E RID: 2094
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x0400082F RID: 2095
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04000830 RID: 2096
		public SoundDef soundDef;

		// Token: 0x04000831 RID: 2097
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04000832 RID: 2098
		public int ticksBeforeSustainerStart;

		// Token: 0x04000833 RID: 2099
		public bool orbitOrigin;

		// Token: 0x04000834 RID: 2100
		public FloatRange orbitSpeed;

		// Token: 0x04000835 RID: 2101
		public float orbitSnapStrength;

		// Token: 0x04000836 RID: 2102
		public bool makeMoteOnSubtrigger;

		// Token: 0x04000837 RID: 2103
		public bool destroyMoteOnCleanup;

		// Token: 0x04000838 RID: 2104
		public FloatRange cameraShake;

		// Token: 0x04000839 RID: 2105
		public float distanceAttenuationScale;

		// Token: 0x0400083A RID: 2106
		public float distanceAttenuationMax = 100f;

		// Token: 0x0400083B RID: 2107
		public float randomWeight = 1f;

		// Token: 0x0400083C RID: 2108
		public bool subTriggerOnSpawn = true;

		// Token: 0x0400083D RID: 2109
		public List<SubEffecterDef> children;
	}
}
