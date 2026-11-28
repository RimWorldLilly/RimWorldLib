using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001005 RID: 4101
	public class TrainableDef : Def
	{
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x06006770 RID: 26480 RVA: 0x0024977D File Offset: 0x0024797D
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null)
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x002497A8 File Offset: 0x002479A8
		public bool MatchesTag(string tag)
		{
			if (tag == this.defName)
			{
				return true;
			}
			using (List<string>.Enumerator enumerator = this.tags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == tag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006772 RID: 26482 RVA: 0x00249814 File Offset: 0x00247A14
		public override IEnumerable<string> ConfigErrors()
		{
			TrainableDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new TrainableDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004165 RID: 16741
		public float difficulty = -1f;

		// Token: 0x04004166 RID: 16742
		public float minBodySize;

		// Token: 0x04004167 RID: 16743
		public List<TrainableDef> prerequisites;

		// Token: 0x04004168 RID: 16744
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04004169 RID: 16745
		public bool defaultTrainable;

		// Token: 0x0400416A RID: 16746
		public TrainabilityDef requiredTrainability;

		// Token: 0x0400416B RID: 16747
		public int steps = 1;

		// Token: 0x0400416C RID: 16748
		public float listPriority;

		// Token: 0x0400416D RID: 16749
		[NoTranslate]
		public string icon;

		// Token: 0x0400416E RID: 16750
		public bool specialTrainable;

		// Token: 0x0400416F RID: 16751
		[Obsolete]
		public AbilityDef enablesAbility;

		// Token: 0x04004170 RID: 16752
		[Unsaved(false)]
		public int indent;

		// Token: 0x04004171 RID: 16753
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
