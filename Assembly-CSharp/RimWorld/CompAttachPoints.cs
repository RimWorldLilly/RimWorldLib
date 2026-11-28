using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001824 RID: 6180
	public class CompAttachPoints : ThingComp
	{
		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x06009A95 RID: 39573 RVA: 0x0037C2B9 File Offset: 0x0037A4B9
		public CompProperties_AttachPoints Props
		{
			get
			{
				return (CompProperties_AttachPoints)this.props;
			}
		}

		// Token: 0x06009A96 RID: 39574 RVA: 0x0037C2C6 File Offset: 0x0037A4C6
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.points = new AttachPointTracker(this.Props.points, this.parent);
		}

		// Token: 0x04005BE8 RID: 23528
		public AttachPointTracker points;
	}
}
