using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000DFE RID: 3582
	public class Thought_NotBondedAnimalMaster : Thought_Situational
	{
		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060060E7 RID: 24807 RVA: 0x00230E50 File Offset: 0x0022F050
		protected override float BaseMoodOffset
		{
			get
			{
				return base.CurStage.baseMoodEffect * (float)Mathf.Min(((ThoughtWorker_NotBondedAnimalMaster)this.def.Worker).GetAnimalsCount(this.pawn), 3);
			}
		}

		// Token: 0x040035FF RID: 13823
		private const int MaxAnimals = 3;
	}
}
