using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000159 RID: 345
	public class SkyfallerProperties
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0003A1E7 File Offset: 0x000383E7
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0003A207 File Offset: 0x00038407
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x04000724 RID: 1828
		public bool hitRoof = true;

		// Token: 0x04000725 RID: 1829
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x04000726 RID: 1830
		public IntRange ticksToDiscardInReverse = IntRange.Zero;

		// Token: 0x04000727 RID: 1831
		public bool reversed;

		// Token: 0x04000728 RID: 1832
		public bool flightFlippedHorizontally;

		// Token: 0x04000729 RID: 1833
		public float explosionRadius = 3f;

		// Token: 0x0400072A RID: 1834
		public DamageDef explosionDamage;

		// Token: 0x0400072B RID: 1835
		public bool damageSpawnedThings;

		// Token: 0x0400072C RID: 1836
		public float explosionDamageFactor = 1f;

		// Token: 0x0400072D RID: 1837
		public IntRange metalShrapnelCountRange = IntRange.Zero;

		// Token: 0x0400072E RID: 1838
		public IntRange rubbleShrapnelCountRange = IntRange.Zero;

		// Token: 0x0400072F RID: 1839
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x04000730 RID: 1840
		public ThingDef spawnThing;

		// Token: 0x04000731 RID: 1841
		public bool minimalRoofDestruction;

		// Token: 0x04000732 RID: 1842
		public SkyfallerMovementType movementType;

		// Token: 0x04000733 RID: 1843
		public float speed = 1f;

		// Token: 0x04000734 RID: 1844
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x04000735 RID: 1845
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x04000736 RID: 1846
		public float cameraShake;

		// Token: 0x04000737 RID: 1847
		public SoundDef impactSound;

		// Token: 0x04000738 RID: 1848
		public bool rotateGraphicTowardsDirection;

		// Token: 0x04000739 RID: 1849
		public SoundDef anticipationSound;

		// Token: 0x0400073A RID: 1850
		public SoundDef floatingSound;

		// Token: 0x0400073B RID: 1851
		public int anticipationSoundTicks = 100;

		// Token: 0x0400073C RID: 1852
		public int motesPerCell = 3;

		// Token: 0x0400073D RID: 1853
		public float moteSpawnTime = float.MinValue;

		// Token: 0x0400073E RID: 1854
		public SimpleCurve xPositionCurve;

		// Token: 0x0400073F RID: 1855
		public SimpleCurve zPositionCurve;

		// Token: 0x04000740 RID: 1856
		public SimpleCurve angleCurve;

		// Token: 0x04000741 RID: 1857
		public SimpleCurve rotationCurve;

		// Token: 0x04000742 RID: 1858
		public SimpleCurve speedCurve;

		// Token: 0x04000743 RID: 1859
		public int fadeInTicks;

		// Token: 0x04000744 RID: 1860
		public int fadeOutTicks;
	}
}
