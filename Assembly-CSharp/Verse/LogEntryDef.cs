using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000183 RID: 387
	public class LogEntryDef : Def
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x0003CD5E File Offset: 0x0003AF5E
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.iconMiss.NullOrEmpty())
				{
					this.iconMissTex = ContentFinder<Texture2D>.Get(this.iconMiss, true);
				}
				if (!this.iconDamaged.NullOrEmpty())
				{
					this.iconDamagedTex = ContentFinder<Texture2D>.Get(this.iconDamaged, true);
				}
				if (!this.iconDamagedFromInstigator.NullOrEmpty())
				{
					this.iconDamagedFromInstigatorTex = ContentFinder<Texture2D>.Get(this.iconDamagedFromInstigator, true);
				}
			});
		}

		// Token: 0x04000964 RID: 2404
		[NoTranslate]
		public string iconMiss;

		// Token: 0x04000965 RID: 2405
		[NoTranslate]
		public string iconDamaged;

		// Token: 0x04000966 RID: 2406
		[NoTranslate]
		public string iconDamagedFromInstigator;

		// Token: 0x04000967 RID: 2407
		[Unsaved(false)]
		public Texture2D iconMissTex;

		// Token: 0x04000968 RID: 2408
		[Unsaved(false)]
		public Texture2D iconDamagedTex;

		// Token: 0x04000969 RID: 2409
		[Unsaved(false)]
		public Texture2D iconDamagedFromInstigatorTex;
	}
}
