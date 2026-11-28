using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000509 RID: 1289
	public static class HumanlikeMeshPoolUtility
	{
		// Token: 0x0600262B RID: 9771 RVA: 0x000E8756 File Offset: 0x000E6956
		public static float HumanlikeBodyWidthForPawn(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.bodyWidth != null)
			{
				return pawn.ageTracker.CurLifeStage.bodyWidth.Value;
			}
			return 1.5f;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000E8794 File Offset: 0x000E6994
		public static float HumanlikeHeadWidthForPawn(Pawn pawn)
		{
			float num = 1.5f;
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				num *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return num;
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000E87E0 File Offset: 0x000E69E0
		public static GraphicMeshSet GetHumanlikeBodySetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			float num = HumanlikeMeshPoolUtility.HumanlikeBodyWidthForPawn(pawn);
			return MeshPool.GetMeshSetForSize(num * wFactor, num * hFactor);
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000E8800 File Offset: 0x000E6A00
		public static GraphicMeshSet GetHumanlikeHeadSetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			float num = HumanlikeMeshPoolUtility.HumanlikeHeadWidthForPawn(pawn);
			return MeshPool.GetMeshSetForSize(num * wFactor, num * hFactor);
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000E8820 File Offset: 0x000E6A20
		public static GraphicMeshSet GetHumanlikeHairSetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			Vector2 vector = pawn.story.headType.hairMeshSize * new Vector2(wFactor, hFactor);
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return MeshPool.GetMeshSetForSize(vector.x, vector.y);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000E8898 File Offset: 0x000E6A98
		public static GraphicMeshSet GetHumanlikeBeardSetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			Vector2 vector = pawn.story.headType.beardMeshSize * new Vector2(wFactor, hFactor);
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return MeshPool.GetMeshSetForSize(vector.x, vector.y);
		}
	}
}
