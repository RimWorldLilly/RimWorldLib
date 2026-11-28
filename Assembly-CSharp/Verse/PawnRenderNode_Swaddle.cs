using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039D RID: 925
	public class PawnRenderNode_Swaddle : PawnRenderNode
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000AE152 File Offset: 0x000AC352
		public new PawnRenderNodeProperties_Swaddle Props
		{
			get
			{
				return (PawnRenderNodeProperties_Swaddle)this.props;
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x000AE15F File Offset: 0x000AC35F
		public PawnRenderNode_Swaddle(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x000AE16C File Offset: 0x000AC36C
		public override Color ColorFor(Pawn pawn)
		{
			Rand.PushState(pawn.thingIDNumber);
			PawnRenderNodeProperties_Swaddle props = this.Props;
			float randomInRange = props.brightnessRange.RandomInRange;
			float num = Rand.Range(-props.swaddleColorOffset, props.swaddleColorOffset);
			float num2 = Rand.Range(-props.swaddleColorOffset, props.swaddleColorOffset);
			float num3 = Rand.Range(-props.swaddleColorOffset, props.swaddleColorOffset);
			Rand.PopState();
			return new Color(randomInRange + num, randomInRange + num2, randomInRange + num3);
		}
	}
}
