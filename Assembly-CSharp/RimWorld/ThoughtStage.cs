using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001001 RID: 4097
	public class ThoughtStage
	{
		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x06006750 RID: 26448 RVA: 0x0024921D File Offset: 0x0024741D
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x06006751 RID: 26449 RVA: 0x0024923E File Offset: 0x0024743E
		public string LabelSocialCap
		{
			get
			{
				if (this.cachedLabelSocialCap == null)
				{
					this.cachedLabelSocialCap = this.labelSocial.CapitalizeFirst();
				}
				return this.cachedLabelSocialCap;
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06006752 RID: 26450 RVA: 0x0024925F File Offset: 0x0024745F
		public string LabelAbstractCap
		{
			get
			{
				if (this.cachedLabelAbstractCap == null)
				{
					this.cachedLabelAbstractCap = ((this.labelAbstract != null) ? this.labelAbstract.CapitalizeFirst() : this.label.CapitalizeFirst());
				}
				return this.cachedLabelAbstractCap;
			}
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x00249295 File Offset: 0x00247495
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06006754 RID: 26452 RVA: 0x002492AF File Offset: 0x002474AF
		public IEnumerable<string> ConfigErrors()
		{
			ThoughtStage.<ConfigErrors>d__19 <ConfigErrors>d__ = new ThoughtStage.<ConfigErrors>d__19(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004119 RID: 16665
		[MustTranslate]
		public string label;

		// Token: 0x0400411A RID: 16666
		[MustTranslate]
		public string labelSocial;

		// Token: 0x0400411B RID: 16667
		[MustTranslate]
		public string labelAbstract;

		// Token: 0x0400411C RID: 16668
		[MustTranslate]
		public string description;

		// Token: 0x0400411D RID: 16669
		public float baseMoodEffect;

		// Token: 0x0400411E RID: 16670
		public float baseOpinionOffset;

		// Token: 0x0400411F RID: 16671
		public bool visible = true;

		// Token: 0x04004120 RID: 16672
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04004121 RID: 16673
		[Unsaved(false)]
		private string cachedLabelSocialCap;

		// Token: 0x04004122 RID: 16674
		[Unsaved(false)]
		private string cachedLabelAbstractCap;

		// Token: 0x04004123 RID: 16675
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04004124 RID: 16676
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelSocial;
	}
}
