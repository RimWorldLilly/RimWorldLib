using System;
using System.Collections;
using UnityEngine;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002320 RID: 8992
	public class WorldDrawLayer_Rivers : WorldDrawLayer_Paths
	{
		// Token: 0x170026EA RID: 9962
		// (get) Token: 0x0600DE1B RID: 56859 RVA: 0x0052A2D3 File Offset: 0x005284D3
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026EB RID: 9963
		// (get) Token: 0x0600DE1C RID: 56860 RVA: 0x0052A2D6 File Offset: 0x005284D6
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DE1D RID: 56861 RVA: 0x0052A2DC File Offset: 0x005284DC
		public WorldDrawLayer_Rivers()
		{
			this.pointyEnds = true;
		}

		// Token: 0x0600DE1E RID: 56862 RVA: 0x0052A393 File Offset: 0x00528593
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Rivers.<Regenerate>d__11 <Regenerate>d__ = new WorldDrawLayer_Rivers.<Regenerate>d__11(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DE1F RID: 56863 RVA: 0x0052A3A4 File Offset: 0x005285A4
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
			return inp + inp.normalized * 0.01f;
		}

		// Token: 0x04008E9A RID: 36506
		private readonly Color32 riverColor = new Color32(73, 82, 100, byte.MaxValue);

		// Token: 0x04008E9B RID: 36507
		private const float PerlinFrequency = 0.6f;

		// Token: 0x04008E9C RID: 36508
		private const float PerlinMagnitude = 0.1f;

		// Token: 0x04008E9D RID: 36509
		private readonly ModuleBase riverDisplacementX = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		// Token: 0x04008E9E RID: 36510
		private readonly ModuleBase riverDisplacementY = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		// Token: 0x04008E9F RID: 36511
		private readonly ModuleBase riverDisplacementZ = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 91572032, QualityMode.Medium);
	}
}
