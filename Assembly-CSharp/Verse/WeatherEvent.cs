using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000325 RID: 805
	public abstract class WeatherEvent
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060018B0 RID: 6320
		public abstract bool Expired { get; }

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060018B1 RID: 6321 RVA: 0x0009454F File Offset: 0x0009274F
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060018B2 RID: 6322 RVA: 0x0009455E File Offset: 0x0009275E
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00094565 File Offset: 0x00092765
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x0009456C File Offset: 0x0009276C
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x00094582 File Offset: 0x00092782
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x060018B6 RID: 6326
		public abstract void FireEvent();

		// Token: 0x060018B7 RID: 6327
		public abstract void WeatherEventTick();

		// Token: 0x060018B8 RID: 6328 RVA: 0x00094591 File Offset: 0x00092791
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x04001486 RID: 5254
		protected Map map;
	}
}
