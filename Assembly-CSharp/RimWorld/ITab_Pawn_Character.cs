using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6F RID: 7535
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x17002431 RID: 9265
		// (get) Token: 0x0600C342 RID: 49986 RVA: 0x00473C74 File Offset: 0x00471E74
		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (this.SelPawn != null)
				{
					pawn = this.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.InnerPawn;
					}
				}
				if (pawn == null)
				{
					Log.Error("Character tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		// Token: 0x17002432 RID: 9266
		// (get) Token: 0x0600C343 RID: 49987 RVA: 0x00473CBA File Offset: 0x00471EBA
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x0600C344 RID: 49988 RVA: 0x00473CCA File Offset: 0x00471ECA
		public ITab_Pawn_Character()
		{
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x0600C345 RID: 49989 RVA: 0x00473CE8 File Offset: 0x00471EE8
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout) + new Vector2(17f, 17f) * 2f;
		}

		// Token: 0x0600C346 RID: 49990 RVA: 0x00473D20 File Offset: 0x00471F20
		protected override void FillTab()
		{
			this.UpdateSize();
			Vector2 vector = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout);
			CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), this.PawnToShowInfoAbout, null, default(Rect), true);
		}
	}
}
