using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003C7 RID: 967
	public class PawnRenderNodeWorker_Beard : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x06001CA8 RID: 7336 RVA: 0x000B0421 File Offset: 0x000AE621
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && (parms.facing != Rot4.North || parms.flipHead);
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000B044C File Offset: 0x000AE64C
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			HeadTypeDef headType = parms.pawn.story.headType;
			Vector3 a = base.OffsetFor(node, parms, out pivot);
			if (parms.facing == Rot4.East)
			{
				a += Vector3.right * headType.beardOffsetXEast;
			}
			else if (parms.facing == Rot4.West)
			{
				a += Vector3.left * headType.beardOffsetXEast;
			}
			return a + (headType.beardOffset + parms.pawn.style.beardDef.GetOffset(headType, parms.facing));
		}
	}
}
