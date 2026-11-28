using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002322 RID: 8994
	public class WorldDrawLayer_Satellites : WorldDrawLayer
	{
		// Token: 0x170026EE RID: 9966
		// (get) Token: 0x0600DE27 RID: 56871 RVA: 0x0052A5B2 File Offset: 0x005287B2
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x170026EF RID: 9967
		// (get) Token: 0x0600DE28 RID: 56872 RVA: 0x0052A5B9 File Offset: 0x005287B9
		public override bool ShouldRegenerate
		{
			get
			{
				return this.lastUpdate != GenTicks.TicksGame;
			}
		}

		// Token: 0x0600DE29 RID: 56873 RVA: 0x0052A5CB File Offset: 0x005287CB
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Satellites.<Regenerate>d__9 <Regenerate>d__ = new WorldDrawLayer_Satellites.<Regenerate>d__9(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008EA3 RID: 36515
		private const float DistanceToOrbit = 25f;

		// Token: 0x04008EA4 RID: 36516
		private Material material;

		// Token: 0x04008EA5 RID: 36517
		private int lastUpdate;

		// Token: 0x04008EA6 RID: 36518
		private List<WorldDrawLayer_Satellites.Orbit> orbits;

		// Token: 0x02003E00 RID: 15872
		private struct Orbit
		{
			// Token: 0x0400F4A0 RID: 62624
			public float inclination;

			// Token: 0x0400F4A1 RID: 62625
			public float angleOfAscendingNode;

			// Token: 0x0400F4A2 RID: 62626
			public float altitude;

			// Token: 0x0400F4A3 RID: 62627
			public float rotationAngle;

			// Token: 0x0400F4A4 RID: 62628
			public float angleDeltaSecond;
		}
	}
}
