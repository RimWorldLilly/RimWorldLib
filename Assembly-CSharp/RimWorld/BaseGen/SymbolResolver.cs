using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002153 RID: 8531
	public abstract class SymbolResolver
	{
		// Token: 0x0600D53A RID: 54586 RVA: 0x004F17A8 File Offset: 0x004EF9A8
		public virtual bool CanResolve(ResolveParams rp)
		{
			return rp.rect.Width >= this.minRectSize.x && rp.rect.Height >= this.minRectSize.z;
		}

		// Token: 0x0600D53B RID: 54587
		public abstract void Resolve(ResolveParams rp);

		// Token: 0x04008725 RID: 34597
		public IntVec2 minRectSize = IntVec2.One;

		// Token: 0x04008726 RID: 34598
		public float selectionWeight = 1f;
	}
}
