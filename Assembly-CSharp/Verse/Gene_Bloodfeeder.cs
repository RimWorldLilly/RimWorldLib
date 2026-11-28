using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E1 RID: 993
	public class Gene_Bloodfeeder : Gene
	{
		// Token: 0x06001D3F RID: 7487 RVA: 0x000B2880 File Offset: 0x000B0A80
		public override void PostAdd()
		{
			base.PostAdd();
			if (this.pawn.IsPrisonerOfColony && this.pawn.guest != null)
			{
				if (this.pawn.guest.HasInteractionWith((PrisonerInteractionModeDef interaction) => interaction.hideIfNoBloodfeeders))
				{
					this.pawn.guest.SetNoInteraction();
				}
			}
		}
	}
}
