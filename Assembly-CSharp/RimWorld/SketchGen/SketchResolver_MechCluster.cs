using System;

namespace RimWorld.SketchGen
{
	// Token: 0x020020D0 RID: 8400
	public class SketchResolver_MechCluster : SketchResolver
	{
		// Token: 0x0600D373 RID: 54131 RVA: 0x004DEFDE File Offset: 0x004DD1DE
		protected override void ResolveInt(SketchResolveParams parms)
		{
			MechClusterGenerator.ResolveSketch(parms);
		}

		// Token: 0x0600D374 RID: 54132 RVA: 0x004DEFE6 File Offset: 0x004DD1E6
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}
	}
}
