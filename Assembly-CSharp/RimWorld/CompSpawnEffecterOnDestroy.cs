using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200197C RID: 6524
	public class CompSpawnEffecterOnDestroy : ThingComp
	{
		// Token: 0x17001C74 RID: 7284
		// (get) Token: 0x0600A3DA RID: 41946 RVA: 0x003A8CCF File Offset: 0x003A6ECF
		private CompProperties_SpawnEffecterOnDestroy Props
		{
			get
			{
				return (CompProperties_SpawnEffecterOnDestroy)this.props;
			}
		}

		// Token: 0x0600A3DB RID: 41947 RVA: 0x003A8CDC File Offset: 0x003A6EDC
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.Props.effect.Spawn(this.parent.Position, previousMap, 1f);
		}
	}
}
