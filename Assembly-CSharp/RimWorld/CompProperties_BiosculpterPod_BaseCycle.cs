using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001830 RID: 6192
	public abstract class CompProperties_BiosculpterPod_BaseCycle : CompProperties
	{
		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x06009B28 RID: 39720 RVA: 0x0037F3D4 File Offset: 0x0037D5D4
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x06009B29 RID: 39721 RVA: 0x0037F3FC File Offset: 0x0037D5FC
		public string LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x04005C40 RID: 23616
		[NoTranslate]
		public string key;

		// Token: 0x04005C41 RID: 23617
		[MustTranslate]
		public string label;

		// Token: 0x04005C42 RID: 23618
		[MustTranslate]
		public string description;

		// Token: 0x04005C43 RID: 23619
		[NoTranslate]
		public string iconPath;

		// Token: 0x04005C44 RID: 23620
		public float durationDays;

		// Token: 0x04005C45 RID: 23621
		public Color operatingColor = new Color(0.5f, 0.7f, 0.5f);

		// Token: 0x04005C46 RID: 23622
		public ThoughtDef gainThoughtOnCompletion;

		// Token: 0x04005C47 RID: 23623
		public List<ResearchProjectDef> requiredResearch;

		// Token: 0x04005C48 RID: 23624
		public List<ThingDefCountClass> extraRequiredIngredients;

		// Token: 0x04005C49 RID: 23625
		private Texture2D icon;
	}
}
