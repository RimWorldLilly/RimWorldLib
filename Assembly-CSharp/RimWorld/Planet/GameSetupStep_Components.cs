using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022D3 RID: 8915
	public class GameSetupStep_Components : GameSetupStep
	{
		// Token: 0x1700263A RID: 9786
		// (get) Token: 0x0600DBD9 RID: 56281 RVA: 0x0051E7EB File Offset: 0x0051C9EB
		public override int SeedPart
		{
			get
			{
				return 508565678;
			}
		}

		// Token: 0x0600DBDA RID: 56282 RVA: 0x0051E7F2 File Offset: 0x0051C9F2
		public override void GenerateFresh()
		{
			Find.World.ConstructComponents();
		}

		// Token: 0x0600DBDB RID: 56283 RVA: 0x0051E7FE File Offset: 0x0051C9FE
		public override void GenerateWithoutWorldData()
		{
			this.GenerateFromScribe();
		}

		// Token: 0x0600DBDC RID: 56284 RVA: 0x0051E806 File Offset: 0x0051CA06
		public override void GenerateFromScribe()
		{
			Find.World.ConstructComponents();
			Find.World.ExposeComponents();
		}
	}
}
