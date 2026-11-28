using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC3 RID: 8131
	public class StatWorker_MeleeArmorPenetration : StatWorker
	{
		// Token: 0x0600CE76 RID: 52854 RVA: 0x004BFE49 File Offset: 0x004BE049
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x0600CE77 RID: 52855 RVA: 0x004BFE66 File Offset: 0x004BE066
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				string str = "Getting MeleeArmorPenetration stat for ";
				Def def = req.Def;
				Log.Error(str + ((def != null) ? def.ToString() : null) + " without concrete pawn. This always returns 0.");
			}
			return this.GetArmorPenetration(req, applyPostProcess);
		}

		// Token: 0x0600CE78 RID: 52856 RVA: 0x004BFEA0 File Offset: 0x004BE0A0
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x0600CE79 RID: 52857 RVA: 0x004BFEBC File Offset: 0x004BE0BC
		private float GetArmorPenetration(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedArmorPenetration(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2;
		}
	}
}
