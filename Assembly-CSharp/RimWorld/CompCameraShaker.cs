using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001843 RID: 6211
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x06009B7F RID: 39807 RVA: 0x00380CB5 File Offset: 0x0037EEB5
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x06009B80 RID: 39808 RVA: 0x00380CC4 File Offset: 0x0037EEC4
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned && this.parent.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.SetMinShake(this.Props.mag);
			}
		}
	}
}
