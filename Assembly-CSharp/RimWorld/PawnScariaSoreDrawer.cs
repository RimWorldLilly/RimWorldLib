using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001492 RID: 5266
	public class PawnScariaSoreDrawer : PawnScarDrawer
	{
		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x0600811B RID: 33051 RVA: 0x002E0C18 File Offset: 0x002DEE18
		protected override string ScarTexturePath
		{
			get
			{
				return "Things/Pawn/Overlays/Scaria/ScariaSoresOverlay";
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x0600811C RID: 33052 RVA: 0x002E0C1F File Offset: 0x002DEE1F
		protected override float ScaleFactor
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x0600811D RID: 33053 RVA: 0x002E0C26 File Offset: 0x002DEE26
		public PawnScariaSoreDrawer(Pawn pawn) : base(pawn)
		{
		}
	}
}
