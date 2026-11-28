using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CF RID: 5071
	public class Command_AbilityTrial : Command_AbilitySpeech
	{
		// Token: 0x06007E43 RID: 32323 RVA: 0x002D23D5 File Offset: 0x002D05D5
		public Command_AbilityTrial(Ability ability, Pawn pawn) : base(ability, pawn)
		{
			this.defaultLabel = "Accuse".Translate();
			this.defaultIconColor = Color.white;
		}
	}
}
