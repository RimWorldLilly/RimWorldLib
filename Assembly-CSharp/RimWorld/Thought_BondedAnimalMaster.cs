using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000DFB RID: 3579
	public class Thought_BondedAnimalMaster : Thought_Situational
	{
		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060060E1 RID: 24801 RVA: 0x00230DD1 File Offset: 0x0022EFD1
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_BondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x040035FE RID: 13822
		private const int MaxAnimals = 3;
	}
}
