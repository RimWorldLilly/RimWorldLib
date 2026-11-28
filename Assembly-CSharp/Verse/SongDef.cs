using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityStubs;

namespace Verse
{
	// Token: 0x020001BD RID: 445
	public class SongDef : Def
	{
		// Token: 0x06000C45 RID: 3141 RVA: 0x00040B34 File Offset: 0x0003ED34
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				string[] array = this.clipPath.Split(new char[]
				{
					'/',
					'\\'
				});
				this.defName = array[array.Length - 1];
				base.ResolveDefNameHash();
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00040B88 File Offset: 0x0003ED88
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.clip = ContentFinder<AudioClip>.Get(this.clipPath, true);
				ImportDefaults.KeepStereo(this.clip);
			});
		}

		// Token: 0x04000B97 RID: 2967
		[NoTranslate]
		public string clipPath;

		// Token: 0x04000B98 RID: 2968
		public float volume = 1f;

		// Token: 0x04000B99 RID: 2969
		public bool playOnMap = true;

		// Token: 0x04000B9A RID: 2970
		public float commonality = 1f;

		// Token: 0x04000B9B RID: 2971
		public bool tense;

		// Token: 0x04000B9C RID: 2972
		public TimeOfDay allowedTimeOfDay = TimeOfDay.Any;

		// Token: 0x04000B9D RID: 2973
		public List<Season> allowedSeasons;

		// Token: 0x04000B9E RID: 2974
		public RoyalTitleDef minRoyalTitle;

		// Token: 0x04000B9F RID: 2975
		[Unsaved(false)]
		public AudioClip clip;
	}
}
