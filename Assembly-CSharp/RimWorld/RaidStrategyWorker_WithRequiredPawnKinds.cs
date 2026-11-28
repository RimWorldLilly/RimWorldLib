using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D20 RID: 3360
	public abstract class RaidStrategyWorker_WithRequiredPawnKinds : RaidStrategyWorker
	{
		// Token: 0x06005E41 RID: 24129
		protected abstract bool MatchesRequiredPawnKind(PawnKindDef kind);

		// Token: 0x06005E42 RID: 24130
		protected abstract int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null);

		// Token: 0x06005E43 RID: 24131 RVA: 0x00227760 File Offset: 0x00225960
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithRequiredPawns(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x00227785 File Offset: 0x00225985
		public bool CanUseWithGroupMaker(PawnGroupMaker groupMaker)
		{
			return groupMaker.options != null && groupMaker.options.Any((PawnGenOption o) => this.MatchesRequiredPawnKind(o.kind));
		}

		// Token: 0x06005E45 RID: 24133 RVA: 0x002277A8 File Offset: 0x002259A8
		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestRequiredPawnCost(faction, groupKind));
		}

		// Token: 0x06005E46 RID: 24134 RVA: 0x002277BF File Offset: 0x002259BF
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestRequiredPawnCost(faction, groupKind);
		}

		// Token: 0x06005E47 RID: 24135 RVA: 0x002277CC File Offset: 0x002259CC
		private float CheapestRequiredPawnCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithRequiredPawns(faction, groupKind);
			if (!enumerable.Any<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get MinimumPoints for ",
					base.GetType().ToString(),
					" for faction ",
					faction.ToString(),
					" but the faction has no groups with the required pawn kind. groupKind=",
					(groupKind != null) ? groupKind.ToString() : null
				}));
				return 99999f;
			}
			float num = 9999999f;
			foreach (PawnGroupMaker pawnGroupMaker in enumerable)
			{
				foreach (PawnGenOption pawnGenOption in from op in pawnGroupMaker.options
				where this.MatchesRequiredPawnKind(op.kind)
				select op)
				{
					if (pawnGenOption.Cost < num)
					{
						num = pawnGenOption.Cost;
					}
				}
			}
			return num;
		}

		// Token: 0x06005E48 RID: 24136 RVA: 0x002278D4 File Offset: 0x00225AD4
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption opt, List<PawnGenOptionWithXenotype> chosenOpts, Faction faction = null)
		{
			return (chosenOpts == null || chosenOpts.Count >= this.MinRequiredPawnsForPoints(pointsTotal, faction) || this.MatchesRequiredPawnKind(opt.kind)) && base.CanUsePawnGenOption(pointsTotal, opt, chosenOpts, faction);
		}

		// Token: 0x06005E49 RID: 24137 RVA: 0x00227908 File Offset: 0x00225B08
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithRequiredPawns(Faction faction, PawnGroupKindDef groupKind)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return Enumerable.Empty<PawnGroupMaker>();
			}
			Predicate<PawnGenOption> <>9__1;
			return faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
			{
				if (gm.kindDef == groupKind && gm.options != null)
				{
					List<PawnGenOption> options = gm.options;
					Predicate<PawnGenOption> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PawnGenOption op) => this.MatchesRequiredPawnKind(op.kind)));
					}
					return options.Any(predicate);
				}
				return false;
			});
		}
	}
}
