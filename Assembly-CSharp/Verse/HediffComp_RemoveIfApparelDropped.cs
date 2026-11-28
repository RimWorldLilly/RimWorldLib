using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000470 RID: 1136
	public class HediffComp_RemoveIfApparelDropped : HediffComp
	{
		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x000C0F56 File Offset: 0x000BF156
		public HediffCompProperties_RemoveIfApparelDropped Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfApparelDropped)this.props;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x000C0F63 File Offset: 0x000BF163
		public override bool CompShouldRemove
		{
			get
			{
				return !this.parent.pawn.apparel.Wearing(this.wornApparel);
			}
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000C0F83 File Offset: 0x000BF183
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Apparel>(ref this.wornApparel, "wornApparel", false);
		}

		// Token: 0x040018D8 RID: 6360
		public Apparel wornApparel;
	}
}
