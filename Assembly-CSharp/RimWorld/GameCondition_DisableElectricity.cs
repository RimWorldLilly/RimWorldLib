using System;

namespace RimWorld
{
	// Token: 0x0200113E RID: 4414
	public class GameCondition_DisableElectricity : GameCondition
	{
		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06006EE4 RID: 28388 RVA: 0x0026DC2A File Offset: 0x0026BE2A
		public override bool ElectricityDisabled
		{
			get
			{
				return true;
			}
		}
	}
}
