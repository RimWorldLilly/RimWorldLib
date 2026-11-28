using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200022D RID: 557
	public static class Altitudes
	{
		// Token: 0x0600102D RID: 4141 RVA: 0x0005D6A0 File Offset: 0x0005B8A0
		static Altitudes()
		{
			for (int i = 0; i < 40; i++)
			{
				Altitudes.Alts[i] = (float)i * 0.36585367f;
			}
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0005D6EE File Offset: 0x0005B8EE
		public static float AltitudeFor(this AltitudeLayer alt)
		{
			return Altitudes.Alts[(int)alt];
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0005D6F7 File Offset: 0x0005B8F7
		public static float AltitudeFor(this AltitudeLayer alt, float incOffset)
		{
			return alt.AltitudeFor() + incOffset * 0.03658537f;
		}

		// Token: 0x04000F2F RID: 3887
		private const int NumAltitudeLayers = 40;

		// Token: 0x04000F30 RID: 3888
		private static readonly float[] Alts = new float[40];

		// Token: 0x04000F31 RID: 3889
		private const float LayerSpacing = 0.36585367f;

		// Token: 0x04000F32 RID: 3890
		public const float AltInc = 0.03658537f;

		// Token: 0x04000F33 RID: 3891
		public static readonly Vector3 AltIncVect = new Vector3(0f, 0.03658537f, 0f);
	}
}
