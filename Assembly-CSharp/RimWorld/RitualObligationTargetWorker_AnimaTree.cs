using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016A7 RID: 5799
	public class RitualObligationTargetWorker_AnimaTree : RitualObligationTargetFilter
	{
		// Token: 0x0600914C RID: 37196 RVA: 0x00353451 File Offset: 0x00351651
		public RitualObligationTargetWorker_AnimaTree()
		{
		}

		// Token: 0x0600914D RID: 37197 RVA: 0x00353459 File Offset: 0x00351659
		public RitualObligationTargetWorker_AnimaTree(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600914E RID: 37198 RVA: 0x00353462 File Offset: 0x00351662
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			return Enumerable.Empty<TargetInfo>();
		}

		// Token: 0x0600914F RID: 37199 RVA: 0x0035346C File Offset: 0x0035166C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			CompPsylinkable compPsylinkable = target.Thing.TryGetComp<CompPsylinkable>();
			if (compPsylinkable == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			foreach (Pawn pawn in target.Map.mapPawns.FreeColonistsSpawned)
			{
				if (compPsylinkable.CanPsylink(pawn, null, false))
				{
					flag2 = true;
				}
				if (compPsylinkable.Props.requiredFocus.CanPawnUse(pawn))
				{
					flag = true;
				}
			}
			if (compPsylinkable.CompSubplant.SubplantsForReading.Count < compPsylinkable.Props.requiredSubplantCountPerPsylinkLevel[0])
			{
				return "RitualTargetAnimaTreeNotEnoughAnimaGrass".Translate(compPsylinkable.Props.requiredSubplantCountPerPsylinkLevel[0]);
			}
			if (!flag)
			{
				return "RitualTargetAnimaTreeNoPawnsWithNatureFocus".Translate();
			}
			if (!flag2)
			{
				return "RitualTargetAnimaTreeNoPawnsToLink".Translate();
			}
			return true;
		}

		// Token: 0x06009150 RID: 37200 RVA: 0x00353588 File Offset: 0x00351788
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			return new RitualObligationTargetWorker_AnimaTree.<GetTargetInfos>d__4(-2);
		}
	}
}
