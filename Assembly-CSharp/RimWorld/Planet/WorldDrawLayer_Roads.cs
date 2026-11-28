using System;
using System.Collections;
using UnityEngine;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002321 RID: 8993
	public class WorldDrawLayer_Roads : WorldDrawLayer_Paths
	{
		// Token: 0x170026EC RID: 9964
		// (get) Token: 0x0600DE21 RID: 56865 RVA: 0x0052A421 File Offset: 0x00528621
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026ED RID: 9965
		// (get) Token: 0x0600DE22 RID: 56866 RVA: 0x0052A424 File Offset: 0x00528624
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DE23 RID: 56867 RVA: 0x0052A427 File Offset: 0x00528627
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Roads.<Regenerate>d__7 <Regenerate>d__ = new WorldDrawLayer_Roads.<Regenerate>d__7(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DE24 RID: 56868 RVA: 0x0052A438 File Offset: 0x00528638
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			Vector3 coordinate = inp * distortionFrequency;
			float magnitude = inp.magnitude;
			Vector3 a = new Vector3(this.roadDisplacementX.GetValue(coordinate), this.roadDisplacementY.GetValue(coordinate), this.roadDisplacementZ.GetValue(coordinate));
			if ((double)a.magnitude > 0.0001)
			{
				float d = (1f / (1f + Mathf.Exp(-a.magnitude / 1f * 2f)) * 2f - 1f) * 1f;
				a = a.normalized * d;
			}
			inp = (inp + a * distortionIntensity).normalized * magnitude;
			return inp + inp.normalized * 0.02f;
		}

		// Token: 0x04008EA0 RID: 36512
		private readonly ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

		// Token: 0x04008EA1 RID: 36513
		private readonly ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

		// Token: 0x04008EA2 RID: 36514
		private readonly ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);
	}
}
