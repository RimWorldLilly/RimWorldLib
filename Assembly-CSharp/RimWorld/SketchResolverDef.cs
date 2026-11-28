using System;
using RimWorld.SketchGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE5 RID: 4069
	public class SketchResolverDef : Def
	{
		// Token: 0x060066EF RID: 26351 RVA: 0x00247D41 File Offset: 0x00245F41
		public void Resolve(SketchResolveParams parms)
		{
			this.resolver.Resolve(parms);
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x00247D4F File Offset: 0x00245F4F
		public bool CanResolve(SketchResolveParams parms)
		{
			return this.resolver.CanResolve(parms);
		}

		// Token: 0x0400404D RID: 16461
		public SketchResolver resolver;

		// Token: 0x0400404E RID: 16462
		public bool isRoot;
	}
}
