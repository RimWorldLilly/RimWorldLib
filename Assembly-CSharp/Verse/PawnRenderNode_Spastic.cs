using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003AF RID: 943
	public class PawnRenderNode_Spastic : PawnRenderNode
	{
		// Token: 0x06001C4A RID: 7242 RVA: 0x000AE888 File Offset: 0x000ACA88
		public PawnRenderNode_Spastic(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000AE894 File Offset: 0x000ACA94
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return new GraphicMeshSet(MeshPool.GridPlane(this.props.overrideMeshSize ?? this.props.drawSize));
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000AE8D4 File Offset: 0x000ACAD4
		public bool CheckAndDoSpasm(PawnDrawParms parms, out PawnRenderNode_Spastic.SpasmData dat, out float progress)
		{
			if (!parms.pawn.Dead)
			{
				PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = this.props as PawnRenderNodeProperties_Spastic;
				if (pawnRenderNodeProperties_Spastic != null && !parms.Portrait && !parms.Cache && !parms.Statue)
				{
					if (this.spasmData == null)
					{
						this.spasmData = new PawnRenderNode_Spastic.SpasmData();
					}
					if (Find.TickManager.TicksGame >= this.spasmData.nextSpasm)
					{
						this.spasmData.tickStart = Find.TickManager.TicksGame;
						this.spasmData.duration = (float)this.GetNextSpasmDurationTicks();
						this.spasmData.nextSpasm = this.GetNextSpasmTick();
						this.spasmData.rotationStart = this.spasmData.rotationTarget;
						this.spasmData.rotationTarget = pawnRenderNodeProperties_Spastic.rotationRange.RandomInRange;
						this.spasmData.scaleStart = this.spasmData.scaleTarget;
						this.spasmData.scaleTarget = pawnRenderNodeProperties_Spastic.scaleRange.RandomInRange;
						this.spasmData.offsetStart = this.spasmData.offsetTarget;
						this.spasmData.offsetTarget = new Vector3(pawnRenderNodeProperties_Spastic.offsetRangeX.RandomInRange, 0f, pawnRenderNodeProperties_Spastic.offsetRangeZ.RandomInRange);
					}
					progress = (float)(Find.TickManager.TicksGame - this.spasmData.tickStart) / Mathf.Max(this.spasmData.duration, 0.0001f);
					dat = this.spasmData;
					return true;
				}
			}
			progress = 0f;
			dat = null;
			return false;
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000AEA5C File Offset: 0x000ACC5C
		protected virtual int GetNextSpasmTick()
		{
			PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = this.props as PawnRenderNodeProperties_Spastic;
			if (pawnRenderNodeProperties_Spastic != null)
			{
				return this.spasmData.tickStart + (int)this.spasmData.duration + pawnRenderNodeProperties_Spastic.nextSpasmTicksRange.RandomInRange;
			}
			return 0;
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000AEAA0 File Offset: 0x000ACCA0
		protected virtual int GetNextSpasmDurationTicks()
		{
			PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = this.props as PawnRenderNodeProperties_Spastic;
			if (pawnRenderNodeProperties_Spastic != null)
			{
				return pawnRenderNodeProperties_Spastic.durationTicksRange.RandomInRange;
			}
			return 0;
		}

		// Token: 0x04001705 RID: 5893
		protected PawnRenderNode_Spastic.SpasmData spasmData;

		// Token: 0x020026AD RID: 9901
		public class SpasmData
		{
			// Token: 0x0600F490 RID: 62608 RVA: 0x00594C30 File Offset: 0x00592E30
			public SpasmData()
			{
				this.duration = 1f;
				this.scaleStart = (this.scaleTarget = 1f);
			}

			// Token: 0x04009E85 RID: 40581
			public float rotationStart;

			// Token: 0x04009E86 RID: 40582
			public float rotationTarget;

			// Token: 0x04009E87 RID: 40583
			public float scaleStart;

			// Token: 0x04009E88 RID: 40584
			public float scaleTarget;

			// Token: 0x04009E89 RID: 40585
			public Vector3 offsetStart;

			// Token: 0x04009E8A RID: 40586
			public Vector3 offsetTarget;

			// Token: 0x04009E8B RID: 40587
			public int tickStart;

			// Token: 0x04009E8C RID: 40588
			public int nextSpasm;

			// Token: 0x04009E8D RID: 40589
			public float duration;
		}
	}
}
