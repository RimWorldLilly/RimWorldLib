using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159C RID: 5532
	public struct VerbEntry
	{
		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x06008852 RID: 34898 RVA: 0x003158C9 File Offset: 0x00313AC9
		public bool IsMeleeAttack
		{
			get
			{
				return this.verb.IsMeleeAttack;
			}
		}

		// Token: 0x06008853 RID: 34899 RVA: 0x003158D6 File Offset: 0x00313AD6
		public VerbEntry(Verb verb, Pawn pawn, List<Verb> allVerbs, float highestSelWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = VerbUtility.FinalSelectionWeight(verb, pawn, allVerbs, highestSelWeight);
		}

		// Token: 0x06008854 RID: 34900 RVA: 0x003158EF File Offset: 0x00313AEF
		public VerbEntry(Verb verb, float selectionWeight)
		{
			this.verb = verb;
			this.cachedSelectionWeight = selectionWeight;
		}

		// Token: 0x06008855 RID: 34901 RVA: 0x00315900 File Offset: 0x00313B00
		public float GetSelectionWeight(Thing target)
		{
			if (!this.verb.IsUsableOn(target))
			{
				return 0f;
			}
			float num = this.cachedSelectionWeight;
			bool? flag;
			if (target == null)
			{
				flag = null;
			}
			else
			{
				ThingDef def = target.def;
				flag = ((def != null) ? new bool?(def.IsEdifice()) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				num *= this.verb.verbProps.commonalityVsEdificeFactor;
			}
			return num;
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x00315973 File Offset: 0x00313B73
		public override string ToString()
		{
			Verb verb = this.verb;
			return ((verb != null) ? verb.ToString() : null) + " - " + this.cachedSelectionWeight.ToString();
		}

		// Token: 0x04005413 RID: 21523
		public Verb verb;

		// Token: 0x04005414 RID: 21524
		private float cachedSelectionWeight;
	}
}
