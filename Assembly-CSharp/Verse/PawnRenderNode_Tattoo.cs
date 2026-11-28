using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B0 RID: 944
	public abstract class PawnRenderNode_Tattoo : PawnRenderNode
	{
		// Token: 0x06001C4F RID: 7247 RVA: 0x000AEAC9 File Offset: 0x000ACCC9
		public PawnRenderNode_Tattoo(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000AEAD4 File Offset: 0x000ACCD4
		public override Color ColorFor(Pawn pawn)
		{
			Color result = base.ColorFor(pawn);
			result.a *= 0.8f;
			return result;
		}

		// Token: 0x04001706 RID: 5894
		protected const float TattooOpacity = 0.8f;
	}
}
