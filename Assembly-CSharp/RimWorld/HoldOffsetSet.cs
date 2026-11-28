using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EFF RID: 3839
	public class HoldOffsetSet
	{
		// Token: 0x060063DC RID: 25564 RVA: 0x0023CA88 File Offset: 0x0023AC88
		public HoldOffset Pick(Rot4 rotation)
		{
			if (rotation == Rot4.North)
			{
				return this.northDefault;
			}
			if (rotation == Rot4.East)
			{
				return this.east;
			}
			if (rotation == Rot4.South)
			{
				return this.south;
			}
			if (rotation == Rot4.West)
			{
				return this.west;
			}
			return null;
		}

		// Token: 0x04003896 RID: 14486
		public HoldOffset northDefault;

		// Token: 0x04003897 RID: 14487
		public HoldOffset east;

		// Token: 0x04003898 RID: 14488
		public HoldOffset south;

		// Token: 0x04003899 RID: 14489
		public HoldOffset west;
	}
}
