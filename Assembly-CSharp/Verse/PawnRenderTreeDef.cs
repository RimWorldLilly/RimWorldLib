using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000C1 RID: 193
	public class PawnRenderTreeDef : Def
	{
		// Token: 0x06000779 RID: 1913 RVA: 0x0002BABD File Offset: 0x00029CBD
		public override IEnumerable<string> ConfigErrors()
		{
			PawnRenderTreeDef.<ConfigErrors>d__1 <ConfigErrors>d__ = new PawnRenderTreeDef.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002BACD File Offset: 0x00029CCD
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			PawnRenderNodeProperties pawnRenderNodeProperties = this.root;
			if (pawnRenderNodeProperties == null)
			{
				return;
			}
			pawnRenderNodeProperties.ResolveReferencesRecursive();
		}

		// Token: 0x04000346 RID: 838
		public PawnRenderNodeProperties root;
	}
}
