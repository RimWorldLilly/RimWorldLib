using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001491 RID: 5265
	public class PawnShamblerScarDrawer : PawnScarDrawer
	{
		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06008119 RID: 33049 RVA: 0x002E0C08 File Offset: 0x002DEE08
		protected override string ScarTexturePath
		{
			get
			{
				return "Things/Pawn/Overlays/ShamblerScars/ShamblerScarOverlay";
			}
		}

		// Token: 0x0600811A RID: 33050 RVA: 0x002E0C0F File Offset: 0x002DEE0F
		public PawnShamblerScarDrawer(Pawn pawn) : base(pawn)
		{
		}
	}
}
