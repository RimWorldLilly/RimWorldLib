using System;

namespace Verse
{
	// Token: 0x0200045C RID: 1116
	public class HediffComp_MessageBase : HediffComp
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001FBC RID: 8124 RVA: 0x000C0590 File Offset: 0x000BE790
		private HediffCompProperties_MessageBase Props
		{
			get
			{
				return (HediffCompProperties_MessageBase)this.props;
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000C05A0 File Offset: 0x000BE7A0
		protected virtual void Message()
		{
			if (this.Props.onlyMessageForColonistsOrPrisoners && !base.Pawn.IsColonist && !base.Pawn.IsPrisonerOfColony)
			{
				return;
			}
			Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType, true);
		}
	}
}
