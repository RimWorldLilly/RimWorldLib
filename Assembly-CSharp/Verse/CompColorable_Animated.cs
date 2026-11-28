using System;

namespace Verse
{
	// Token: 0x0200053F RID: 1343
	public class CompColorable_Animated : CompColorable
	{
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002774 RID: 10100 RVA: 0x000F6718 File Offset: 0x000F4918
		public CompProperties_ColorableAnimated Props
		{
			get
			{
				return (CompProperties_ColorableAnimated)this.props;
			}
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000F6728 File Offset: 0x000F4928
		public override void Initialize(CompProperties props)
		{
			this.props = props;
			if (this.Props.startWithRandom)
			{
				this.colorOffset = Rand.RangeInclusive(0, this.Props.colors.Count - 1);
			}
			base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000F6794 File Offset: 0x000F4994
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.Props.changeInterval))
			{
				base.SetColor(this.Props.colors[this.colorOffset % this.Props.colors.Count]);
				this.colorOffset++;
			}
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000F67FA File Offset: 0x000F49FA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.colorOffset, "colorOffset", 0, false);
		}

		// Token: 0x04001C4B RID: 7243
		public int colorOffset;
	}
}
