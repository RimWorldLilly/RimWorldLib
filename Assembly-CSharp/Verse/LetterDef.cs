using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000182 RID: 386
	public class LetterDef : Def
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x0003CCB7 File Offset: 0x0003AEB7
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003CCEC File Offset: 0x0003AEEC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		// Token: 0x0400095A RID: 2394
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x0400095B RID: 2395
		public Color color = Color.white;

		// Token: 0x0400095C RID: 2396
		public Color flashColor = Color.white;

		// Token: 0x0400095D RID: 2397
		public float flashInterval = 90f;

		// Token: 0x0400095E RID: 2398
		public bool bounce;

		// Token: 0x0400095F RID: 2399
		public SoundDef arriveSound;

		// Token: 0x04000960 RID: 2400
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x04000961 RID: 2401
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		// Token: 0x04000962 RID: 2402
		public bool forcedSlowdown;

		// Token: 0x04000963 RID: 2403
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
