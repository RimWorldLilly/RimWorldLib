using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AA RID: 5290
	public class Recipe_RemoveBodyPart_Cut : Recipe_RemoveBodyPart
	{
		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06008195 RID: 33173 RVA: 0x002E3DBC File Offset: 0x002E1FBC
		protected override bool SpawnPartsWhenRemoved
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008196 RID: 33174 RVA: 0x002E3DC0 File Offset: 0x002E1FC0
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, (BodyPartRecord record) => !pawn.health.hediffSet.PartIsMissing(record));
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x002E3DF2 File Offset: 0x002E1FF2
		public override void ApplyThoughts(Pawn pawn, Pawn billDoer)
		{
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.GenericBrutal);
			}
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x002E3E04 File Offset: 0x002E2004
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return "Cut".Translate();
		}
	}
}
