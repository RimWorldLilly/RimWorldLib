using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001867 RID: 6247
	public class CompDisruptorFlarePack : CompAIUsablePack
	{
		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x06009C4D RID: 40013 RVA: 0x00384687 File Offset: 0x00382887
		public new CompProperties_DisruptorFlarePack Props
		{
			get
			{
				return (CompProperties_DisruptorFlarePack)this.props;
			}
		}

		// Token: 0x06009C4E RID: 40014 RVA: 0x00384694 File Offset: 0x00382894
		protected override float ChanceToUse(Pawn wearer)
		{
			return 0f;
		}

		// Token: 0x06009C4F RID: 40015 RVA: 0x0038469B File Offset: 0x0038289B
		protected override void UsePack(Pawn wearer)
		{
			throw new NotImplementedException();
		}
	}
}
