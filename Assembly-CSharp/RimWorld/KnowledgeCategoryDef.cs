using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F82 RID: 3970
	public class KnowledgeCategoryDef : Def
	{
		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06006599 RID: 26009 RVA: 0x00242EEF File Offset: 0x002410EF
		public Texture2D Tex
		{
			get
			{
				return this.texInt;
			}
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x00242EF7 File Offset: 0x002410F7
		public override IEnumerable<string> ConfigErrors()
		{
			KnowledgeCategoryDef.<ConfigErrors>d__6 <ConfigErrors>d__ = new KnowledgeCategoryDef.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x00242F07 File Offset: 0x00241107
		public override void PostLoad()
		{
			if (!this.texPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.texInt = ContentFinder<Texture2D>.Get(this.texPath, true);
				});
			}
		}

		// Token: 0x04003C50 RID: 15440
		public KnowledgeCategoryDef overflowCategory;

		// Token: 0x04003C51 RID: 15441
		public Color color;

		// Token: 0x04003C52 RID: 15442
		[NoTranslate]
		public string texPath;

		// Token: 0x04003C53 RID: 15443
		[Unsaved(false)]
		private Texture2D texInt;
	}
}
