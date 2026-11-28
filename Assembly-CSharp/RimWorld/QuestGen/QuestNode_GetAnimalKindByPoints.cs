using System;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B6 RID: 8630
	public class QuestNode_GetAnimalKindByPoints : QuestNode
	{
		// Token: 0x0600D6FD RID: 55037 RVA: 0x004FCCC5 File Offset: 0x004FAEC5
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x0600D6FE RID: 55038 RVA: 0x004FCCCE File Offset: 0x004FAECE
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D6FF RID: 55039 RVA: 0x004FCCDC File Offset: 0x004FAEDC
		private bool SetVars(Slate slate)
		{
			float points = slate.Get<float>("points", 0f, false);
			PawnKindDef var;
			if ((from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && !x.RaceProps.Dryad && !x.RaceProps.neverIncludeInQuests && x.combatPower < points
			select x).TryRandomElement(out var))
			{
				slate.Set<PawnKindDef>("animalKindDef", var, false);
				return true;
			}
			return false;
		}
	}
}
