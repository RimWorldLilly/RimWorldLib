using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BC RID: 6076
	public abstract class ScenPart_ScatterThings : ScenPart_ThingCount
	{
		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x06009790 RID: 38800
		protected abstract bool NearPlayerStart { get; }

		// Token: 0x06009791 RID: 38801 RVA: 0x0036FAF4 File Offset: 0x0036DCF4
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			new GenStep_ScatterThings
			{
				nearPlayerStart = this.NearPlayerStart,
				allowFoggedPositions = !this.NearPlayerStart,
				thingDef = this.thingDef,
				stuff = this.stuff,
				count = this.count,
				spotMustBeStandable = true,
				minSpacing = 5f,
				quality = this.quality,
				clusterSize = ((this.thingDef.category == ThingCategory.Building) ? 1 : 4),
				allowRoofed = this.allowRoofed,
				minify = true
			}.Generate(map, default(GenStepParams));
		}

		// Token: 0x06009792 RID: 38802 RVA: 0x0036FBA5 File Offset: 0x0036DDA5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.allowRoofed, "allowRoofed", false, false);
		}

		// Token: 0x06009793 RID: 38803 RVA: 0x0036FBBF File Offset: 0x0036DDBF
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.allowRoofed > false) ? 1 : 0);
		}

		// Token: 0x04005AC0 RID: 23232
		public bool allowRoofed = true;
	}
}
